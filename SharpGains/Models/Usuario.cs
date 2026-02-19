using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("USUARIO")]
[Index("Correo", Name = "UQ__USUARIO__2A586E0B99290F91", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("apellidos")]
    [StringLength(150)]
    public string? Apellidos { get; set; }

    [Column("correo")]
    [StringLength(255)]
    public string Correo { get; set; } = null!;

    [Column("contrasena")]
    public string? Contrasena { get; set; }

    [Column("altura", TypeName = "decimal(5, 2)")]
    public decimal? Altura { get; set; }

    [Column("peso", TypeName = "decimal(5, 2)")]
    public decimal? Peso { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Sesion> Sesions { get; set; } = new List<Sesion>();
}
