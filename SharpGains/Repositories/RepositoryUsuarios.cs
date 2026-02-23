using Microsoft.AspNetCore.Http.HttpResults;
using SharpGains.Data;
using SharpGains.Models;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpGains.Repositories
{
    public class RepositoryUsuarios
    {
        #region STORED PROCEDURES
        //REGISTRO DE USUARIO:

        //        CREATE PROCEDURE SP_REGISTER_USER
        //              (@nombre NVARCHAR(100),
        //	            @apellidos NVARCHAR(150),
        //	            @correo NVARCHAR(255),
        //	            @contrasena NVARCHAR(MAX),
        //	            @altura DECIMAL(5,2),
        //	            @peso DECIMAL(5,2))
        //      AS
        //           DECLARE @idnuevousuario INT
        //           SELECT @idnuevousuario = ISNULL(MAX(id), 0) + 1 FROM USUARIO

        //          INSERT INTO USUARIO
        //          VALUES(@idnuevousuario, @nombre, @apellidos, @correo, @contrasena, @altura, @peso)
        //      GO
        //      ------------------------
        #endregion

        private SharpGainsContext context;

        public RepositoryUsuarios (SharpGainsContext context)
        {
            this.context = context;
        }

    }
}
