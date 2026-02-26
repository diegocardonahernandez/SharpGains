using Microsoft.AspNetCore.Mvc;

namespace SharpGains.Controllers
{
    public class EjerciciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
 