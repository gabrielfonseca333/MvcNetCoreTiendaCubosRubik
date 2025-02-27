﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreTiendaCubosRubik.Models
{
    [Table("Compra")]
    public class Compra
    {
        //tiene: id_compra, id_cubo, cantidad, precio, fechapedido.
        [Key]
        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Column("id_cubo")]
        public int IdCubo { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio")]
        public int Precio { get; set; }

        [Column("fechapedido")]
        public DateTime FechaPedido { get; set; }
    }
}
