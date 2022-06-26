using API_Disney.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Disney.Data
{
    public class DisneyDbContext:DbContext
    {
            public DisneyDbContext(DbContextOptions options) : base(options) { }

            public DbSet<Genero> Generos { get; set; }
            public DbSet<Pelicula> Peliculas { get; set; }
            public DbSet<Personaje> Personajes { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
    }
}
