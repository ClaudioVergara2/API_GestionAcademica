using System;
using System.Linq;
using API_Notas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API_Notas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotasController : ControllerBase
    {
        private readonly EvaluacionContext _context;

        public NotasController(EvaluacionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListadoNotas")]
        public IActionResult ListadoNotas()
        {
            var listado = _context.Notas.Select(n => new
            {
                ID_Nota = n.IdNota,
                Nota = n.Nota1,
                ID_Alumno = n.IdAlumno,
                Numero_Nota = n.NumNota
            }).ToList();

            return Ok(listado);
        }

        [HttpPost]
        [Route("InsertarNota")]
        public IActionResult InsertarNota(decimal nota, int idAlumno, int numNota)
        {
            try
            {
                var tipoAsignatura = _context.Alumnos
                    .Include(a => a.IdCursoNavigation)
                    .ThenInclude(c => c.CodAsignaturaNavigation)
                    .ThenInclude(a => a.IdTipoAsignaturaNavigation)
                    .Where(a => a.IdAlumno == idAlumno)
                    .Select(a => a.IdCursoNavigation.CodAsignaturaNavigation.IdTipoAsignaturaNavigation.NomTipoAsignatura)
                    .FirstOrDefault();

                var cantidadNotasPermitidas = _context.TipoAsignaturas
                    .Where(t => t.NomTipoAsignatura == tipoAsignatura)
                    .Select(t => t.CantidadNotas)
                    .FirstOrDefault();

                if (numNota > cantidadNotasPermitidas)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Número de nota excede la cantidad permitida" });
                }

                if (numNota == 1)
                {
                    Nota nuevaNota = new Nota
                    {
                        Nota1 = nota,
                        IdAlumno = idAlumno,
                        NumNota = numNota
                    };

                    _context.Notas.Add(nuevaNota);
                    _context.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { respuesta = "Nota insertada correctamente" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Solo se permite insertar la primera nota" });
                }
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
            }
        }
        [HttpPost]
        [Route("EditarNota")]
        public IActionResult EditarNota(int idNota, decimal nuevaNota)
        {
            try
            {
                var notaExistente = _context.Notas.FirstOrDefault(n => n.IdNota == idNota);

                if (notaExistente == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Nota no encontrada" });
                }

                if (nuevaNota == 0.0m)
                {
                    if (notaExistente.Nota1 != null && notaExistente.Nota1 != 0.0m)
                    {
                        notaExistente.Nota1 = 1.0m;
                    }
                }
                else
                {
                    notaExistente.Nota1 = nuevaNota;
                }

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Nota actualizada correctamente" });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
            }
        }
    }
}


