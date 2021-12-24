using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private AppDbContext _context {get;} 
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            return Json(new { 
                action = "Update",
                Id=id
            });
        }

        public IActionResult Detail(int id)
        {
            return Json(new
            {
                action = "detail",
                Id = id
            });
        }
        public IActionResult Delete(int id)
        {
            return Json(new
            {
                action = "delete",
                Id = id
            });
        }
    }
}
