
using System.Text.Json;
using BiteBudget.Data;
using BiteBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiteBudget.Controllers
{
    public class CartController : Controller
    {
        private readonly BiteBudgetContext _context;
        private const string CartKey = "BiteBudgetCart";

        public CartController(BiteBudgetContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json)
                    ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(
                CartKey, JsonSerializer.Serialize(cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int mealId)
        {
            if (!await _context.Meals.AnyAsync(m => m.Id == mealId))
                return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MealId == mealId);

            if (item == null)
                cart.Add(new CartItem { MealId = mealId, Quantity = 1 });
            else
                item.Quantity++;

            SaveCart(cart);
            TempData["Message"] = "Meal added to your cart!";
            return RedirectToAction("Index", "Meals");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int mealId)
        {
            var cart = GetCart();
            cart.RemoveAll(x => x.MealId == mealId);
            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var meals = await _context.Meals
                .Where(m => cart.Select(c => c.MealId).Contains(m.Id))
                .ToListAsync();

            var lines = cart.Join(
                meals,
                c => c.MealId,
                m => m.Id,
                (c, m) => new CartLineViewModel
                {
                    Meal = m,
                    Quantity = c.Quantity
                }).ToList();

            return View(lines);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction(nameof(Index));

            var ids = cart.Select(c => c.MealId).ToList();
            var meals = await _context.Meals
                .Where(m => ids.Contains(m.Id)).ToListAsync();

            ViewBag.Subtotal = cart.Sum(c =>
                meals.First(m => m.Id == c.MealId).Price * c.Quantity);
            ViewBag.Delivery = 30m;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(
            string customerName,
            string deliveryAddress,
            string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(customerName) ||
                string.IsNullOrWhiteSpace(deliveryAddress))
            {
                ModelState.AddModelError("", "Enter your name and delivery address.");
                return await CheckoutWithErrors();
            }

            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction(nameof(Index));

            var ids = cart.Select(c => c.MealId).ToList();
            var meals = await _context.Meals
                .Where(m => ids.Contains(m.Id)).ToListAsync();

            if (meals.Count != ids.Distinct().Count())
                return BadRequest("A meal in your cart no longer exists.");

            var subtotal = cart.Sum(c =>
                meals.First(m => m.Id == c.MealId).Price * c.Quantity);
            var delivery = subtotal >= 300 ? 0m : 30m;

            var order = new Order
            {
                CustomerName = customerName.Trim(),
                DeliveryAddress = deliveryAddress.Trim(),
                Subtotal = subtotal,
                DeliveryCharge = delivery,
                Total = subtotal + delivery,
                PaymentMethod = paymentMethod == "Demo Online Payment"
                    ? "Demo Online Payment" : "Pay on Delivery",
                PaymentStatus = paymentMethod == "Demo Online Payment"
                    ? "Demo - Not Charged" : "Pending",
                Items = cart.Select(c =>
                {
                    var meal = meals.First(m => m.Id == c.MealId);
                    return new OrderItem
                    {
                        MealId = meal.Id,
                        MealName = meal.Name,
                        UnitPrice = meal.Price,
                        Quantity = c.Quantity
                    };
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);
            return RedirectToAction(nameof(Confirmation),
                new { id = order.Id });
        }

        private Task<IActionResult> CheckoutWithErrors()
        {
            ViewBag.Subtotal = 0m;
            ViewBag.Delivery = 30m;
            return Task.FromResult<IActionResult>(View("Checkout"));
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
