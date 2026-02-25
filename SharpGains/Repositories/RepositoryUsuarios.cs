using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SharpGains.Data;
using SharpGains.Models;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpGains.Repositories
{
    public class RepositoryUsuarios
    {
        #region STORED PROCEDURES
        //REGISTRO:

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

        //LOGIN:

 //       CREATE PROCEDURE SP_LOGIN_USUARIO(@correo NVARCHAR(255), @contrasena NVARCHAR(256))
//          AS
 //            SELECT* FROM USUARIO
 //            WHERE correo = @correo AND contrasena = @contrasena
 //         GO
        #endregion

        private SharpGainsContext context;

        public RepositoryUsuarios (SharpGainsContext context)
        {
            this.context = context;
        }

        public async Task<int> RegistrarUsuarioAsync
            (string nombre, string apellidos, string correo, string contrasena, decimal altura, decimal peso)
        {
       
            var passwordHasher = new PasswordHasher<Usuario>();
            var usuarioTemporal = new Usuario();
            string hashGenerado = passwordHasher.HashPassword(usuarioTemporal, contrasena);
                
            string sql = "SP_REGISTER_USER @nombre, @apellidos, @correo, @contrasena, @altura, @peso";
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellidos = new SqlParameter("@apellidos", apellidos);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamHashContrasena = new SqlParameter("@contrasena", hashGenerado);
            SqlParameter pamAltura = new SqlParameter("@altura", altura);
            SqlParameter pamPeso = new SqlParameter("@peso", peso);
            int registros = await this.context.Database.ExecuteSqlRawAsync(sql, pamNombre, pamApellidos, pamCorreo, pamHashContrasena, pamAltura, pamPeso);
            return registros;
        }

        public async Task<Usuario> BuscarUsuario(string correo)
        {
            return await this.context.Usuarios
                .Where(u => u.Correo == correo)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> Login(string correo, string contrasena)
        {
            string sql = "SP_LOGIN_USUARIO @correo, @contrasena";
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContrasena = new SqlParameter("@contrasena", contrasena);
            var consulta = await this.context.Usuarios.FromSqlRaw(sql, correo, contrasena).ToListAsync();
            Usuario usuario = consulta.FirstOrDefault();
            return usuario;

        }

    }
}
