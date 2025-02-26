using Microsoft.AspNetCore.Mvc;
using MvcNetCoreTiendaCubosRubik.Models;
using MvcNetCoreTiendaCubosRubik.Repositories;

namespace MvcNetCoreTiendaCubosRubik.Controllers
{
    public class CubosController : Controller
    {

        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this.repo.GetCubosAsync();
            return View(cubos);
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
    }
}
