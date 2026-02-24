using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuarios repo;

        public UsuariosController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario nuevoUsuario)
        {

            if (await this.repo.BuscarUsuario(nuevoUsuario.Correo) is not null)
            {
                ViewBag.ERROR = "El correo ya está registrado.";
                return View(nuevoUsuario);
            }

            var registros = await this.repo.RegistrarUsuarioAsync(
                nuevoUsuario.Nombre,
                nuevoUsuario.Apellidos,
                nuevoUsuario.Correo,
                nuevoUsuario.Contrasena,
                (decimal)nuevoUsuario.Altura,
                (decimal)nuevoUsuario.Peso);

            if (registros <= 0)
            {
                ViewBag.ERROR = "No se pudo crear el usuario.";
                return View(nuevoUsuario);
            }

            ViewBag.MENSAJE = "Usuario creado correctamente";
            ModelState.Clear();
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> ValidarCorreo(string correo)
        {
            Usuario usuario = await this.repo.BuscarUsuario(correo);
                if(usuario != null)
            {
                return Content("<span class='text-danger'>Este correo ya está en uso.</span>", "text/html");
            }
            return Content("<span class='text-success'>Correo disponible.</span>", "text/html");
        }

    }
}
