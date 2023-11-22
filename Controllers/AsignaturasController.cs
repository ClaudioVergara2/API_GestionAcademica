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
            return Ok(JsonConvert.SerializeObject(listado));
        }

        [HttpPost]
        [Route("InsertarAsignatura")]
        public IActionResult InsertarAsignatura([FromBody] AsignaturaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CodAsignatura) || string.IsNullOrEmpty(request.NomAsignatura))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error", detalle = "Los datos ingresados no pueden estar vacíos." });
                }

                var existingAsignatura = _context.Asignaturas.FirstOrDefault(a => a.CodAsignatura == request.CodAsignatura);
                if (existingAsignatura != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error", detalle = "El código de asignatura ya está en uso." });
                }

                Asignatura asg = new Asignatura();
                asg.CodAsignatura = request.CodAsignatura;
                asg.NomAsignatura = request.NomAsignatura;
                asg.IdSemestre = request.IdSemestre;
                asg.IdTipoAsignatura = request.IdTipoAsignatura;

                var semestreExists = _context.Semestres.Any(s => s.IdSemestre == request.IdSemestre);
                var tipoAsignaturaExists = _context.TipoAsignaturas.Any(t => t.IdTipoAsignatura == request.IdTipoAsignatura);
                if (!semestreExists || !tipoAsignaturaExists)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error", detalle = "Semestre o tipo de asignatura no existen." });
                }

                _context.Asignaturas.Add(asg);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertado correctamente" });
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", detalle = ex.Message, detalleInterno = innerMessage });
            }
        }
        public class AsignaturaRequest
        {
            public string CodAsignatura { get; set; }
            public string NomAsignatura { get; set; }
            public int IdSemestre { get; set; }
            public int IdTipoAsignatura { get; set; }
        }
    }
}
