using System;
using System.Linq;
using API_Notas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API_Notas.Controllers;

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
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = innerMessage });
        }
    }
}
