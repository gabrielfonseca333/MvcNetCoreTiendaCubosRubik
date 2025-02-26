using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreTiendaCubosRubik.Models
{
    [Table("Cubos")]
    public class Cubo
    {
        //id_cubo
        [Key]
        [Column("id_cubo")]
        public int IdCubo { get; set; }

        //nombre
        [Column("nombre")]
        public string Nombre { get; set; }

        //modelo
        [Column("modelo")]
        public string Modelo { get; set; }

        //marca
        [Column("marca")]
        public string Marca { get; set; }

        //imagen
        [Column("imagen")]
        public string Imagen { get; set; }

        //precio
        [Column("precio")]
        public int Precio { get; set; }
    }
}
