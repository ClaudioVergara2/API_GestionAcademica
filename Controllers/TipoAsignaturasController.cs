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
    public class TipoAsignaturasController : Controller
    {
        private readonly EvaluacionContext _context;

        public TipoAsignaturasController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoTipoAsignatura")]
        public IActionResult ListadoTipoAsignatura()
        {
            List<TipoAsignatura> listado = new List<TipoAsignatura>();
            var sql = from ta in _context.TipoAsignaturas
                      select new
                      {
                          ta.IdTipoAsignatura,
                          ta.NomTipoAsignatura,
                          ta.CantidadNotas
                      };
            foreach(var tipoA in sql)
            {
                TipoAsignatura tipoAsignatura = new TipoAsignatura();
                tipoAsignatura.IdTipoAsignatura = tipoA.IdTipoAsignatura;
                tipoAsignatura.NomTipoAsignatura = tipoA.NomTipoAsignatura;
                tipoAsignatura.CantidadNotas = tipoA.CantidadNotas;
                listado.Add(tipoAsignatura);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarTipoAsignatura")]
        public IActionResult InsertarTipoAsignatura(string nomTipo, int cantidad)
        {
            TipoAsignatura tipo = new TipoAsignatura();
            tipo.NomTipoAsignatura = nomTipo;
            tipo.CantidadNotas = cantidad;
            _context.TipoAsignaturas.Add(tipo);
            _context.SaveChanges();
            return Ok("Insertado");
        }
    }
}
