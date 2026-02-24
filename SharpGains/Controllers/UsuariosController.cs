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

        public async Task <IActionResult> Registro(Usuario nuevoUsuario)
        {
            int registros = await this.repo.RegistrarUsuarioAsync(nuevoUsuario.Nombre, nuevoUsuario.Apellidos, nuevoUsuario.Correo,
                nuevoUsuario.Contrasena, (decimal)nuevoUsuario.Altura, (decimal)nuevoUsuario.Peso);

                if(registros <= 0)
            {
                ViewBag.ERROR = "Error";
            }
            else
            {
                ViewBag.MENSAJE = "Usuario creado correctamente";
            }

            return View();
            
        }

    }
}
