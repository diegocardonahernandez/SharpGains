using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Services;

namespace SharpGains.Controllers
{
    public class SesionesController : Controller
    {
        private SesionesService service;

        public SesionesController(SesionesService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Sesion(int idRutina)
        {
            string? idUsuarioLogeado = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (idUsuarioLogeado == null)
            {
                TempData["ERROR"] = "Debes iniciar sesión para acceder a tu sesión de entrenamiento.";
                return RedirectToAction("Login", "Usuarios");
            }

            Rutina? rutina = await this.service.GetRutinaConEjerciciosParaSesionAsync(idRutina);
            if (rutina == null)
            {
                TempData["ERROR"] = "La rutina seleccionada no existe o ya no está disponible.";
                return RedirectToAction("Perfil", "Usuarios");
            }

            int idUsuario = int.Parse(idUsuarioLogeado);
            if (rutina.IdUsuario != idUsuario)
            {
                TempData["ERROR"] = "No tienes permisos para entrenar esta rutina.";
                return RedirectToAction("Perfil", "Usuarios");
            }

            rutina.EjercicioRutinas = rutina.EjercicioRutinas
                .OrderBy(er => er.Orden)
                .ToList();

            return View(rutina);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarSesion([FromBody] FinalizarSesionRequest request)
        {
            string? idUsuarioLogeado = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (idUsuarioLogeado == null)
            {
                return Unauthorized();
            }

            if (request.IdRutina <= 0)
            {
                return BadRequest(new { error = "La rutina es obligatoria." });
            }

            if (request.Series == null || request.Series.Count == 0)
            {
                return BadRequest(new { error = "Debes enviar al menos una serie." });
            }

            int idUsuario = int.Parse(idUsuarioLogeado);

            Rutina? rutina = await this.service.GetRutinaConEjerciciosParaSesionAsync(request.IdRutina);
            if (rutina == null || rutina.IdUsuario != idUsuario)
            {
                return BadRequest(new { error = "La rutina no es válida para este usuario." });
            }

            string jsonSeries = JsonSerializer.Serialize(request.Series);

            await this.service.FinalizarSesionAsync(
                idUsuario,
                request.IdRutina,
                request.Notas,
                jsonSeries
            );

            return Ok(new { mensaje = "Sesión finalizada correctamente." });
        }
    }

    public class FinalizarSesionRequest
    {
        public int IdRutina { get; set; }
        public string? Notas { get; set; }
        public List<FinalizarSerieRequest> Series { get; set; } = [];
    }

    public class FinalizarSerieRequest
    {
        public int IdEjercicio { get; set; }
        public int NumeroSerie { get; set; }
        public decimal Peso { get; set; }
        public int Repeticiones { get; set; }
        public int? Rpe { get; set; }
    }
}
