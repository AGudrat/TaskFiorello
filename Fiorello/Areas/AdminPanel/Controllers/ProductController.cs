using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities;
using Fiorello.ViewModel.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IWebHostEnvironment _env { get; }

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1,int take=5)
        {
            var products = await _context.Products
                                   .Where(p => p.IsDeleted == false)
                                   .OrderByDescending(p => p.Id)
                                   .Skip((page-1)*take)
                                   .Take(take)
                                   .Include(p => p.Images)
                                   .Include(p => p.Category)
                                   .ToListAsync();
            var productVms = GetProductList(products);
            int pageCount = GetPageCount(take);
            Paginate<ProductListViewModel> model = new Paginate<ProductListViewModel>(productVms, page, pageCount);
            return View(model);
        }
        private int GetPageCount(int take)
        {
            var productCount =_context.Products.Where(p => p.IsDeleted == false).Count();
            return (int)Math.Ceiling(((decimal)productCount / take));
        }

        private List<ProductListViewModel> GetProductList(List<Product> products)
        {
            List<ProductListViewModel> model = new List<ProductListViewModel>();
            foreach (var item in products)
            {
                var product = new ProductListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Count = item.Count,
                    Price = item.Price,
                    CategoryName = item.Category.Name,
                    Image = item.Images.Where(i => i.IsMain).FirstOrDefault().Image
                };
                model.Add(product);
            }
            return model;
        }
      
    }
}
