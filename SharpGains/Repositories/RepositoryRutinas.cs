using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharpGains.Data;
using SharpGains.Models;

namespace SharpGains.Repositories
{

    #region STORED PROCEDURES
    //    CREATE PROCEDURE SP_PAGINAR_EJERCICIOS_GP
    //    (@grupoMuscular NVARCHAR(50), @posicion INT, @registros INT OUT)
    //AS
    //    SELECT @registros = count(id) FROM EJERCICIO
    //    WHERE grupo_muscular = @grupoMuscular

    //    SELECT* FROM
    //        (SELECT CAST(ROW_NUMBER() OVER (ORDER BY nombre) AS INT)
    //		AS POSICION, * FROM EJERCICIO
    //        WHERE grupo_muscular = @grupoMuscular) QUERY
    //        WHERE(QUERY.POSICION >= @posicion AND QUERY.POSICION<(@posicion + 3))
    //GO

    // -----------------------

//    CREATE PROCEDURE SP_CREAR_RUTINA
//    @id_usuario INT,
//    @nombre_rutina NVARCHAR(150),
//    @json_ejercicios NVARCHAR(MAX)
//AS
//BEGIN
//    SET NOCOUNT ON;
//    DECLARE @nuevo_id_rutina INT;
//    BEGIN TRAN;

//    BEGIN TRY
//        SELECT @nuevo_id_rutina = ISNULL(MAX(id), 0) + 1 FROM RUTINA;

//    INSERT INTO RUTINA
//    VALUES(@nuevo_id_rutina, @id_usuario, @nombre_rutina);

//    INSERT INTO EJERCICIO_RUTINA(id_rutina, id_ejercicio, series_objetivo, repeticiones_objetivo, orden)
//        SELECT
//            @nuevo_id_rutina,
//            idEjercicio,
//            seriesObjetivo,
//            repeticionesObjetivo,
//            orden
//        FROM OPENJSON(@json_ejercicios)
//        WITH(
//            idEjercicio INT '$.IdEjercicio',
//            seriesObjetivo INT '$.SeriesObjetivo',
//            repeticionesObjetivo INT '$.RepeticionesObjetivo',
//            orden INT '$.Orden'
//        );

//    COMMIT TRAN;

//    END TRY
//    BEGIN CATCH
//        ROLLBACK TRAN;
//    THROW;
//    END CATCH
//END
//GO
    #endregion
    public class RepositoryRutinas
    {
        private SharpGainsContext context;

        public RepositoryRutinas(SharpGainsContext context)
        {
            this.context = context;
        }

        public async Task<int> CrearRutinaAsync(int idUsuario, string nombreRutina, string jsonEjercicios)
        {
            string sql = "SP_CREAR_RUTINA @id_usuario, @nombre_rutina, @json_ejercicios";
            SqlParameter pamIdUsuario = new SqlParameter("@id_usuario", idUsuario);
            SqlParameter pamNombreRutina = new SqlParameter("@nombre_rutina", nombreRutina);
            SqlParameter pamJsonEjercicios = new SqlParameter("@json_ejercicios", jsonEjercicios);
            int resultado = await this.context.Database.ExecuteSqlRawAsync(sql, pamIdUsuario, pamNombreRutina, pamJsonEjercicios);
            return resultado;
        }
    }
}
