using System;
using System.Collections.Generic;
using DirectorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectorAPI.Data;

public partial class Sistem21PrimariaContext : DbContext
{
    public Sistem21PrimariaContext()
    {
    }

    public Sistem21PrimariaContext(DbContextOptions<Sistem21PrimariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<AlumnoTutor> AlumnoTutor { get; set; }

    public virtual DbSet<Asignatura> Asignatura { get; set; }

    public virtual DbSet<Calificacion> Calificacion { get; set; }

    public virtual DbSet<Director> Director { get; set; }

    public virtual DbSet<Docente> Docente { get; set; }

    public virtual DbSet<DocenteAlumno> DocenteAlumno { get; set; }

    public virtual DbSet<DocenteAsignatura> DocenteAsignatura { get; set; }

    public virtual DbSet<DocenteGrupo> DocenteGrupo { get; set; }

    public virtual DbSet<Grupo> Grupo { get; set; }

    public virtual DbSet<Periodo> Periodo { get; set; }

    public virtual DbSet<Tutor> Tutor { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.IdGrupo, "fkAlumnoGrupo_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.IdGrupo)
                .HasColumnType("int(11)")
                .HasColumnName("idGrupo");
            entity.Property(e => e.Matricula).HasMaxLength(10);
            entity.Property(e => e.Nombre).HasMaxLength(200);

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Alumno)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("fkAlumnoGrupo");
        });

        modelBuilder.Entity<AlumnoTutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno_tutor");

            entity.HasIndex(e => e.IdAlumno, "fkAlumno_idx");

            entity.HasIndex(e => e.IdTutor, "fkTutor_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdAlumno).HasColumnType("int(11)");
            entity.Property(e => e.IdTutor).HasColumnType("int(11)");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.AlumnoTutor)
                .HasForeignKey(d => d.IdAlumno)
                .HasConstraintName("fkAlumno");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.AlumnoTutor)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkTutor");
        });

        modelBuilder.Entity<Asignatura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asignatura");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.TipoAsignatura).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("calificacion");

            entity.HasIndex(e => e.IdAlumno, "fkCal_Alumno_idx");

            entity.HasIndex(e => e.IdAsignatura, "fkCal_Asignatura_idx");

            entity.HasIndex(e => e.IdDocente, "fkCal_Docente_idx");

            entity.HasIndex(e => e.IdPeriodo, "fkCal_Periodo_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Calificacion1).HasColumnName("Calificacion");
            entity.Property(e => e.IdAlumno).HasColumnType("int(11)");
            entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");
            entity.Property(e => e.IdDocente).HasColumnType("int(11)");
            entity.Property(e => e.IdPeriodo).HasColumnType("int(11)");
            entity.Property(e => e.Unidad).HasColumnType("int(11)");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.IdAlumno)
                .HasConstraintName("fkCal_Alumno");

            entity.HasOne(d => d.IdAsignaturaNavigation).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.IdAsignatura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkCal_Asignatura");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkCal_Docente");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.Calificacion)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkCal_Periodo");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("director");

            entity.HasIndex(e => e.Idusuario, "fkDocente_Usaurio_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Idusuario)
                .HasColumnType("int(11)")
                .HasColumnName("idusuario");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(10);

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Director)
                .HasForeignKey(d => d.Idusuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkdirector_Usaurio");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docente");

            entity.HasIndex(e => e.IdUsuario, "fkDocente_usuario_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(45);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(45);
            entity.Property(e => e.Correo).HasMaxLength(60);
            entity.Property(e => e.Edad).HasColumnType("int(11)");
            entity.Property(e => e.IdUsuario).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TipoDocente).HasColumnType("int(11)");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Docente)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkDocente_usuario");
        });

        modelBuilder.Entity<DocenteAlumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docente_alumno");

            entity.HasIndex(e => e.IdAlumno, "fkAlumno_Docente_idx");

            entity.HasIndex(e => e.IdDocente, "fkDocente_idx");

            entity.HasIndex(e => e.IdPeriodo, "fkPeriodoGrupo_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdAlumno)
                .HasColumnType("int(11)")
                .HasColumnName("idAlumno");
            entity.Property(e => e.IdDocente)
                .HasColumnType("int(11)")
                .HasColumnName("idDocente");
            entity.Property(e => e.IdPeriodo)
                .HasColumnType("int(11)")
                .HasColumnName("idPeriodo");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.DocenteAlumno)
                .HasForeignKey(d => d.IdAlumno)
                .HasConstraintName("fkAlumno_Docente");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.DocenteAlumno)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkDocente_Grupo_Alumno");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.DocenteAlumno)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkPeriodoGrupo");
        });

        modelBuilder.Entity<DocenteAsignatura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docente_asignatura");

            entity.HasIndex(e => e.IdAsignatura, "fkAsignatura_idx");

            entity.HasIndex(e => e.IdDocente, "fkDocente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdAsignatura).HasColumnType("int(11)");
            entity.Property(e => e.IdDocente).HasColumnType("int(11)");

            entity.HasOne(d => d.IdAsignaturaNavigation).WithMany(p => p.DocenteAsignatura)
                .HasForeignKey(d => d.IdAsignatura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkAsignatura");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.DocenteAsignatura)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkDocente");
        });

        modelBuilder.Entity<DocenteGrupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docente_grupo");

            entity.HasIndex(e => e.IdDocente, "fkDocente_Grupo_");

            entity.HasIndex(e => e.IdPeriodo, "fkGrupoPeriodo_idx");

            entity.HasIndex(e => e.IdGrupo, "fkGrupo_Docente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdDocente)
                .HasColumnType("int(11)")
                .HasColumnName("idDocente");
            entity.Property(e => e.IdGrupo)
                .HasColumnType("int(11)")
                .HasColumnName("idGrupo");
            entity.Property(e => e.IdPeriodo)
                .HasColumnType("int(11)")
                .HasColumnName("idPeriodo");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.DocenteGrupo)
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkDocente_Grupo_");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.DocenteGrupo)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkGrupo_Docente_");

            entity.HasOne(d => d.IdPeriodoNavigation).WithMany(p => p.DocenteGrupo)
                .HasForeignKey(d => d.IdPeriodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkGrupoPeriodo");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("grupo");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Grado).HasMaxLength(2);
            entity.Property(e => e.Seccion).HasMaxLength(2);
        });

        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("periodo");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Año).HasColumnType("year(4)");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tutor");

            entity.HasIndex(e => e.Idusuario, "fkPadre_Usuario_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Idusuario)
                .HasColumnType("int(11)")
                .HasColumnName("idusuario");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Telefono).HasMaxLength(10);

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Tutor)
                .HasForeignKey(d => d.Idusuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkPadre_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contraseña).HasColumnType("tinytext");
            entity.Property(e => e.Rol).HasColumnType("int(11)");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(45)
                .HasColumnName("Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
