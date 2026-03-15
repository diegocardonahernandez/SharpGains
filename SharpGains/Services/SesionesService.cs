using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Services
{
    public class SesionesService
    {
        private RepositorySesiones repoSesiones;

        public SesionesService(RepositorySesiones repoSesiones)
        {
            this.repoSesiones = repoSesiones;
        }

        public async Task<Rutina?> GetRutinaConEjerciciosParaSesionAsync(int idRutina)
        {
            return await this.repoSesiones.GetRutinaConEjerciciosParaSesionAsync(idRutina);
        }

        public async Task<int> FinalizarSesionAsync(int idUsuario, int idRutina, string? notas, string jsonSeries)
        {
            return await this.repoSesiones.FinalizarSesionAsync(idUsuario, idRutina, notas, jsonSeries);
        }
    }
}
