using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Controllers
{
    public class EjerciciosController : Controller
    {
        private RepositoryEjercicios repo;

        public EjerciciosController(RepositoryEjercicios repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Ejercicios()
        {
            List<Ejercicio> ejercicios = await this.repo.GetEjercicios();
            return View(ejercicios);
        }
    }
}
 