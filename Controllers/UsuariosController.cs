﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Notas.Models;
using Newtonsoft.Json;
using Humanizer;
using System.Security.Policy;

namespace API_Notas.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        public readonly EvaluacionContext _context;

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
            return Ok(JsonConvert.SerializeObject(listado));
        }

        [HttpPost]
        [Route("AutenticarUsuario")]
        public IActionResult AutenticarUsuario([FromBody] CredencialesUsuario credenciales)
        {
            try
            {
                if (credenciales == null || string.IsNullOrWhiteSpace(credenciales.NombreUsuario) || string.IsNullOrWhiteSpace(credenciales.Contraseña))
                {
                    return BadRequest(new { mensaje = "Error", respuesta = "Nombre de usuario y contraseña son obligatorios." });
                }

                // Buscar el usuario en la base de datos
                var usuario = _context.Usuarios.FirstOrDefault(u => u.NomUsuario == credenciales.NombreUsuario && u.Contraseña == credenciales.Contraseña);

                // Verificar si el usuario fue encontrado
                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Error", respuesta = "Usuario no encontrado o contraseña incorrecta." });
                }

                return Ok(new { mensaje = "Correcto", respuesta = "Usuario autenticado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error", respuesta = ex.Message });
            }
        }

        public class CredencialesUsuario
        {
            public string NombreUsuario { get; set; }
            public string Contraseña { get; set; }
        }
    }
}
