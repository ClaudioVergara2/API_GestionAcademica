using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Asignatura
{
    public string CodAsignatura { get; set; } = null!;

    public string NomAsdignatura { get; set; } = null!;

    public int IdSemestre { get; set; }

    public int IdTipoAsignatura { get; set; }

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public virtual Semestre IdSemestreNavigation { get; set; } = null!;

    public virtual TipoAsignatura IdTipoAsignaturaNavigation { get; set; } = null!;
}
