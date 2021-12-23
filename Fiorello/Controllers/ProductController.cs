using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            return View(_context.Products.OrderByDescending(p => p.Id)
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasketAsync(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return BadRequest();
            List<BasketViewModel> basket = GetBasket();
            UpdateBasket((int)id, basket);
            return RedirectToAction("Index", "Product");
        }
        private void UpdateBasket(int id, List<BasketViewModel> basket)
        {
            BasketViewModel basketProduct = basket.Find(p => p.Id == id);
            if (basketProduct == null)
            {
                basket.Add(new BasketViewModel
                {
                    Id = id,
                    Count = 1
                });
            }
            else
            {
                basketProduct.Count += 1;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

        }
        private List<BasketViewModel> GetBasket()
        {
            List<BasketViewModel> basket;
            if(Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketViewModel>();
            }
            return basket;
        }

        public async Task<IActionResult> Basket()
        {
            List<BasketViewModel> basket = GetBasket();
            List<BasketItemViewModel> model = await GetBasketList(basket);
            return View(model);
        }

        private async Task<List<BasketItemViewModel>> GetBasketList(List<BasketViewModel> basket)
        {
            List<BasketItemViewModel> model = new List<BasketItemViewModel>();
            foreach (BasketViewModel item in basket)
            {
                Product dbProduct = await _context.Products
                                                  .Include(p => p.Images)
                                                  .FirstOrDefaultAsync(p=>p.Id==item.Id);
                BasketItemViewModel itemVM = GetBasketItem(item, dbProduct);
                model.Add(itemVM);
            }
            return model;
        }
        private BasketItemViewModel GetBasketItem(BasketViewModel item,Product dbProduct)
        {
            return new BasketItemViewModel
            {
                Id = item.Id,
                Name = dbProduct.Name,
                Count = item.Count,
                StockCount = dbProduct.Count,
                Image = dbProduct.Images.Where(i => i.IsMain).FirstOrDefault().Image,
                Price = dbProduct.Price,
                IsActive = dbProduct.IsDeleted
            };
        }
    }
}
