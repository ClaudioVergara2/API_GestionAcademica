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
    public class CursosController : Controller
    {
        private readonly EvaluacionContext _context;

        public CursosController(EvaluacionContext context)
        {
            _context = context;
        }

        // GET: Cursos
        public async Task<IActionResult> Index()
        {
            var evaluacionContext = _context.Cursos.Include(c => c.CodAsignaturaNavigation);
            return View(await evaluacionContext.ToListAsync());
        }

        // GET: Cursos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.CodAsignaturaNavigation)
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursos/Create
        public IActionResult Create()
        {
            ViewData["CodAsignatura"] = new SelectList(_context.Asignaturas, "CodAsignatura", "CodAsignatura");
            return View();
        }

        // POST: Cursos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCurso,RutPersona,CodAsignatura,Seccion")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodAsignatura"] = new SelectList(_context.Asignaturas, "CodAsignatura", "CodAsignatura", curso.CodAsignatura);
            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            ViewData["CodAsignatura"] = new SelectList(_context.Asignaturas, "CodAsignatura", "CodAsignatura", curso.CodAsignatura);
            return View(curso);
        }

        // POST: Cursos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCurso,RutPersona,CodAsignatura,Seccion")] Curso curso)
        {
            if (id != curso.IdCurso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.IdCurso))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodAsignatura"] = new SelectList(_context.Asignaturas, "CodAsignatura", "CodAsignatura", curso.CodAsignatura);
            return View(curso);
        }

        // GET: Cursos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.CodAsignaturaNavigation)
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cursos == null)
            {
                return Problem("Entity set 'EvaluacionContext.Cursos'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
          return (_context.Cursos?.Any(e => e.IdCurso == id)).GetValueOrDefault();
        }
        [HttpGet]
        [Route("ListadoCursos")]
        public IActionResult ListadoCursos()
        {
            List<Curso> listado = new List<Curso>();
            var sql = from c in _context.Cursos
                      select new
                      {
                          c.IdCurso,
                          c.RutPersona,
                          c.CodAsignatura,
                          c.Seccion
                      };
            foreach (var cu in sql)
            {
                Curso curso = new Curso();
                curso.IdCurso = cu.IdCurso;
                curso.RutPersona = cu.RutPersona;
                curso.Seccion = cu.Seccion;
                listado.Add(curso);
            }
            return Ok(listado);
        }
        [HttpPost]
        [Route("InsertarCursos")]
        public IActionResult InsertarCursos(string rut, string cod, int seccion)
        {
            try
            {
                Curso cur = new Curso();
                cur.RutPersona = rut;
                cur.CodAsignatura = cod;
                cur.Seccion = seccion;
                _context.Cursos.Add(cur);
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
