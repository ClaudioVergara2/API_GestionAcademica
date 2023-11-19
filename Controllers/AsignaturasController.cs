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
    public class AsignaturasController : Controller
    {
        private readonly EvaluacionContext _context;

        public AsignaturasController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoAsignatura")]
        public IActionResult ListadoAsignatura()
        {
            List<Asignatura> listado = new List<Asignatura>();
            var sql = from c in _context.Asignaturas
                      select new
                      {
                          c.CodAsignatura,
                          c.NomAsignatura,
                          c.IdSemestre,
                          c.IdTipoAsignatura
                      };
            foreach (var asig in sql)
            {
                Asignatura asignatura = new Asignatura();
                asignatura.CodAsignatura = asig.CodAsignatura;
                asignatura.NomAsignatura = asig.NomAsignatura;
                asignatura.IdSemestre = asig.IdSemestre;
                asignatura.IdTipoAsignatura = asig.IdTipoAsignatura;
                listado.Add(asignatura);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarAsignatura")]
        public IActionResult InsertarAsignatura(string cod, string nom, int idSemestre, int idTipoAsignatura)
        {
            try
            {
                Asignatura asg = new Asignatura();
                asg.CodAsignatura = cod;
                asg.NomAsignatura = nom; // Asegúrate también de que esta propiedad esté escrita correctamente
                asg.IdSemestre = idSemestre; // Asume que Asignatura tiene esta propiedad para la clave foránea
                asg.IdTipoAsignatura = idTipoAsignatura; // Asume que Asignatura tiene esta propiedad para la clave foránea

                // Verifica si los IDs de semestre y tipo asignatura existen
                var semestreExists = _context.Semestres.Any(s => s.IdSemestre == idSemestre);
                var tipoAsignaturaExists = _context.TipoAsignaturas.Any(t => t.IdTipoAsignatura == idTipoAsignatura);
                if (!semestreExists || !tipoAsignaturaExists)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error", detalle = "Semestre o tipo de asignatura no existen" });
                }

                _context.Asignaturas.Add(asg);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertado correctamente" });
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                // Considera registrar esta información en un log
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", detalle = ex.Message, detalleInterno = innerMessage });
            }
        }

    }
}
