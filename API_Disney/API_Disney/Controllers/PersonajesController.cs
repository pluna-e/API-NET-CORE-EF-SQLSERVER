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
    [Route("/characters")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private readonly DisneyDbContext _context;

        public PersonajesController(DisneyDbContext context)
        {
            _context = context;
        }

        //GET: /characters
        [HttpGet]
        public IQueryable GetPersonajes(string? name,int? age,int? idPeli)
        {
            IQueryable resultado;

            // /characters?name = nombre
            IQueryable filtrarNombre(string? nombre)
            {
                resultado = from per in _context.Personajes
                            where per.Nombre == nombre
                            select new
                            {
                                Imagen = per.Imagen,
                                Nombre = per.Nombre
                            };
                return resultado.AsQueryable();
            }

            // /characters?age = edad
            IQueryable filtrarEdad(int? edad)
            {
                resultado = from per in _context.Personajes
                            where per.Edad == edad
                            select new
                            {
                                Imagen = per.Imagen,
                                Nombre = per.Nombre
                            };
                return resultado.AsQueryable();
            }

            // /characters?movies = idMovie
            IQueryable filtrarPeliculas(int? idPelicula)
            {
                resultado = from pel in _context.Peliculas
                            where pel.PeliculaId == idPelicula
                            select new
                            {
                                Imagen = pel.Personajes.Select(p=>p.Imagen),
                                Nombre = pel.Personajes.Select(p=>p.Nombre)
                            };
                return resultado.AsQueryable();
            }
            IQueryable mostrarTodos()
            {
                resultado = from per in _context.Personajes
                            select new
                            {
                                Imagen = per.Imagen,
                                Nombre = per.Nombre
                            };
                return resultado.AsQueryable();
            }

            if(name != null){ return filtrarNombre(name);}
            if(age != null){ return filtrarEdad(age);}
            if(idPeli != null) { return filtrarPeliculas(idPeli);}

            return mostrarTodos();
            //var url = HttpContext.Request.Query["name"];

            //if (url.Count > 0)
            //{
            //    resultado = from per in _context.Personajes
            //                where per.Nombre == url[0].ToString()
            //                select new
            //                {
            //                    Imagen = per.Imagen,
            //                    Nombre = per.Nombre
            //                };
            //    return resultado;
            //}
        }

        //DETALLE PERSONAJE
        [HttpGet("detalles/{id}")]
        public IQueryable DetallePersonaje(int id)
        {
            IQueryable result = from per in _context.Personajes
                                where per.PersonajeId == id
                                select new
                                {
                                    Nombre = per.Nombre,
                                    Edad = per.Edad,
                                    Peso = per.Peso,
                                    Historia = per.Historia,
                                    Imagen = per.Imagen,
                                    Peliculas = per.Peliculas.Select(p=>p.Titulo)
                                };
            return result.AsQueryable();
        }

        // PUT: api/Personajes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonaje(int id, Personaje personaje)
        {
            if (id != personaje.PersonajeId)
            {
                return BadRequest();
            }

            _context.Entry(personaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonajeExists(id))
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

        // POST: api/Personajes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Personaje>> PostPersonaje(Personaje personaje)
        {
          if (_context.Personajes == null)
          {
              return Problem("Entity set 'DisneyDbContext.Personajes'  is null.");
          }
            _context.Personajes.Add(personaje);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonaje", new { id = personaje.PersonajeId }, personaje);
        }

        // DELETE: api/Personajes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonaje(int id)
        {
            if (_context.Personajes == null)
            {
                return NotFound();
            }
            var personaje = await _context.Personajes.FindAsync(id);
            if (personaje == null)
            {
                return NotFound();
            }

            _context.Personajes.Remove(personaje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonajeExists(int id)
        {
            return (_context.Personajes?.Any(e => e.PersonajeId == id)).GetValueOrDefault();
        }
    }
}
