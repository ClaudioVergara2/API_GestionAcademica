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
            List<Perfil> listado = new List<Perfil>();
            var sql = from per in _context.Perfils
                      select new
                      {
                          per.IdPerfil,
                          per.NomPerfil,
                      };
            foreach (var perf in sql)
            {
                Perfil perfil = new Perfil();
                perfil.IdPerfil = perf.IdPerfil;
                perfil.NomPerfil = perf.NomPerfil;
                listado.Add(perfil);
            }
            return Ok(listado);
        }

        [HttpPost]
        [Route("InsertarPerfil")]
        public IActionResult InsertarPerfil(string nom)
        {
            try
            {
                Perfil perfil = new Perfil();
                perfil.NomPerfil = nom;
                _context.Perfils.Add(perfil);
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
