using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    public string RutPersona { get; set; } = null!;

    public string CodAsignatura { get; set; } = null!;

    public int Seccion { get; set; }

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual Asignatura CodAsignaturaNavigation { get; set; } = null!;
}
public class CursoDTO
{
    public int IdCurso { get; set; }
    public string RutPersona { get; set; }
    public string CodAsignatura { get; set; }
    public int Seccion { get; set; }
}
