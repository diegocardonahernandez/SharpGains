using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SharpGains.Data;
using SharpGains.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    #endregion
    public class RepositoryRutinas
    {
        private SharpGainsContext context;

        public RepositoryRutinas(SharpGainsContext context)
        {
            this.context = context;
        }

    }
}
