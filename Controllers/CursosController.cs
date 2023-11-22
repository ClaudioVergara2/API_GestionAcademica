using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Notas.Models;
using Newtonsoft.Json;

namespace API_Notas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursosController : Controller
    {
        private readonly EvaluacionContext _context;

        public CursosController(EvaluacionContext context)
        {
            _context = context;
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

            return Ok(JsonConvert.SerializeObject(listado));
        }

        [HttpPost]
        [Route("InsertarCursos")]
        public IActionResult InsertarCursos([FromBody] CursoRequest request)
        {
            try
            {
                if (_context.Cursos.Any(c => c.RutPersona == request.RutPersona && c.CodAsignatura == request.CodAsignatura && c.Seccion == request.Seccion))
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Error", respuesta = "Ya existe una sección para el mismo profesor y curso." });
                }

                if (_context.Cursos.Any(c => c.CodAsignatura == request.CodAsignatura && c.Seccion == request.Seccion))
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Error", respuesta = "La sección ya está ocupada por otro profesor." });
                }

                Curso cur = new Curso();
                cur.RutPersona = request.RutPersona;
                cur.CodAsignatura = request.CodAsignatura;
                cur.Seccion = request.Seccion;
                _context.Cursos.Add(cur);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Error", respuesta = ex.Message });
            }
        }
        public class CursoRequest
        {
            public string RutPersona { get; set; }
            public string CodAsignatura { get; set; }
            public int Seccion { get; set; }
        }

        [HttpGet]
        [Route("ListadoAsignaturas")]
        public IActionResult ListadoAsignaturas()
        {
            List<Asignatura> listado = _context.Asignaturas
                .Select(a => new Asignatura { CodAsignatura = a.CodAsignatura, NomAsignatura = a.NomAsignatura })
                .ToList();
            return Ok(JsonConvert.SerializeObject(listado));
        }

        [HttpGet]
        [Route("ListadoProfesores")]
        public IActionResult ListadoProfesores()
        {
            List<string> listado = _context.Personas
                .Where(p => p.IdPerfil == 2) 
                .Select(p => p.RutPersona)
                .ToList();
            return Ok(JsonConvert.SerializeObject(listado));
        }
    }
}
