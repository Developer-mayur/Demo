using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;

namespace DemoApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineShopContext _db;

        public AdminController(OnlineShopContext db)
        {
            _db = db;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _db.Products.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await product.ImageFile.CopyToAsync(ms);
                    product.ImageData = ms.ToArray();
                }

                product.CreatedAt = DateTime.Now;
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _db.Products.FindAsync(id);
                    if (existingProduct == null) return NotFound();

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CreatedAt = product.CreatedAt;

                    if (product.ImageFile != null && product.ImageFile.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await product.ImageFile.CopyToAsync(memoryStream);
                        existingProduct.ImageData = memoryStream.ToArray();
                    }

                    _db.Update(existingProduct);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Products.Any(e => e.ProductId == product.ProductId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Method to serve image from binary data
        public async Task<IActionResult> GetImage(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null || product.ImageData == null)
                return NotFound();

            return File(product.ImageData, "image/jpeg");
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Any(e => e.ProductId == id);
        }
    }
}
