using Microsoft.EntityFrameworkCore;

namespace MvcNetCoreTiendaCubosRubik.Data
{
    public class CubosContext: DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options) : base(options)
        {}

        public DbSet<Models.Cubo> Cubos { get; set; }
    }
}
