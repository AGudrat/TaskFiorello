using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
   
    public class ProductController : Controller
    {
        private AppDbContext _context { get; }

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Products
                                .Where(p => p.IsDeleted==false)
                                .Include(c => c.Category)
                                .OrderByDescending(p => p.Id)
                                .ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            bool isExsist = _context.Products
                                    .Any(p => p.Name.ToLower().Trim() == product.Name.ToLower().Trim());
            if (isExsist)
            {
                ModelState.AddModelError("Name", "*Bu product artıq mövcuddur.");
                return View();
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
