using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = _db.Product!.OrderBy(a => a.TypeOfProduct).ThenBy(a => a.NumberName)
                .ToList();
            return View(products);
        }

        public IActionResult ProductsCrumble()
        {
            var products = _db.Product!.Where(a => a.TypeOfProduct == TypesOfProduct.Crumble)
                .OrderBy(a => a.NumberName).ToList();
            return View(products);
        }

        public IActionResult ProductsMarble()
        {
            var products = _db.Product!.Where(a => a.TypeOfProduct == TypesOfProduct.Marble)
                .OrderBy(a => a.NumberName).ToList();
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}