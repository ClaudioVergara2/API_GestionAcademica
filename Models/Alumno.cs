using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Alumno
{
    public int IdAlumno { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public bool Habilitado { get; set; } = true;
    public string RutPersona { get; set; } = null!;

    public int IdCurso { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();

    public virtual Persona RutPersonaNavigation { get; set; } = null!;
}
