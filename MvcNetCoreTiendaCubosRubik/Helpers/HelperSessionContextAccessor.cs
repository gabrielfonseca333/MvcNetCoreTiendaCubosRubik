using MvcNetCoreTiendaCubosRubik.Extensions;
using MvcNetCoreTiendaCubosRubik.Models;

namespace MvcNetCoreTiendaCubosRubik.Helpers
{
    public class HelperSessionContextAccessor
    {
        private IHttpContextAccessor contextAccessor;

        public HelperSessionContextAccessor(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public List<Cubo> GetCubosSession()
        { 
            List<Cubo> cubos = this.contextAccessor.HttpContext.Session.GetObject<List<Cubo>>("CUBOS");
            return cubos;
        }
    }
}
