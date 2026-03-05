using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Controllers
{
    public class EjerciciosController : Controller
    {
        private RepositoryEjercicios repo;
        private RepositoryUsuarios repoUsuarios;

        public EjerciciosController(RepositoryEjercicios repo, RepositoryUsuarios repoUsuarios)
        {
            this.repo = repo;
            this.repoUsuarios = repoUsuarios;
        }

        public async Task<IActionResult> Ejercicios()
        {
            List<Ejercicio> ejercicios = await this.repo.GetEjercicios();
            ViewBag.USUARIO = await this.GetUsuarioLogueado();
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

        private async Task<Usuario?> GetUsuarioLogueado()
        {
            string? userId = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (userId == null) return null;
            return await this.repoUsuarios.GetUsuario(int.Parse(userId));
        }
    }
}