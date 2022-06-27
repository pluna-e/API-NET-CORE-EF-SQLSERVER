using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Disney.Data;
using API_Disney.Models;

namespace API_Disney.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly DisneyDbContext _context;

        public GenerosController(DisneyDbContext context)
        {
            _context = context;
        }

        // GET: api/Generos
        [HttpGet]
        public IQueryable GetGeneros()
        {
            var response = from pel in _context.Peliculas
                           join gen in _context.Generos
                           on pel.GeneroId equals gen.GeneroId
                           select new
                           {
                               Nombre = gen.Nombre,
                               Imagen = gen.Imagen,
                               Pelicula = pel.Titulo
                           };
            return response.AsQueryable();
        }

        // GET: api/Generos/5
        [HttpGet("{id}")]
        public IQueryable GetGenero(int id)
        {
            var genero =  _context.Generos.Find(id);

           
            var response = from pel in _context.Peliculas
                           join gen in _context.Generos
                           on pel.GeneroId equals id
                           select new
                           {
                               Nombre = genero.Nombre,
                               Imagen = genero.Imagen,
                               Pelicula = pel.Titulo
                           };
            return response.AsQueryable();
        }

        // PUT: api/Generos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenero(int id, Genero genero)
        {
            if (id != genero.GeneroId)
            {
                return BadRequest();
            }

            _context.Entry(genero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Generos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genero>> PostGenero(Genero genero)
        {
          if (_context.Generos == null)
          {
              return Problem("Entity set 'DisneyDbContext.Generos'  is null.");
          }
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenero", new { id = genero.GeneroId }, genero);
        }

        // DELETE: api/Generos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            if (_context.Generos == null)
            {
                return NotFound();
            }
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroExists(int id)
        {
            return (_context.Generos?.Any(e => e.GeneroId == id)).GetValueOrDefault();
        }
    }
}
