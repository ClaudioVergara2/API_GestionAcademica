using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Semestre
{
    public int IdSemestre { get; set; }

    public string NomSemestre { get; set; } = null!;

    public int AnioSemestre { get; set; }

    public int Estado { get; set; }

    public virtual ICollection<Asignatura> Asignaturas { get; set; } = new List<Asignatura>();
}
