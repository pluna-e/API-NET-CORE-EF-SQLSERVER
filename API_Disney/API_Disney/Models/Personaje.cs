using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API_Disney.Models
{
    public class Personaje
    {
        public Personaje()
        {
            this.Peliculas = new HashSet<Pelicula>();
        }

        [Key]//convención de EF 6 si el nombre del atributo lleva "Id" al final no hace falta poner el DataAnnotation
        public int PersonajeId { get; set; }

        [Required]//un atributo Required si al momento de querer guardar la entidad se encuentra en null lanzará la siguiente Exception ->  System.Data.Entity.Validation.DbEntityValidationException 
        public byte[] Imagen { get; set; }

        [Required]
        [FromQuery(Name ="name")]
        public string Nombre { get; set; }

        [FromQuery(Name = "age")]
        public int Edad { get; set; }
        public double Peso { get; set; }
        public string Historia { get; set; }

        //peliculas o series asociadas-----> relación m-n
        public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
