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
    public class SemestresController : Controller
    {
        private readonly EvaluacionContext _context;

        public SemestresController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoSemestre")]
        public IActionResult ListadoSemestre()
        {
            List<Semestre> listado = new List<Semestre>();
            var sql = from s in _context.Semestres
                      select new
                      {
                          s.IdSemestre,
                          s.NomSemestre,
                          s.AnioSemestre,
                          s.Estado
                      };
            foreach (var se in sql)
            {
                Semestre semestre = new Semestre();
                semestre.NomSemestre = se.NomSemestre;
                semestre.IdSemestre = semestre.IdSemestre;
                semestre.AnioSemestre= semestre.AnioSemestre;
                semestre.Estado = semestre.Estado;
                listado.Add(semestre);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarSemestre")]
        public IActionResult InsertarSemestre(string nom, int anio, int estado)
        {
            try
            {
                Semestre st = new Semestre();
                st.NomSemestre = nom;
                st.AnioSemestre = anio;
                st.Estado = estado;
                _context.Semestres.Add(st);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { respuesta = "Insertadp correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Error", respuesta = ex.Message });
            }
        }
        //EDITAR 
    }
}
