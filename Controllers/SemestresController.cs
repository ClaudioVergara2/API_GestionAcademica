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
                // Validar que los datos no estén vacíos
                if (string.IsNullOrEmpty(nom))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El nombre del semestre no puede estar vacío." });
                }

                // Validar que el año sea mayor que 0
                if (anio <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El año del semestre debe ser mayor que 0." });
                }

                // Validar que el estado sea 0 o 1
                if (estado != 0 && estado != 1)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El estado del semestre debe ser 0 (INACTIVO) o 1 (ACTIVO)." });
                }

                // Crear una instancia de Semestre
                Semestre st = new Semestre();
                st.NomSemestre = nom;
                st.AnioSemestre = anio;
                st.Estado = estado;

                // Agregar el semestre al contexto y guardar cambios
                _context.Semestres.Add(st);
                _context.SaveChanges();

                // Obtener el estado en formato de texto
                string estadoTexto = estado == 1 ? "ACTIVO" : "INACTIVO";

                // Devolver una respuesta exitosa junto con los datos ingresados
                return StatusCode(StatusCodes.Status200OK, new
                {
                    respuesta = "Insertado correctamente",
                    semestre = new
                    {
                        Nombre = st.NomSemestre,
                        Año = st.AnioSemestre,
                        Estado = estadoTexto
                    }
                });
            }
            catch (Exception ex)
            {
                // Devolver un mensaje de error en caso de excepción
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }
        [HttpPost]
        [Route("EditarEstadoSemestre")]
        public IActionResult EditarEstadoSemestre(int? idSemestre, int? nuevoEstado)
        {
            try
            {
                if (!idSemestre.HasValue || idSemestre <= 0)
                {
                    return BadRequest(new { mensaje = "El ID del semestre no es válido" });
                }

                if (!nuevoEstado.HasValue || (nuevoEstado != 0 && nuevoEstado != 1))
                {
                    return BadRequest(new { mensaje = "El nuevo estado no es válido" });
                }

                var semestre = _context.Semestres.FirstOrDefault(s => s.IdSemestre == idSemestre);

                if (semestre == null)
                {
                    return NotFound(new { mensaje = "Semestre no encontrado" });
                }

                semestre.Estado = nuevoEstado.Value;
                _context.Semestres.Update(semestre);
                _context.SaveChanges();

                return Ok(new { respuesta = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

    }
}
