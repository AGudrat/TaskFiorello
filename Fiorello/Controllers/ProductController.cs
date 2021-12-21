using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext _context { get; }
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Products.OrderByDescending(p=>p.Id)
                                         .Take(8)
                                         .Include(p => p.Images));
        }


        public IActionResult LoadProduct(int skip)
        {
            var model = _context.Products.OrderByDescending(p => p.Id)
                                         .Skip(skip)
                                         .Take(8)
                                         .Include(p => p.Images)
                                         .ToList();
            return PartialView("_ProductPartial", model);
        }
    }
}
