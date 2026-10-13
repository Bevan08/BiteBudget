
using BiteBudget.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiteBudget.Controllers
{
    public class MealsController : Controller
    {
        private readonly BiteBudgetContext _context;

        public MealsController(BiteBudgetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            decimal? budget,
            string preference = "All")
        {
            var meals = _context.Meals.AsQueryable();

            if (budget.HasValue && budget.Value > 0)
            {
                meals = meals.Where(m => m.Price <= budget.Value);
                ViewBag.Budget = budget.Value;
            }

            if (preference == "Vegetarian")
            {
                meals = meals.Where(m => m.IsVegetarian);
            }
            else if (preference == "NonVegetarian")
            {
                meals = meals.Where(m => !m.IsVegetarian);
            }

            ViewBag.Preference = preference;

            return View(await meals.OrderBy(m => m.Price).ToListAsync());
        }
    }
}
