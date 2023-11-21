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
    public IActionResult InsertarAlumno(string rutPersona, int idCurso)
    {
        try
        {
            if (string.IsNullOrEmpty(rutPersona))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El campo 'rutPersona' no puede estar vacío" });
            }

            if (idCurso <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El campo 'idCurso' no puede estar vacío" });
            }

            Alumno alumno = new Alumno
            {
                RutPersona = rutPersona,
                IdCurso = idCurso
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
