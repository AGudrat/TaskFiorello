﻿using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _context.Products
                                    .Where(p => p.IsDeleted == false)
                                    .Include(p => p.Category)
                                    .Include(p => p.Images)
                                    .OrderByDescending(p => p.Id)
                                    .Take(8)
                                    .ToListAsync();
            return View(await Task.FromResult(model));
        }
    }
}
