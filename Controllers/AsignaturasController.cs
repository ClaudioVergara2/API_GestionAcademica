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
                          c.NomAsdignatura,
                          c.IdSemestre,
                          c.IdTipoAsignatura
                      };
            foreach (var asig in sql)
            {
                Asignatura asignatura = new Asignatura();
                asignatura.CodAsignatura = asig.CodAsignatura;
                asignatura.NomAsdignatura = asig.NomAsdignatura;
                asignatura.IdSemestre = asig.IdSemestre;
                asignatura.IdTipoAsignatura = asig.IdTipoAsignatura;
                listado.Add(asignatura);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarAsignatura")]
        public IActionResult InsertarAsignatura(string cod, string nom)
        {
            try
            {
                Asignatura asg = new Asignatura();
                asg.CodAsignatura = cod;
                asg.NomAsdignatura = nom;
                _context.Asignaturas.Add(asg);
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
