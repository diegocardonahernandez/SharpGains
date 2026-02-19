using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("RUTINA")]
public partial class Rutina
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdRutinaNavigation")]
    public virtual ICollection<EjercicioRutina> EjercicioRutinas { get; set; } = new List<EjercicioRutina>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Rutinas")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdRutinaNavigation")]
    public virtual ICollection<Sesion> Sesions { get; set; } = new List<Sesion>();
}
