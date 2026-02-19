using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("EJERCICIO")]
public partial class Ejercicio
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Column("grupo_muscular")]
    [StringLength(100)]
    public string GrupoMuscular { get; set; } = null!;

    [Column("equipamiento")]
    [StringLength(100)]
    public string? Equipamiento { get; set; }

    [InverseProperty("IdEjercicioNavigation")]
    public virtual ICollection<EjercicioRutina> EjercicioRutinas { get; set; } = new List<EjercicioRutina>();

    [InverseProperty("IdEjercicioNavigation")]
    public virtual ICollection<Serie> Series { get; set; } = new List<Serie>();
}
