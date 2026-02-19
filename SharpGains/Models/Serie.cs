using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharpGains.Models;

[Table("SERIE")]
public partial class Serie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_sesion")]
    public int IdSesion { get; set; }

    [Column("id_ejercicio")]
    public int IdEjercicio { get; set; }

    [Column("numero_serie")]
    public int NumeroSerie { get; set; }

    [Column("peso", TypeName = "decimal(6, 2)")]
    public decimal Peso { get; set; }

    [Column("repeticiones")]
    public int Repeticiones { get; set; }

    [Column("rpe")]
    public int? Rpe { get; set; }

    [ForeignKey("IdEjercicio")]
    [InverseProperty("Series")]
    public virtual Ejercicio IdEjercicioNavigation { get; set; } = null!;

    [ForeignKey("IdSesion")]
    [InverseProperty("Series")]
    public virtual Sesion IdSesionNavigation { get; set; } = null!;
}
