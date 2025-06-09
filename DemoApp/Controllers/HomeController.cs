using System.Diagnostics;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineShopContext _db;

        public HomeController(ILogger<HomeController> logger, OnlineShopContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _db.Products.ToListAsync();
                if (products == null)
                {
                    return NotFound();
                }

                return View(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(500, "Internal server error");
            }
        }


        public IActionResult About()
        {
            return View();
        }
    }
}
