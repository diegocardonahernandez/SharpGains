using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("USUARIO_SEGURIDAD")]
public partial class UsuarioSeguridad
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("UsuarioSeguridad")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
