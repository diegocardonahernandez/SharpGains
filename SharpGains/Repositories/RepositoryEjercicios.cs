using Microsoft.EntityFrameworkCore;
using SharpGains.Data;
using SharpGains.Models;

namespace SharpGains.Repositories
{
    public class RepositoryEjercicios
    {
        private SharpGainsContext context;

        public RepositoryEjercicios (SharpGainsContext context)
        {
            this.context = context;
        }

        public async Task<List<Ejercicio>> GetEjercicios()
        {
            var consulta = from datos in this.context.Ejercicios
                           select datos;
            return await consulta.ToListAsync();
        }

    }
}
