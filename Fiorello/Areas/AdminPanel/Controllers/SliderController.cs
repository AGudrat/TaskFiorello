using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {

        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }   
        public SliderController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Slider.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState==ModelValidationState.Invalid)
            {
                return View();
            }
            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Fayl 'image' tipində olmalıdır.");
                return View();
            }
            if (!slider.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Şəklin ölçüsü 200kb dan çoxdur.");
                return View();
            }
            string fileName =await slider.Photo.SaveFileAsync(_env.WebRootPath,"img");
            slider.Image = fileName;
            await _context.Slider.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider =await _context.Slider.FindAsync(id);
            if (slider == null) return NotFound();
            Helper.RemoveFile(_env.WebRootPath, "img", slider.Image);
            _context.Slider.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
