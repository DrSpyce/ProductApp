using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _appEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _db = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult GetFiles()
        {
            var googleService = new GoogleDriveService();
            googleService.ListAllFilesAndFolders();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product, IFormFile uploadedFile)
        {
            product.SetDbName();
            if (_db.Product!.Any(p => p.ShownName == product.ShownName))
            {
                ModelState.AddModelError("DbName", "DbName can't be duplicated");
            }
            if (ModelState.IsValid)
            {
                var googleService = new GoogleDriveService();
                product.ImageUrl = await googleService.UploadImageAsync(uploadedFile,
                    product.ShownName);
                _db.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
