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
    public class PerfilsController : ControllerBase
    {
        public readonly EvaluacionContext _context;

        public PerfilsController(EvaluacionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListadoPerfiles")]
        public IActionResult ListadoPerfiles()
        {
            List<Perfil> listado = _context.Perfils
                .Select(per => new Perfil
                {
                    IdPerfil = per.IdPerfil,
                    NomPerfil = per.NomPerfil
                })
                .ToList();

            return Ok(JsonConvert.SerializeObject(listado));
        }

        [HttpPost]
        [Route("InsertarPerfil")]
        public IActionResult InsertarPerfil([FromBody] PerfilRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NomPerfil))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El nombre del perfil no puede estar vacío." });
                }

                var nuevoPerfil = new Perfil
                {
                    NomPerfil = request.NomPerfil
                };

                _context.Perfils.Add(nuevoPerfil);
                _context.SaveChanges();

                var response = new InsertarPerfilResponse
                {
                    Respuesta = "Insertado correctamente",
                    Perfil = new PerfilResponse
                    {
                        Nombre = nuevoPerfil.NomPerfil
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        public class InsertarPerfilResponse
        {
            public string Respuesta { get; set; }
            public PerfilResponse Perfil { get; set; }
        }

        public class PerfilResponse
        {
            public string Nombre { get; set; }
        }

        public class PerfilRequest
        {
            public string NomPerfil { get; set; }
        }
    }
}
