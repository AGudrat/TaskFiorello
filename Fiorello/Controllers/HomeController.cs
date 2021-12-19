using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeViewModel {
                Slider = await _context.Slider
                                .ToListAsync(),
                Categories = await _context.Categories
                                    .Where(c => c.IsDeleted == false)
                                    .ToListAsync(),
                Products = await _context.Products
                                    .Where(p => p.IsDeleted == false)
                                    .Include(p => p.Category)
                                    .Include(p => p.Images)
                                    .OrderByDescending(p => p.Id)
                                    .Take(8)
                                    .ToListAsync(),
                About = await _context.About.FirstOrDefaultAsync(),
                Features = await _context.Features.ToListAsync(),
                AboutVideo = await _context.AboutVideo.FirstOrDefaultAsync(),
                Experts = await _context.Experts.ToListAsync(),
                Blogs = await _context.Blogs.ToListAsync()
            };
            return View(homeVM);
        }
    }
}
