using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Usuario
{
    public string NomUsuario { get; set; } = null!;

    public string RutPersona { get; set; } = null!;

    public string? Contraseña { get; set; }

    public virtual Persona RutPersonaNavigation { get; set; } = null!;
}
