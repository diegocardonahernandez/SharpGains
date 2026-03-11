using SharpGains.Models;
using SharpGains.Repositories;

namespace SharpGains.Services
{
    public class CrearRutinaService
    {
        private RepositoryEjercicios repoEjercicios;
        private RepositoryRutinas repoRutinas;

        public CrearRutinaService(RepositoryEjercicios repoEjercicios, RepositoryRutinas repoRutinas)
        {
            this.repoEjercicios = repoEjercicios;
            this.repoRutinas = repoRutinas;
        }

        public async Task<ModelEjerciciosPaginados> GetEjerciciosGPpaginados(string grupoMuscular, int posicion)
        {
            return await this.repoEjercicios.GetEjerciciosGrupoMuscularPaginados(grupoMuscular, posicion);
        }

        public async Task<int> CrearRutinaAsync(int idUsuario, string nombreRutina, string jsonEjercicios)
        {
            return await this.repoRutinas.CrearRutinaAsync(idUsuario, nombreRutina, jsonEjercicios);
        }
    }
}
