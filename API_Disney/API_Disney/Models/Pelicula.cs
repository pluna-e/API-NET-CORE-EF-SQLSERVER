using System.ComponentModel.DataAnnotations;

namespace API_Disney.Models
{
    public class Pelicula
    {
        public Pelicula()
        {
            this.Personajes = new HashSet<Personaje>();
        }

        [Key]
        public int PeliculaId { get; set; }
        [Required]
        public byte[] Imagen { get; set; }

        [Required]
        public string Titulo { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }//del 1 al 5

        //personajes asociados
        public virtual ICollection<Personaje> Personajes { get; set; } //relacion m-n

        //genero
        public int GeneroId { get; set; }//EF genera que la fk sea NotNull
        public Genero Genero { get; set; }//EF genera la fk de genero en la tabla pelicula
    }
}
