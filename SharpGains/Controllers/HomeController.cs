using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Controllers
{
    public class HomeController : Controller
    {
        private RepositoryUsuarios repo;

        public HomeController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            string idUsuario = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (idUsuario != null)
            {
                Usuario usuario = await this.repo.GetUsuario(int.Parse(idUsuario));
                ViewBag.USUARIO = usuario;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
