using Microsoft.AspNetCore.Mvc;

namespace BiteBudget.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
