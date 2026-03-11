using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Services;

namespace SharpGains.Controllers
{
    public class RutinasController : Controller
    {
        private CrearRutinaService service;

        public RutinasController(CrearRutinaService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> CrearRutina(string? grupoMuscular, int? posicion)
        {
            if (grupoMuscular == null)
            {
                return View();
            }
            else
            {
                if (posicion == null)
                {
                    posicion = 1;
                }

                ModelEjerciciosPaginados model = await this.service.GetEjerciciosGPpaginados(grupoMuscular, posicion.Value);
                int numRegistros = model.Registros;

                int siguiente = posicion.Value + 3;
                if (siguiente > numRegistros)
                {
                    siguiente = posicion.Value;
                }

                int anterior = posicion.Value - 3;
                if (anterior < 1)
                {
                    anterior = 1;
                }

                int ultimaPagina = numRegistros - ((numRegistros - 1) % 3);

                ViewData["ULTIMO"] = ultimaPagina;
                ViewData["SIGUIENTE"] = siguiente;
                ViewData["ANTERIOR"] = anterior;
                ViewData["GRUPO"] = grupoMuscular;

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearRutina([FromBody] CrearRutinaRequest request)
        {
            string? userId = HttpContext.Session.GetString("IDUSUARIOLOGEADO");
            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return BadRequest(new { error = "El nombre de la rutina es obligatorio." });
            }

            if (request.Ejercicios == null || request.Ejercicios.Count == 0)
            {
                return BadRequest(new { error = "Debes añadir al menos un ejercicio." });
            }

            int idUsuario = int.Parse(userId);
            string jsonEjercicios = JsonSerializer.Serialize(request.Ejercicios);

            await this.service.CrearRutinaAsync(idUsuario, request.Nombre, jsonEjercicios);

            return Ok(new { mensaje = "Rutina creada correctamente." });
        }
    }

    public class CrearRutinaRequest
    {
        public string Nombre { get; set; } = null!;
        public List<EjercicioRutinaRequest> Ejercicios { get; set; } = [];
    }

    public class EjercicioRutinaRequest
    {
        public int IdEjercicio { get; set; }
        public int SeriesObjetivo { get; set; }
        public int RepeticionesObjetivo { get; set; }
        public int Orden { get; set; }
    }
}
