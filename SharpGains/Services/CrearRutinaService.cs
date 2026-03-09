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

        public async Task<List<Ejercicio>> GetEjerciciosAsync()
        {
            return await this.repoEjercicios.GetEjercicios();
        }

        public async Task<List<Ejercicio>> GetEjerciciosGrupoMuscularAsync(string grupoMuscular)
        {
            return await this.repoEjercicios.GetEjerciciosGrupoMuscular(grupoMuscular);
        }

        public async Task<ModelEjerciciosPaginados> GetEjerciciosGPpaginados(string grupoMuscular, int posicion)
        {
            return await this.repoEjercicios.GetEjerciciosGrupoMuscularPaginados(grupoMuscular, posicion);
        }

    }
}
