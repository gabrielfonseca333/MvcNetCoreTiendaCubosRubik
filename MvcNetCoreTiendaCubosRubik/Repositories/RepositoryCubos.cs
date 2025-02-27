using Microsoft.EntityFrameworkCore;
using MvcNetCoreTiendaCubosRubik.Data;
using MvcNetCoreTiendaCubosRubik.Models;

namespace MvcNetCoreTiendaCubosRubik.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;
        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        //---------------------- METODOS CRUD CUBOS ----------------------
        //get
        public async Task<List<Cubo>> GetCubosAsync()
        {
            var consulta = from datos in this.context.Cubos
                           select datos;
            return await consulta.ToListAsync();
        }

        //get by id
        public async Task<Cubo> FindCuboAsync(int id)
        {
            var consulta = from datos in this.context.Cubos
                           where datos.IdCubo == id
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        //add
        public async Task InsertCuboAsync(int idCubo, string nombre, string modelo, string marca, string imagen, int precio)
        {
            Cubo cubo = new Cubo();
            cubo.IdCubo = idCubo;
            cubo.Nombre = nombre;
            cubo.Modelo = modelo;
            cubo.Marca = marca;
            cubo.Imagen = imagen;
            cubo.Precio = precio;
            this.context.Cubos.Add(cubo);
            await this.context.SaveChangesAsync();

        }

        //update
        public async Task UpdateCuboAsync(int idCubo, string nombre, string modelo, string marca, string imagen, int precio)
        {
            Cubo cubo = await this.FindCuboAsync(idCubo);
            cubo.Nombre = nombre;
            cubo.Modelo = modelo;
            cubo.Marca = marca;
            cubo.Imagen = imagen;
            cubo.Precio = precio;
            await this.context.SaveChangesAsync();
        }


        //---------------------- METODOS COMPRA ----------------------
        /*
         tengo que hacer un metodo que inserte varios cubos a la vez, porque el usuario hace la compra desde el carrito. 
        No se si el metodo del repositorio es solo uno a la vez o puedo hacer varios inserts a la vez.
        La tabla compra tiene: id_compra, id_cubo, cantidad, precio, fechapedido.

        //vamos a hacer en Insert de compra

         */

        //add compra
        public async Task InsertCompraAsync(int idCompra, int idCubo, int cantidad, int precio, DateTime fechaPedido)
        {
            Compra compra = new Compra();
            compra.IdCompra = idCompra;
            compra.IdCubo = idCubo;
            compra.Cantidad = cantidad;
            compra.Precio = precio;
            compra.FechaPedido = fechaPedido;
            this.context.Compras.Add(compra);
            await this.context.SaveChangesAsync();
        }

        //add varias compras a la vez
        public async Task InsertComprasAsync(List<Compra> compras)
        {
            this.context.Compras.AddRange(compras);
            await this.context.SaveChangesAsync();
        }


    }
}
