using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[PrimaryKey("IdEjercicio", "IdRutina")]
[Table("EJERCICIO_RUTINA")]
public partial class EjercicioRutina
{
    [Key]
    [Column("id_ejercicio")]
    public int IdEjercicio { get; set; }

    [Key]
    [Column("id_rutina")]
    public int IdRutina { get; set; }

    [Column("series_objetivo")]
    public int SeriesObjetivo { get; set; }

    [Column("repeticiones_objetivo")]
    public int RepeticionesObjetivo { get; set; }

    [Column("orden")]
    public int Orden { get; set; }

    [ForeignKey("IdEjercicio")]
    [InverseProperty("EjercicioRutinas")]
    public virtual Ejercicio IdEjercicioNavigation { get; set; } = null!;

    [ForeignKey("IdRutina")]
    [InverseProperty("EjercicioRutinas")]
    public virtual Rutina IdRutinaNavigation { get; set; } = null!;
}
