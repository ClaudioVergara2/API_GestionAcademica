using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Perfil
{
    public int IdPerfil { get; set; }

    public string NomPerfil { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
