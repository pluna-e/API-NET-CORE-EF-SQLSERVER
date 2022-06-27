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
    [Route("/movies")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly DisneyDbContext _context;

        public PeliculasController(DisneyDbContext context)
        {
            _context = context;
        }

        // Listado de peliculas deberá mostrar solamente los campos imagen, título y fecha de creación.
        [HttpGet]
        public IQueryable GetPeliculas(string? name,int? genre,string? order)
        {
            IQueryable respuesta;
            // / movies ? name = nombre
            // / movies ? genre = idGenero
            // / movies ? order = ASC | DESC
            IQueryable filtroTitulo(string? nombre)
            {
                respuesta = from pel in _context.Peliculas
                            where pel.Titulo == nombre
                            select new
                            {
                                Imagen = pel.Imagen,
                                Titulo = pel.Titulo,
                                Fechadecreación = pel.FechaCreacion
                            };
                return respuesta.AsQueryable();
            }
            IQueryable filtroGenero(int? genero)
            {
                respuesta = from pel in _context.Peliculas
                            where pel.GeneroId == genero
                            select new
                            {
                                Imagen = pel.Imagen,
                                Titulo = pel.Titulo,
                                Fechadecreación = pel.FechaCreacion
                            };
                return respuesta.AsQueryable();
            }
            IQueryable ordenar(string? ordenTipo)
            {
                if (ordenTipo.Equals("ASC"))
                {
                    respuesta = from pel in _context.Peliculas
                                orderby pel.FechaCreacion ascending
                                select new
                                {
                                    Imagen = pel.Imagen,
                                    Titulo = pel.Titulo,
                                    Fechadecreación = pel.FechaCreacion
                                };
                }
                else
                {
                    respuesta = from pel in _context.Peliculas
                                orderby pel.FechaCreacion descending
                                select new
                                {
                                    Imagen = pel.Imagen,
                                    Titulo = pel.Titulo,
                                    Fechadecreación = pel.FechaCreacion
                                };
                }
                 return respuesta.AsQueryable();
            }

            IQueryable mostrarTodas()
            {
                respuesta = from pel in _context.Peliculas
                                select new
                                {
                                    Imagen = pel.Imagen,
                                    Titulo = pel.Titulo,
                                    Fechadecreación = pel.FechaCreacion
                                };
                return respuesta.AsQueryable();

            }

            if (name != null) { return filtroTitulo(name); }
            if (genre != null) { return filtroGenero(genre); }
            if (order != null) { return ordenar(order); }

            return mostrarTodas();
        }

        // GET: api/Peliculas/5
        [HttpGet("/detalle/{id}")]
        public IQueryable GetPelicula(int id)
        {
            IQueryable respuesta = from pel in _context.Peliculas
                                   where pel.PeliculaId == id
                                   select new
                                   {
                                       Titulo = pel.Titulo,
                                       Imagen = pel.Imagen,
                                       FecCreaccion = pel.FechaCreacion,
                                       Generos = pel.Genero.Nombre,
                                       Calificación = pel.Calificacion,
                                       Personajes = pel.Personajes.Select(personaje => personaje.Nombre)
                                   };
            return respuesta.AsQueryable();
        }

        // PUT: api/Peliculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, Pelicula pelicula)
        {
            if (id != pelicula.PeliculaId)
            {
                return BadRequest();
            }

            _context.Entry(pelicula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(id))
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

        // POST: api/Peliculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pelicula>> PostPelicula(Pelicula pelicula)
        {
          if (_context.Peliculas == null)
          {
              return Problem("Entity set 'DisneyDbContext.Peliculas'  is null.");
          }
            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPelicula", new { id = pelicula.PeliculaId }, pelicula);
        }

        // DELETE: api/Peliculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            if (_context.Peliculas == null)
            {
                return NotFound();
            }
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeliculaExists(int id)
        {
            return (_context.Peliculas?.Any(e => e.PeliculaId == id)).GetValueOrDefault();
        }
    }
}
