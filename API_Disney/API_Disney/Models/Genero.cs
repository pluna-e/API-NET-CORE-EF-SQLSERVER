using System.ComponentModel.DataAnnotations;

namespace API_Disney.Models
{
    public class Genero
    {
        [Key]
        public int GeneroId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public byte[] Imagen { get; set; }

        //peliculas o series asociadas
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
