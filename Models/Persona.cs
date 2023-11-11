using System;
using System.Collections.Generic;

namespace API_Notas.Models;

public partial class Persona
{
    public string RutPersona { get; set; } = null!;

    public string NomPersona { get; set; } = null!;

    public string ApePersona { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public int IdPerfil { get; set; }

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual Perfil IdPerfilNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
