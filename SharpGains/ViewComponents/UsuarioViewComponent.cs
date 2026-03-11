using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.ViewComponents
{
    public class UsuarioViewComponent : ViewComponent
    {
        private RepositoryUsuarios repo;

        public UsuarioViewComponent(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? idUsuario = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (idUsuario != null)
            {
                Usuario usuario = await this.repo.GetUsuario(int.Parse(idUsuario));
                return View(usuario);
            }
            return View((Usuario?)null);
        }
    }
}
