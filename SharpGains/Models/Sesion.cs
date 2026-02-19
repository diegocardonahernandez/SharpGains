using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("SESION")]
public partial class Sesion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_rutina")]
    public int? IdRutina { get; set; }

    [Column("fecha_inicio", TypeName = "datetime")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin", TypeName = "datetime")]
    public DateTime? FechaFin { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

    [ForeignKey("IdRutina")]
    [InverseProperty("Sesions")]
    public virtual Rutina? IdRutinaNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Sesions")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdSesionNavigation")]
    public virtual ICollection<Serie> Series { get; set; } = new List<Serie>();
}
