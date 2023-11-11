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
    public class UsuariosController : Controller
    {
        private readonly EvaluacionContext _context;

        public UsuariosController(EvaluacionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ListadoUsuario")]
        public IActionResult ListadoUsuario()
        {
            List<Usuario> listado = new List<Usuario>();
            var sql = from U in _context.Usuarios
                      select new
                      {
                          U.NomUsuario,
                          U.RutPersona,
                          U.Contraseña
                      };
            foreach (var r in sql)
            {
                Usuario us = new Usuario();
                us.NomUsuario = r.NomUsuario;
                us.RutPersona = r.RutPersona;
                us.Contraseña = r.Contraseña;
                listado.Add(us);
            }
            return Ok(listado);
        }
    }
}
