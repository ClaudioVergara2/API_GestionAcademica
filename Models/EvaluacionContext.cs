using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Notas.Models;

public partial class EvaluacionContext : DbContext
{
    public EvaluacionContext()
    {
    }

    public EvaluacionContext(DbContextOptions<EvaluacionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Asignatura> Asignaturas { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Semestre> Semestres { get; set; }

    public virtual DbSet<TipoAsignatura> TipoAsignaturas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ClaudioPC\\SQLEXPRESS; Database=EVALUACION;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno);

            entity.ToTable("ALUMNOS");

            entity.Property(e => e.IdAlumno).HasColumnName("ID_ALUMNO");
            entity.Property(e => e.IdCurso).HasColumnName("ID_CURSO");
            entity.Property(e => e.RutPersona)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("RUT_PERSONA");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdCurso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ALUMNOS_CURSO");

            entity.HasOne(d => d.RutPersonaNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.RutPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ALUMNOS_PERSONA");
        });

        modelBuilder.Entity<Asignatura>(entity =>
        {
            entity.HasKey(e => e.CodAsignatura);

            entity.ToTable("ASIGNATURA");

            entity.Property(e => e.CodAsignatura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("COD_ASIGNATURA");
            entity.Property(e => e.IdSemestre).HasColumnName("ID_SEMESTRE");
            entity.Property(e => e.IdTipoAsignatura).HasColumnName("ID_TIPO_ASIGNATURA");
            entity.Property(e => e.NomAsignatura)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_ASDIGNATURA");

            entity.HasOne(d => d.IdSemestreNavigation)
                  .WithMany(p => p.Asignaturas)
                  .HasForeignKey(d => d.IdSemestre)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_ASIGNATURA_SEMESTRE");

            entity.HasOne(d => d.IdTipoAsignaturaNavigation)
                  .WithMany(p => p.Asignaturas)
                  .HasForeignKey(d => d.IdTipoAsignatura)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_ASIGNATURA_TIPO_ASIGNATURA");
        });



        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso);

            entity.ToTable("CURSO");

            entity.Property(e => e.IdCurso).HasColumnName("ID_CURSO");
            entity.Property(e => e.CodAsignatura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("COD_ASIGNATURA");
            entity.Property(e => e.RutPersona)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("RUT_PERSONA");
            entity.Property(e => e.Seccion).HasColumnName("SECCION");

            entity.HasOne(d => d.CodAsignaturaNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.CodAsignatura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CURSO_ASIGNATURA");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.IdNota);

            entity.ToTable("NOTAS");

            entity.Property(e => e.IdNota).HasColumnName("ID_NOTA");
            entity.Property(e => e.IdAlumno).HasColumnName("ID_ALUMNO");
            entity.Property(e => e.Nota1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("NOTA");
            entity.Property(e => e.NumNota).HasColumnName("NUM_NOTA");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NOTAS_ALUMNOS");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil);

            entity.ToTable("PERFIL");

            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.NomPerfil)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_PERFIL");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.RutPersona).HasName("PK_PERSONA_1");

            entity.ToTable("PERSONA");

            entity.Property(e => e.RutPersona)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("RUT_PERSONA");
            entity.Property(e => e.ApePersona)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APE_PERSONA");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("FECHA_NACIMIENTO");
            entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            entity.Property(e => e.NomPersona)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_PERSONA");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdPerfil)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PERSONA_PERFIL");
        });

        modelBuilder.Entity<Semestre>(entity =>
        {
            entity.HasKey(e => e.IdSemestre);

            entity.ToTable("SEMESTRE");

            entity.Property(e => e.IdSemestre).HasColumnName("ID_SEMESTRE");
            entity.Property(e => e.AnioSemestre).HasColumnName("ANIO_SEMESTRE");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.NomSemestre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOM_SEMESTRE");
        });

        modelBuilder.Entity<TipoAsignatura>(entity =>
        {
            entity.HasKey(e => e.IdTipoAsignatura);

            entity.ToTable("TIPO_ASIGNATURA");

            entity.Property(e => e.IdTipoAsignatura).HasColumnName("ID_TIPO_ASIGNATURA");
            entity.Property(e => e.CantidadNotas).HasColumnName("CANTIDAD_NOTAS");
            entity.Property(e => e.NomTipoAsignatura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NOM_TIPO_ASIGNATURA");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NomUsuario);

            entity.ToTable("USUARIO");

            entity.Property(e => e.NomUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOM_USUARIO");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTRASEÑA");
            entity.Property(e => e.RutPersona)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("RUT_PERSONA");

            entity.HasOne(d => d.RutPersonaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RutPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USUARIO_PERSONA1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
