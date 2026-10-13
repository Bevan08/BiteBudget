
using BiteBudget.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiteBudget.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BiteBudgetContext _context;

        public OrdersController(BiteBudgetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }
    }
}
