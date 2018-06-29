using Microsoft.AspNetCore.Mvc;

namespace Blue.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}