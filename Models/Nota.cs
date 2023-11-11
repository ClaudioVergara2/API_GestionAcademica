using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    public decimal Nota1 { get; set; }

    public int IdAlumno { get; set; }

    public int NumNota { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;
}
