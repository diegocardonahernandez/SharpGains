using Microsoft.Data.SqlClient;
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

        public async Task<List<Ejercicio>> GetEjerciciosGrupoMuscular(string grupoMuscular)
        {
            var consulta = from datos in this.context.Ejercicios
                           where datos.GrupoMuscular == grupoMuscular select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Ejercicio> GetEjercicioDetails(int id)
        {
            var consulta = from datos in this.context.Ejercicios
                           where datos.Id == id
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<ModelEjerciciosPaginados> GetEjerciciosGrupoMuscularPaginados(string grupoMusuclar, int posicion)
        {
            var sql = "SP_PAGINAR_EJERCICIOS_GP @grupoMuscular, @posicion, @registros OUT";
            SqlParameter pamGrupoMuscular = new SqlParameter("@grupoMuscular", grupoMusuclar);
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamRegistros = new SqlParameter("@registros", 1);
            pamRegistros.DbType = System.Data.DbType.Int32;
            pamRegistros.Direction = System.Data.ParameterDirection.Output;
            var consulta = this.context.Ejercicios.FromSqlRaw(sql, pamGrupoMuscular, pamPosicion, pamRegistros);

            List<Ejercicio> ejerciciosGrupo = await consulta.ToListAsync();
            int registros = (int) pamRegistros.Value;

            ModelEjerciciosPaginados model = new ModelEjerciciosPaginados
            {
                Ejercicios = ejerciciosGrupo,
                Registros = registros
            };

            return model;

        }

    }
}
