using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

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

        [HttpPost]
        public IActionResult Create(ProductViewModel product, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                string name;
                
                product.ImageUrl = "https://fakeimg.pl/300/";
                //_db.Add(product);
                //_db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
