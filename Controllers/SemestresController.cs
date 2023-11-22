using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Notas.Models;
using Newtonsoft.Json;

namespace API_Notas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemestresController : ControllerBase
    {
        private readonly EvaluacionContext _context;

        public SemestresController(EvaluacionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListadoSemestre")]
        public IActionResult ListadoSemestre()
        {
            try
            {
                var semestres = _context.Semestres
                    .Select(s => new
                    {
                        s.IdSemestre,
                        s.NomSemestre,
                        s.AnioSemestre,
                        Estado = s.Estado == 1 ? "Activo" : "Inactivo"
                    })
                    .ToList();

                return Ok(JsonConvert.SerializeObject(semestres));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        [HttpPost]
        [Route("InsertarSemestre")]
        public IActionResult InsertarSemestre([FromBody] SemestreRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NomSemestre))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El nombre del semestre no puede estar vacío." });
                }

                if (request.AnioSemestre <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El año del semestre debe ser mayor que 0." });
                }

                if (request.Estado != 0 && request.Estado != 1)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El estado del semestre debe ser 0 (INACTIVO) o 1 (ACTIVO)." });
                }

                var nuevoSemestre = new Semestre
                {
                    NomSemestre = request.NomSemestre,
                    AnioSemestre = request.AnioSemestre,
                    Estado = request.Estado
                };

                _context.Semestres.Add(nuevoSemestre);
                _context.SaveChanges();

                string estadoTexto = nuevoSemestre.Estado == 1 ? "ACTIVO" : "INACTIVO";

                return StatusCode(StatusCodes.Status200OK, new
                {
                    respuesta = "Insertado correctamente",
                    semestre = new
                    {
                        Nombre = nuevoSemestre.NomSemestre,
                        Año = nuevoSemestre.AnioSemestre,
                        Estado = estadoTexto
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        [HttpPost]
        [Route("EditarEstadoSemestre")]
        public IActionResult EditarEstadoSemestre([FromBody] EditarEstadoSemestreRequest request)
        {
            try
            {
                // Validation
                if (request.IdSemestre <= 0)
                {
                    return BadRequest(new { mensaje = "El ID del semestre no es válido" });
                }

                if (request.NuevoEstado != 0 && request.NuevoEstado != 1)
                {
                    return BadRequest(new { mensaje = "El nuevo estado no es válido" });
                }

                // Updating state
                var semestre = _context.Semestres.FirstOrDefault(s => s.IdSemestre == request.IdSemestre);

                if (semestre == null)
                {
                    return NotFound(new { mensaje = "Semestre no encontrado" });
                }

                semestre.Estado = request.NuevoEstado;
                _context.Semestres.Update(semestre);
                _context.SaveChanges();

                return Ok(new { respuesta = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        public class SemestreRequest
        {
            public string NomSemestre { get; set; }
            public int AnioSemestre { get; set; }
            public int Estado { get; set; }
        }

        public class EditarEstadoSemestreRequest
        {
            public int IdSemestre { get; set; }
            public int NuevoEstado { get; set; }
        }
    }
}
