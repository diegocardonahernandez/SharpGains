using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Controllers
{
    public class RutinasController : Controller
    {
        private RepositoryRutinas repo;
        private RepositoryEjercicios repoEjercicios;

        public RutinasController (RepositoryRutinas repo, RepositoryEjercicios repoEjercicios)
        {

            this.repo = repo;
            this.repoEjercicios = repoEjercicios;
        }
        public async Task<IActionResult> CrearRutina()
        {
            List<Ejercicio> ejercicios = await this.repoEjercicios.GetEjercicios();
            return View(ejercicios);
        }
    }
}
