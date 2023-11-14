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
            List<Curso> listado = new List<Curso>();
            var sql = from c in _context.Cursos
                      select new
                      {
                          c.IdCurso,
                          c.RutPersona,
                          c.CodAsignatura,
                          c.Seccion
                      };
            foreach (var cu in sql)
            {
                Curso curso = new Curso();
                curso.IdCurso = cu.IdCurso;
                curso.RutPersona = cu.RutPersona;
                curso.Seccion = cu.Seccion;
                listado.Add(curso);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarCursos")]
        public IActionResult InsertarCursos(string rut, string cod, int seccion)
        {
            try
            {
                Curso cur = new Curso();
                cur.RutPersona = rut;
                cur.CodAsignatura = cod;
                cur.Seccion = seccion;
                _context.Cursos.Add(cur);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Error", respuesta = ex.Message });
            }
        }
    }
}
