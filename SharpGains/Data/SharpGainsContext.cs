using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SharpGains.Models;

namespace SharpGains.Data;

public partial class SharpGainsContext : DbContext
{
    public SharpGainsContext()
    {
    }

    public SharpGainsContext(DbContextOptions<SharpGainsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ejercicio> Ejercicios { get; set; }

    public virtual DbSet<EjercicioRutina> EjercicioRutinas { get; set; }

    public virtual DbSet<Rutina> Rutinas { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<Sesion> Sesions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioSeguridad> UsuarioSeguridad { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ejercicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EJERCICI__3213E83F44246CC2");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<EjercicioRutina>(entity =>
        {
            entity.HasOne(d => d.IdEjercicioNavigation).WithMany(p => p.EjercicioRutinas).HasConstraintName("FK_ER_EJERCICIO");

            entity.HasOne(d => d.IdRutinaNavigation).WithMany(p => p.EjercicioRutinas).HasConstraintName("FK_ER_RUTINA");
        });

        modelBuilder.Entity<Rutina>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RUTINA__3213E83FD62E03EC");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Rutinas).HasConstraintName("FK_RUTINA_USUARIO");
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SERIE__3213E83F02B91B4C");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdEjercicioNavigation).WithMany(p => p.Series)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SERIE_EJERCICIO");

            entity.HasOne(d => d.IdSesionNavigation).WithMany(p => p.Series).HasConstraintName("FK_SERIE_SESION");
        });

        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SESION__3213E83F77A73DE7");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdRutinaNavigation).WithMany(p => p.Sesions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_SESION_RUTINA");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Sesions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SESION_USUARIO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USUARIO__3213E83FF9D3B1A1");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<UsuarioSeguridad>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIO___4E3E04AD4CA96D14");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.UsuarioSeguridad).HasConstraintName("FK_SEGURIDAD_USUARIO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
