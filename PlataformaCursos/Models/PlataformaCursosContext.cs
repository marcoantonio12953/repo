using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlataformaCursos.Models;

public partial class PlataformaCursosContext : DbContext
{
    public PlataformaCursosContext()
    {
    }

    public PlataformaCursosContext(DbContextOptions<PlataformaCursosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Inscripcion> Inscripciones { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<TareasPublicacion> TareasPublicaciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D9IF75B\\SQL2019;Database=PlataformaCursos;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.CursoId).HasName("PK__Cursos__7E023A374F85A062");

            entity.Property(e => e.CursoId).HasColumnName("CursoID");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.ProfesorId).HasColumnName("ProfesorID");
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Profesor).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.ProfesorId)
                .HasConstraintName("FK__Cursos__Profesor__164452B1");
        });

        modelBuilder.Entity<Inscripcion>(entity =>
        {
            entity.HasKey(e => e.InscripcionId).HasName("PK__Inscripc__16831699E016C2B4");

            entity.Property(e => e.InscripcionId).HasColumnName("InscripcionID");
            entity.Property(e => e.AlumnoId).HasColumnName("AlumnoID");
            entity.Property(e => e.CursoId).HasColumnName("CursoID");
            entity.Property(e => e.NumeroTarjeta).HasMaxLength(20);

            entity.HasOne(d => d.Alumno).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.AlumnoId)
                .HasConstraintName("FK__Inscripci__Alumn__1A14E395");

            entity.HasOne(d => d.Curso).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("FK__Inscripci__Curso__1920BF5C");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302D12498FAA5");

            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.NombreRol).HasMaxLength(50);
        });

        modelBuilder.Entity<TareasPublicacion>(entity =>
        {
            entity.HasKey(e => e.PublicacionId).HasName("PK__Tareas_P__10DF15AA3B137201");

            entity.ToTable("Tareas_Publicaciones");

            entity.Property(e => e.PublicacionId).HasColumnName("PublicacionID");
            entity.Property(e => e.CursoId).HasColumnName("CursoID");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Curso).WithMany(p => p.TareasPublicaciones)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("FK__Tareas_Pu__Curso__1CF15040");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE79857750783");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0492EF856").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.RolId).HasColumnName("RolID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__Usuarios__RolID__1367E606");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
