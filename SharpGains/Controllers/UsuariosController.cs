using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrWhiteSpace(correo))
            {
                return Content("", "text/html");
            }
            string patronRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(correo, patronRegex))
            {
                return Content("<span class='sg-feedback-invalid'>Formato de correo inválido.</span>", "text/html");
            }

            Usuario usuario = await this.repo.BuscarUsuario(correo);

            if (usuario != null)
            {
                return Content("<span class='sg-feedback-taken'>Este correo ya está en uso.</span>", "text/html");
            }

            return Content("<span class='sg-feedback-available'>Correo disponible.</span>", "text/html");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            Usuario usuarioLogeado = await this.repo.Login(correo, contrasena);

                if(usuarioLogeado == null)
            {
                ViewBag.ERROR = "Error, el correo o la contraseña no son correctos";
                return View();
            }
            else
            {
                string idusuarioLogeado = "";
                if (HttpContext.Session.GetString("IDUSUARIOLOGEADO") != null)
                {
                    idusuarioLogeado = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
                }

                HttpContext.Session.SetString("IDUSUARIOLOGEADO", usuarioLogeado.Id.ToString());
            }

            return RedirectToAction("index", "Home");

        }

        public async Task<IActionResult> Perfil()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIOLOGEADO"));
            Usuario usuario = await this.repo.GetUsuarioConDatos(idUsuario);

            int totalRutinas = usuario.Rutinas.Count;
            int totalSesiones = usuario.Sesions.Count;
            int totalSeries = usuario.Sesions.SelectMany(s => s.Series).Count();
            decimal volumenTotal = usuario.Sesions
                .SelectMany(s => s.Series)
                .Sum(s => s.Peso * s.Repeticiones);

            ViewBag.TotalRutinas = totalRutinas;
            ViewBag.TotalSesiones = totalSesiones;
            ViewBag.TotalSeries = totalSeries;
            ViewBag.VolumenTotal = volumenTotal;

            return View(usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("IDUSUARIOLOGEADO");
            return RedirectToAction("index", "home");

         }

    }
}
