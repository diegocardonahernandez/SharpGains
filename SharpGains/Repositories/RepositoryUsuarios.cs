using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SharpGains.Data;
using SharpGains.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpGains.Repositories
{
    public class RepositoryUsuarios
    {
        #region STORED PROCEDURES
        //REGISTRO:
        //        CREATE PROCEDURE SP_REGISTER_USER
        //            @nombre NVARCHAR(100),
        //    @apellidos NVARCHAR(150),
        //    @correo NVARCHAR(255),
        //    @contrasena NVARCHAR(255), 
        //    @password_hash NVARCHAR(MAX),
        //    @altura DECIMAL(5,2),
        //    @peso DECIMAL(5,2)
        //AS
        //BEGIN
        //    DECLARE @idnuevousuario INT;
        //    SELECT @idnuevousuario = ISNULL(MAX(id), 0) + 1 FROM USUARIO;

        //        BEGIN TRAN;
        //        BEGIN TRY
        //        INSERT INTO USUARIO
        //        VALUES(@idnuevousuario, @nombre, @apellidos, @correo, @contrasena, @altura, @peso);

        //        INSERT INTO USUARIO_SEGURIDAD
        //        VALUES(@idnuevousuario, @password_hash);

        //        COMMIT TRAN;
        //        END TRY
        //    BEGIN CATCH
        //        ROLLBACK TRAN;
        //        THROW;
        //    END CATCH
        //END
        //GO
        //      ------------------------

        //LOGIN:

//        CREATE PROCEDURE SP_LOGIN_USUARIO(@correo NVARCHAR(255))
//          AS        
//              SELECT U.*, US.password_hash
//              FROM USUARIO U
//              INNER JOIN USUARIO_SEGURIDAD US ON U.id = US.id_usuario
//              WHERE U.correo = @correo
//          GO
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
                
            string sql = "SP_REGISTER_USER @nombre, @apellidos, @correo, @contrasena, @password_hash, @altura, @peso";
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellidos = new SqlParameter("@apellidos", apellidos);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContrasena = new SqlParameter("@contrasena", contrasena);
            SqlParameter pamHashContrasena = new SqlParameter("@password_hash", hashGenerado);
            SqlParameter pamAltura = new SqlParameter("@altura", altura);
            SqlParameter pamPeso = new SqlParameter("@peso", peso);
            int registros = await this.context.Database.ExecuteSqlRawAsync(sql, pamNombre, pamApellidos, pamCorreo,pamContrasena, pamHashContrasena, pamAltura, pamPeso);
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
            string sql = "SP_LOGIN_USUARIO @correo";
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);

            var consulta = await this.context.Usuarios.FromSqlRaw(sql, pamCorreo).ToListAsync();
            Usuario usuario = consulta.FirstOrDefault();

            if (usuario != null)
            {
                var seguridad = await this.context.UsuarioSeguridad
                                          .FirstOrDefaultAsync(us => us.IdUsuario == usuario.Id);

                if (seguridad != null)
                {
                    var hasher = new PasswordHasher<Usuario>();

                    var resultado = hasher.VerifyHashedPassword(usuario, seguridad.PasswordHash, contrasena);

                    if (resultado == PasswordVerificationResult.Success)
                    {
                        return usuario;
                    }
                }
            }
            return null;
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            return await this.context.Usuarios
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetUsuarioConDatos(int id)
        {
            return await this.context.Usuarios
                .Include(u => u.Rutinas)
                .Include(u => u.Sesions)
                    .ThenInclude(s => s.Series)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
