using System;
using System.Collections.Generic;
using System.Linq;
using API_Notas.Models;
using Microsoft.AspNetCore.Mvc;

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
            // Considera registrar el mensaje en un log para análisis detallado
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
        }
    }
    //falta eliminar alumno si este no tene notas.
}
