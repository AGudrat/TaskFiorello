using Fiorello.DAL;
using Fiorello.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketViewModel> basket = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);
            if (basket != null)
            {
                ViewBag.BasketItemCount = basket.Sum(p=>p.Count);
            }
            else
            {
                ViewBag.BasketItemCount = 0; 
            }
            var setting = _context.Settings
                                  .AsEnumerable()
                                  .ToDictionary(s => s.Key, s => s.Value);
            return  View(await Task.FromResult(setting));
        }
    }
}
