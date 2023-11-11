using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class TipoAsignatura
{
    public int IdTipoAsignatura { get; set; }

    public string NomTipoAsignatura { get; set; } = null!;

    public int CantidadNotas { get; set; }

    public virtual ICollection<Asignatura> Asignaturas { get; set; } = new List<Asignatura>();
}
