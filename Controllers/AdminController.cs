using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using System.Net;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _db;

        public AdminController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Product?.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Products(string? errorMessage, TypesOfProduct? type)
        {
            if (!errorMessage.IsNullOrEmpty())
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
            List<ProductModel>? products;
            switch (type)
            {
                case TypesOfProduct.Marble:
                    products = _db.Product!.Where(a => a.TypeOfProduct == TypesOfProduct.Marble)
                        .OrderBy(a => a.NumberName).ToList();
                    break;
                case TypesOfProduct.Crumble:
                    products = _db.Product!.Where(a => a.TypeOfProduct == TypesOfProduct.Crumble)
                        .OrderBy(a => a.NumberName).ToList();
                    break;
                default:
                    products = _db.Product!.OrderBy(a => a.TypeOfProduct).ThenBy(a => a.NumberName)
                    .ToList();
                    break;
            }
            
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Product!.FindAsync(id);
            if(product != null)
            {
                _db.Product.Remove(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            return RedirectToAction("Products", new { errorMessage = "Обьект не найден" });
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

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productToUpdate = await _db.Product!
                .FirstOrDefaultAsync(s => s.Id == id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<ProductModel>(
                productToUpdate,
                "",
                s => s.NumberName, s => s.Price, s => s.TypeOfProduct))
            {
                try
                {
                    productToUpdate.SetDbName();
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Products");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(productToUpdate);
        }
    }
}
