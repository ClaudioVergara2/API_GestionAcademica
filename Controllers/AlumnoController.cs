using System;
using System.Collections.Generic;
using System.Linq;
using API_Notas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Notas.Controllers;

[ApiController]
[Route("[controller]")]
public class AlumnoController : ControllerBase
{
    private readonly EvaluacionContext _context;

    public AlumnoController(EvaluacionContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("ListadoAlumnos")]
    public IActionResult ListadoAlumnos()
    {
        var listado = (from a in _context.Alumnos
                       select new
                       {
                           ID_Alumno = a.IdAlumno,
                           Rut_Persona = a.RutPersona,
                           ID_Curso = a.IdCurso
                       }).ToList();

        return Ok(listado);
    }

    [HttpPost]
    [Route("InsertarAlumno")]
    public IActionResult InsertarAlumno([FromBody] InsertarAlumnoRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Nombres) || string.IsNullOrEmpty(request.Apellidos))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Los campos 'nombres' y 'apellidos' no pueden estar vacíos" });
            }

            if (request.IdCurso <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El campo 'idCurso' no puede estar vacío" });
            }

            Alumno alumno = new Alumno
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                IdCurso = request.IdCurso
            };

            _context.Alumnos.Add(alumno);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, new { respuesta = "Alumno insertado correctamente" });
        }
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
        }
    }

    public class InsertarAlumnoRequest
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int IdCurso { get; set; }
    }

    [HttpGet]
    [Route("ListadoCursos")]
    public IActionResult ListadoCursos()
    {
        var listado = _context.Cursos
            .Include(c => c.CodAsignaturaNavigation) 
            .Select(c => new CursoDTO
            {
                IdCurso = c.IdCurso,
                RutPersona = c.RutPersona,
                CodAsignatura = c.CodAsignatura,
                NomAsignatura = c.CodAsignaturaNavigation.NomAsignatura, 
                Seccion = c.Seccion
            })
            .ToList();

        return Ok(listado);
    }

    [HttpPost]
    [Route("EliminarAlumno")]
    public IActionResult EliminarAlumno(int idAlumno)
    {
        try
        {
            var alumno = _context.Alumnos
                .Include(a => a.Nota)
                .FirstOrDefault(a => a.IdAlumno == idAlumno);

            if (alumno == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Alumno no encontrado" });
            }

            if (alumno.Nota.Any())
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "No se puede eliminar el alumno, tiene notas asociadas" });
            }

            _context.Alumnos.Remove(alumno);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, new { respuesta = "Alumno eliminado correctamente" });
        }
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
        }
    }
} 
