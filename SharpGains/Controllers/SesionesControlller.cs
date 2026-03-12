using Microsoft.AspNetCore.Mvc;

namespace SharpGains.Controllers
{
    public class SesionesControlller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
