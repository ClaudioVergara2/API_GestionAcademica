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
    public class PerfilsController : Controller
    {
        private readonly EvaluacionContext _context;

        public PerfilsController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoPerfil")]
        public IActionResult ListadoPerfil()
        {
            List<Perfil> listado = new List<Perfil>();
            var sql = from per in _context.Perfils
                      select new
                      {
                          per.IdPerfil,
                          per.NomPerfil
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
    }
}
