using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharpGains.Data;
using SharpGains.Models;

namespace SharpGains.Repositories
{
    #region STORED PROCEDURES

    //    CREATE PROCEDURE SP_FINALIZAR_SESION
    //    @id_usuario INT,
    //    @id_rutina INT,
    //    @notas NVARCHAR(MAX),
    //    @json_series NVARCHAR(MAX)
    //AS
    //BEGIN
    //    SET NOCOUNT ON;
    //    DECLARE @nuevo_id_sesion INT;
    //    DECLARE @max_id_serie INT;

    //    BEGIN TRAN;

    //    BEGIN TRY
    //        SELECT @nuevo_id_sesion = ISNULL(MAX(id), 0) + 1 FROM SESION;

    //    INSERT INTO SESION(id, id_usuario, id_rutina, fecha_inicio, fecha_fin, notas)
    //        VALUES(@nuevo_id_sesion, @id_usuario, @id_rutina, GETDATE(), GETDATE(), @notas);

    //    SELECT @max_id_serie = ISNULL(MAX(id), 0) FROM SERIE;

    //    INSERT INTO SERIE(id, id_sesion, id_ejercicio, numero_serie, peso, repeticiones, rpe)
    //        SELECT
    //            @max_id_serie + ROW_NUMBER() OVER(ORDER BY IdEjercicio, NumeroSerie),
    //            @nuevo_id_sesion,
    //            IdEjercicio,
    //            NumeroSerie,
    //            Peso,
    //            Repeticiones,
    //            Rpe
    //        FROM OPENJSON(@json_series)
    //        WITH(
    //            IdEjercicio INT '$.IdEjercicio',
    //            NumeroSerie INT '$.NumeroSerie',
    //            Peso DECIMAL(6,2) '$.Peso',
    //            Repeticiones INT '$.Repeticiones',
    //            Rpe INT '$.Rpe'
    //        );
    //        COMMIT TRAN;

    //    END TRY
    //    BEGIN CATCH
    //        ROLLBACK TRAN;
    //    THROW;
    //    END CATCH
    //END
    //GO

    #endregion

    public class RepositorySesiones
    {
        private SharpGainsContext context;

        public RepositorySesiones(SharpGainsContext context)
        {
            this.context = context;
        }

        public async Task<Rutina?> GetRutinaConEjerciciosParaSesionAsync(int idRutina)
        {
            Rutina? rutina = await this.context.Rutinas
                .Include(r => r.EjercicioRutinas)
                .ThenInclude(er => er.IdEjercicioNavigation)
                .FirstOrDefaultAsync(r => r.Id == idRutina);

            return rutina;
        }

        public async Task<int> FinalizarSesionAsync(int idUsuario, int idRutina, string? notas, string jsonSeries)
        {
            string sql = "SP_FINALIZAR_SESION @id_usuario, @id_rutina, @notas, @json_series";

            SqlParameter pamIdUsuario = new SqlParameter("@id_usuario", idUsuario);
            SqlParameter pamIdRutina = new SqlParameter("@id_rutina", idRutina);
            SqlParameter pamNotas = new SqlParameter("@notas", (object?)notas ?? DBNull.Value);
            SqlParameter pamJsonSeries = new SqlParameter("@json_series", jsonSeries);

            int result = await this.context.Database.ExecuteSqlRawAsync(
                sql,
                pamIdUsuario,
                pamIdRutina,
                pamNotas,
                pamJsonSeries
            );

            return result;
        }
    }
}
