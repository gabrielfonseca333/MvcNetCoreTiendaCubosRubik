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

        //---------------------- METODOS CRUD ----------------------
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
    }
}
