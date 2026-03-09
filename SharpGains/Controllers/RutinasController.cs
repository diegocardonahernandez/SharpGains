using Microsoft.AspNetCore.Mvc;
using SharpGains.Models;
using SharpGains.Repositories;
using SharpGains.Services;

namespace SharpGains.Controllers
{
    public class RutinasController : Controller
    {
        private CrearRutinaService service;

        public RutinasController (CrearRutinaService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> CrearRutina(string? grupoMuscular, int? posicion)
        {
            if(grupoMuscular == null)
            {
                return View();
            }
            else
            {
                if(posicion == null)
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

        //public async Task<IActionResult> AnadirEjercicioRutina(EjercicioRutina newEjercicioRutina)
        //{

        //}

    }
}
