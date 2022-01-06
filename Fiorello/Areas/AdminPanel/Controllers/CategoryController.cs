using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class CategoryController : Controller
    {
        private AppDbContext _context { get; }
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.Where(c => c.IsDeleted == false).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = _context.Categories
                                   .Any(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim() && c.IsDeleted == false);
            if (isExist)
            {
                ModelState.AddModelError("Name", "*Bu kateqoriya artıq mövcuddur.");
                return View();
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) { return NotFound(); }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (!ModelState.IsValid) { return View(); }
            if (id != category.Id) { return BadRequest(); }

            Category dbCategory = await _context.Categories
                                                .Where(c => c.IsDeleted == false && c.Id == category.Id)
                                                .FirstOrDefaultAsync();

            if (dbCategory == null) { return NotFound(); }
            if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            bool isExist = _context.Categories
                                   .Any(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "*Bu kateqoriya artıq mövcuddur.");
                return View(category);
            }
            dbCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            return Json(new
            {
                action = "detail",
                Id = id
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            Category dbCategory = await _context.Categories
                                                .Where(c => c.IsDeleted == false && c.Id == id)
                                                .FirstOrDefaultAsync();
            if (dbCategory == null)
            {
                return NotFound();
            }
            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
