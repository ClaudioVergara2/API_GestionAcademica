using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Notas.Models;
using Newtonsoft.Json;

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
            try
            {
                List<TipoAsignatura> listado = new List<TipoAsignatura>();
                var sql = from ta in _context.TipoAsignaturas
                          select new
                          {
                              ta.IdTipoAsignatura,
                              ta.NomTipoAsignatura,
                              ta.CantidadNotas
                          };
                foreach (var tipoA in sql)
                {
                    TipoAsignatura tipoAsignatura = new TipoAsignatura();
                    tipoAsignatura.IdTipoAsignatura = tipoA.IdTipoAsignatura;
                    tipoAsignatura.NomTipoAsignatura = tipoA.NomTipoAsignatura;
                    tipoAsignatura.CantidadNotas = tipoA.CantidadNotas;
                    listado.Add(tipoAsignatura);
                }
                return Ok(listado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        [HttpPost]
        [Route("InsertarTipoAsignatura")]
        public IActionResult InsertarTipoAsignatura([FromBody] TipoAsignaturaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NomTipoAsignatura) || request.CantidadNotas <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Los datos no pueden estar vacíos." });
                }

                if (_context.TipoAsignaturas.AsEnumerable().Any(t => t.NomTipoAsignatura.Equals(request.NomTipoAsignatura, StringComparison.OrdinalIgnoreCase)))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Ya existe un tipo de asignatura con ese nombre." });
                }

                TipoAsignatura nuevoTipo = new TipoAsignatura
                {
                    NomTipoAsignatura = request.NomTipoAsignatura,
                    CantidadNotas = request.CantidadNotas
                };

                _context.TipoAsignaturas.Add(nuevoTipo);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    mensaje = "Insertado correctamente",
                    idTipoAsignatura = nuevoTipo.IdTipoAsignatura
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        public class TipoAsignaturaRequest
        {
            public string NomTipoAsignatura { get; set; }
            public int CantidadNotas { get; set; }
        }
    }
}
