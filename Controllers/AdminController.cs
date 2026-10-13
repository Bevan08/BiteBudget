
using BiteBudget.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiteBudget.Controllers
{
    public class AdminController : Controller
    {
        private readonly BiteBudgetContext _context;

        public AdminController(BiteBudgetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalOrders = await _context.Orders.CountAsync();

            ViewBag.TotalRevenue = await _context.Orders
                .SumAsync(o => (decimal?)o.Total) ?? 0m;

            ViewBag.TotalMeals = await _context.Meals.CountAsync();

            ViewBag.RecentOrders = await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
