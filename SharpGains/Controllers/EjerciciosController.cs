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

        [HttpGet]
        public async Task<IActionResult> Buscar(string? busqueda)
        {
            List<Ejercicio> ejercicios = await this.repo.GetEjercicios();
            IQueryable<Ejercicio> resultados = ejercicios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                resultados = resultados.Where(e =>
                    e.Nombre.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
            }

            return PartialView("_ResultadosBusquedaEjercicios", resultados.ToList());
        }
    }
}