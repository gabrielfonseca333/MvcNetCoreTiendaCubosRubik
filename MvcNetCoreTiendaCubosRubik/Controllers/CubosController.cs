using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcNetCoreTiendaCubosRubik.Extensions;
using MvcNetCoreTiendaCubosRubik.Helpers;
using MvcNetCoreTiendaCubosRubik.Models;
using MvcNetCoreTiendaCubosRubik.Repositories;

namespace MvcNetCoreTiendaCubosRubik.Controllers
{
    public class CubosController : Controller
    {

        private RepositoryCubos repo;
        HelperSessionContextAccessor helper;
        private IMemoryCache memoryCache;

        public CubosController(RepositoryCubos repo, HelperSessionContextAccessor helper, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.helper = helper;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index(int idcubo, string accion)
        {
            List<Cubo> cubos = await this.repo.GetCubosAsync();

            #region CacheFavoritos
            //__________________ COMIENZO PARA CACHE (FAVORITOS) _____________________

            // Definir la clave para la caché
            string cacheKey = "FAVORITOS";

            // Recuperar los favoritos de la caché
            List<Cubo> favoritos;
            if (!memoryCache.TryGetValue(cacheKey, out favoritos))
            {
                favoritos = new List<Cubo>();
            }

            if (accion != null)
            {
                if (accion.ToLower() == "favorito")
                {
                    Cubo cubo = await this.repo.FindCuboAsync(idcubo);

                    // Verificar si ya está en favoritos
                    if (!favoritos.Any(c => c.IdCubo == cubo.IdCubo))
                    {
                        favoritos.Add(cubo);

                        // Guardar en caché con una duración de 10 minutos
                        memoryCache.Set(cacheKey, favoritos, TimeSpan.FromMinutes(10));
                    }

                    ViewData["MENSAJE"] = "Cubo añadido a favoritos ⭐";
                }
                else if (accion.ToLower() == "quitarfavorito")
                {
                    // Eliminar el cubo de favoritos
                    Cubo cuboAEliminar = favoritos.FirstOrDefault(c => c.IdCubo == idcubo);
                    if (cuboAEliminar != null)
                    {
                        favoritos.Remove(cuboAEliminar);
                        memoryCache.Set(cacheKey, favoritos, TimeSpan.FromMinutes(10));
                    }

                    ViewData["MENSAJE"] = "Cubo eliminado de favoritos ❌";
                }
            }

            // Enviar la lista de favoritos a la vista
            ViewData["Favoritos"] = favoritos;

            //__________________ FIN PARA CACHE (FAVORITOS) _____________________
            #endregion

            #region sessionCarrito
            //__________________ COMIENZO PARA SESSION (CARRITO) _____________________
            // Recuperar los cubos almacenados en el carrito
            List<Cubo> cubosCarrito = HttpContext.Session.GetObject<List<Cubo>>("CUBOS");

            if (cubosCarrito == null)
            {
                cubosCarrito = new List<Cubo>();
            }

            if (accion != null)
            {
                if (accion.ToLower() == "almacenar")
                {
                    Cubo cubo = await this.repo.FindCuboAsync(idcubo);

                    if (!cubosCarrito.Any(c => c.IdCubo == cubo.IdCubo))
                    {
                        cubosCarrito.Add(new Cubo
                        {
                            IdCubo = cubo.IdCubo,
                            Nombre = cubo.Nombre,
                            Modelo = cubo.Modelo,
                            Marca = cubo.Marca,
                            Imagen = cubo.Imagen,
                            Precio = cubo.Precio
                        });

                        HttpContext.Session.SetObject("CUBOS", cubosCarrito);
                    }

                    ViewData["MENSAJE"] = "Cubo añadido al carrito 🛒";
                }
                else if (accion.ToLower() == "quitar")
                {
                    // Eliminar el cubo del carrito
                    Cubo cuboAEliminar = cubosCarrito.FirstOrDefault(c => c.IdCubo == idcubo);
                    if (cuboAEliminar != null)
                    {
                        cubosCarrito.Remove(cuboAEliminar);
                        HttpContext.Session.SetObject("CUBOS", cubosCarrito);
                    }

                    ViewData["MENSAJE"] = "Cubo eliminado del carrito ❌";
                }
            }

            // Enviar la lista de cubos del carrito a la vista
            ViewData["Carrito"] = cubosCarrito;
            //__________________ FIN PARA SESSION (CARRITO) _____________________
            #endregion


            return View(cubos);
        }



        public async Task <IActionResult> Carrito(string accion, int idcubo)
        {

            
            List<Cubo> cubosCarrito = this.helper.GetCubosSession();

            // Clave de caché para los favoritos
            string cacheKey = "FAVORITOS";

            // Recuperar los favoritos de la caché
            List<Cubo> favoritos;
            if (!memoryCache.TryGetValue(cacheKey, out favoritos))
            {
                favoritos = new List<Cubo>();
            }

            if (accion != null)
            {
                if (accion.ToLower() == "comprar")
                {
                    #region logica de compra

                    // Insertar los cubos en la tabla Compra
                    foreach (Cubo cubo in cubosCarrito)
                    {

                        /*
                         Mi repository tiene un metodo InsertCompraAsync que recibe los parametros de la tabla Compra.
                            id_compra, id_cubo, cantidad, precio, fechapedido.

                        tengo que modificar el metodo para que reciba un objeto de tipo Compra, 
                        porque el usuario puede comprar varios cubos a la vez.

                        Además Implementamos la posibilidad de CANTIDAD de producto en el carrito.
                        Si cambiamos la cantidad, cambiará el PRECIO TOTAL EN CLIENTE.

                        Dime como proceder PASO A PASO. 
                        Dime tanto en el Controller como en la View de Carrito. 

                         */

                    }
                    #endregion
                }
                else if (accion.ToLower() == "quitar")
                {
                    // Eliminar el cubo del carrito
                    Cubo cuboAEliminar = cubosCarrito.FirstOrDefault(c => c.IdCubo == idcubo);
                    if (cuboAEliminar != null)
                    {
                        cubosCarrito.Remove(cuboAEliminar);
                        HttpContext.Session.SetObject("CUBOS", cubosCarrito);
                    }
                }
                else if (accion.ToLower() == "favorito")
                {
                    Cubo cubo = cubosCarrito.FirstOrDefault(c => c.IdCubo == idcubo);
                    if (cubo != null && !favoritos.Any(c => c.IdCubo == cubo.IdCubo))
                    {
                        favoritos.Add(cubo);
                        memoryCache.Set(cacheKey, favoritos, TimeSpan.FromMinutes(10));
                    }
                }
                else if (accion.ToLower() == "quitarfavorito")
                {
                    Cubo cuboAEliminar = favoritos.FirstOrDefault(c => c.IdCubo == idcubo);
                    if (cuboAEliminar != null)
                    {
                        favoritos.Remove(cuboAEliminar);
                        memoryCache.Set(cacheKey, favoritos, TimeSpan.FromMinutes(10));
                    }
                }
            }

            // Pasar la lista de favoritos a la vista
            ViewData["Favoritos"] = favoritos;

            return View(cubosCarrito);
        }


        public async Task<IActionResult> Details(int idcubo)
        {
            Cubo cubo = await this.repo.FindCuboAsync(idcubo);
            return View(cubo);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cubo cubo, IFormFile fichero)
        {

            if(fichero == null)
            {
                ViewBag.Mensaje = "Debes seleccionar una imagen";
                return View();
            }

            // Definir la carpeta donde se guardarán las imágenes
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "cubos");

            // Asegurarnos de que la carpeta existe
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Guardar la imagen con un nombre único
            string fileName = Path.GetFileName(fichero.FileName);
            string filePath = Path.Combine(folderPath, fileName);

            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }

            cubo.Imagen = fileName;

            await this.repo.InsertCuboAsync(cubo.IdCubo, cubo.Nombre, cubo.Modelo, cubo.Marca, cubo.Imagen, cubo.Precio);




            ViewBag.Mensaje = "Cubo añadido ✅";
            return View();
        }


        public async Task<IActionResult> Edit(int idcubo)
        {
            Cubo cubo = await this.repo.FindCuboAsync(idcubo);
            if (cubo == null)
            {
                return NotFound();
            }
            return View(cubo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cubo cubo, IFormFile fichero)
        {
            if (fichero != null && fichero.Length > 0)
            {
                // Definir la carpeta donde se guardarán las imágenes
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "cubos");

                // Asegurarnos de que la carpeta existe
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Guardar la nueva imagen con un nombre único
                string fileName = Path.GetFileName(fichero.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    await fichero.CopyToAsync(stream);
                }

                // Asignar la nueva imagen al cubo
                cubo.Imagen = fileName;
            }
            else
            {
                // Si no se sube una nueva imagen, mantener la imagen actual
                Cubo cuboExistente = await this.repo.FindCuboAsync(cubo.IdCubo);
                cubo.Imagen = cuboExistente.Imagen;
            }

            // Actualizar el cubo en la base de datos
            await this.repo.UpdateCuboAsync(cubo.IdCubo, cubo.Nombre, cubo.Modelo, cubo.Marca, cubo.Imagen, cubo.Precio);

            ViewBag.Mensaje = "Cubo actualizado ✅";
            return View(cubo);
        }




    }
}
