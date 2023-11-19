using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Notas.Models;

namespace API_Notas.Controllers
{
    public class PersonasController : Controller
    {
        private readonly EvaluacionContext _context;

        public PersonasController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoPersona")]
        public IActionResult ListadoPersona()
        {
            List<Persona> listado = new List<Persona>();
            var sql = from P in _context.Personas
                      select new
                      {
                          P.RutPersona,
                          P.NomPersona,
                          P.ApePersona,
                          P.FechaNacimiento,
                          P.IdPerfil
                      };
            foreach (var row in sql)
            {
                Persona persona = new Persona();
                persona.RutPersona = row.RutPersona;
                persona.NomPersona = row.NomPersona;
                persona.ApePersona = row.ApePersona;
                persona.FechaNacimiento = row.FechaNacimiento;
                persona.IdPerfil = row.IdPerfil;
                listado.Add(persona);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarPersona")]
        public IActionResult InsertarPersona(string rut, string nom, string ape, int idPerfil, DateTime fecha_nac)
        {
            try
            {
                if (!_context.Perfils.Any(p => p.IdPerfil == idPerfil))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error", respuesta = "El perfil especificado no existe." });
                }
                Persona persona = new Persona
                {
                    RutPersona = rut,
                    NomPersona = nom,
                    ApePersona = ape,
                    IdPerfil = idPerfil,
                    FechaNacimiento = fecha_nac
                };
                _context.Personas.Add(persona);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }
        // FALTA EDITAR: Solo de edita nombre, apellido y fecha de nacimiento.
    }
}
