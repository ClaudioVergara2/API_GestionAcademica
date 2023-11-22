using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Notas.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace API_Notas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonasController : ControllerBase
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
            List<PersonaDto> listado = new List<PersonaDto>();
            var sql = from P in _context.Personas
                      select new PersonaDto
                      {
                          RutPersona = P.RutPersona,
                          NomPersona = P.NomPersona,
                          ApePersona = P.ApePersona,
                          FechaNacimiento = P.FechaNacimiento,
                          NomPerfil = P.IdPerfilNavigation.NomPerfil
                      };

            foreach (var row in sql)
            {
                listado.Add(row);
            }

            return Ok(JsonConvert.SerializeObject(listado));
        }

        public class PersonaDto
        {
            public string RutPersona { get; set; }
            public string NomPersona { get; set; }
            public string ApePersona { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string NomPerfil { get; set; }
        }

        [HttpPost]
        [Route("InsertarPersona")]
        public IActionResult InsertarPersona([FromBody] InsertarPersonaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Rut) || string.IsNullOrWhiteSpace(request.Nom) || string.IsNullOrWhiteSpace(request.Ape))
                {
                    return BadRequest(new { mensaje = "Error", respuesta = "Todos los campos son obligatorios." });
                }

                if (!_context.Perfils.Any(p => p.IdPerfil == request.IdPerfil))
                {
                    return BadRequest(new { mensaje = "Error", respuesta = "El perfil especificado no existe." });
                }

                if (request.IdPerfil != 1 && request.IdPerfil != 2 && request.IdPerfil != 3)
                {
                    return BadRequest(new { mensaje = "Error", respuesta = "El idPerfil debe ser 1 (OPERADOR), 2 (PROFESOR) o 3 (ESTUDIANTE)." });
                }

                Persona persona = new Persona
                {
                    RutPersona = request.Rut,
                    NomPersona = request.Nom,
                    ApePersona = request.Ape,
                    IdPerfil = request.IdPerfil,
                    FechaNacimiento = request.FechaNacimiento
                };

                _context.Personas.Add(persona);
                _context.SaveChanges();

                return Ok(new { respuesta = "Insertado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarPersona")]
        public IActionResult EditarPersona([FromBody] EditarPersonaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Rut) || string.IsNullOrEmpty(request.NuevoNombre) || string.IsNullOrEmpty(request.NuevoApellido))
                {
                    return BadRequest(new { mensaje = "Los campos no pueden estar vacíos" });
                }

                var persona = _context.Personas.FirstOrDefault(p => p.RutPersona == request.Rut);

                if (persona == null)
                {
                    return NotFound(new { mensaje = "Persona no encontrada" });
                }

                persona.NomPersona = request.NuevoNombre;
                persona.ApePersona = request.NuevoApellido;
                if (DateTime.TryParseExact(request.NuevaFechaNac, new[] { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaNacimiento))
                {
                    persona.FechaNacimiento = fechaNacimiento;
                }
                else
                {
                    return BadRequest(new { mensaje = "Error", respuesta = "Formato de fecha no válido." });
                }

                _context.Personas.Update(persona);
                _context.SaveChanges();

                return Ok(new { mensaje = "Persona actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar la persona", error = ex.Message });
            }
        }

        public class InsertarPersonaRequest
        {
            public string Rut { get; set; }
            public string Nom { get; set; }
            public string Ape { get; set; }
            public int IdPerfil { get; set; }
            public DateTime FechaNacimiento { get; set; }
        }

        public class EditarPersonaRequest
        {
            public string Rut { get; set; }
            public string NuevoNombre { get; set; }
            public string NuevoApellido { get; set; }
            public string NuevaFechaNac { get; set; }
        }

        [HttpGet]
        [Route("ObtenerPersona")]
        public IActionResult ObtenerPersona(string rut)
        {
            var persona = _context.Personas
                .Where(p => p.RutPersona == rut)
                .Select(p => new PersonaDto
                {
                    RutPersona = p.RutPersona,
                    NomPersona = p.NomPersona,
                    ApePersona = p.ApePersona,
                    FechaNacimiento = p.FechaNacimiento,
                    NomPerfil = p.IdPerfilNavigation.NomPerfil
                })
                .FirstOrDefault();

            if (persona == null)
            {
                return NotFound(new { mensaje = "Persona no encontrada" });
            }

            return Ok(JsonConvert.SerializeObject(persona));
        }

        [HttpDelete]
        [Route("EliminarPersona")]
        public IActionResult EliminarPersona(string rut)
        {
            try
            {
                var persona = _context.Personas.FirstOrDefault(p => p.RutPersona == rut);

                if (persona == null)
                {
                    return NotFound(new { mensaje = "Persona no encontrada" });
                }

                _context.Personas.Remove(persona);
                _context.SaveChanges();

                return Ok(new { mensaje = "Persona eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al eliminar la persona", error = ex.Message });
            }
        }
    }
}
