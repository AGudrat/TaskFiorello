using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities;
using Fiorello.Utilities.File;
using Fiorello.ViewModel.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin")]

    public class SliderController : Controller
    {

        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }
        private string _errorMesage;
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
        public async Task<IActionResult> Create(MultipleSliderViewModel sliderViewModel)
        {
            #region SingleUpload
            //if (ModelState["Photo"].ValidationState==ModelValidationState.Invalid)
            //{
            //    return View();
            //}
            //if (!slider.Photo.CheckFileType("image/"))
            //{
            //    ModelState.AddModelError("Photo", "Fayl 'image' tipində olmalıdır.");
            //    return View();
            //}
            //if (!slider.Photo.CheckFileSize(200))
            //{
            //    ModelState.AddModelError("Photo", "Şəklin ölçüsü 200kb dan çoxdur.");
            //    return View();
            //}
            //string fileName =await slider.Photo.SaveFileAsync(_env.WebRootPath,"img");
            //slider.Image = fileName;
            //await _context.Slider.AddAsync(slider);
            //await _context.SaveChangesAsync();
            #endregion
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid)
            {
                return View();
            }
            if (!CheckImageValid(sliderViewModel.Photos))
            {
                ModelState.AddModelError("Photos", _errorMesage);
                return View();
            }
            foreach (var photo in sliderViewModel.Photos)
            {
                string fileName = await photo.SaveFileAsync(_env.WebRootPath, "img");
                Slider slider = new Slider
                {
                    Image = fileName
                };
                await _context.Slider.AddAsync(slider);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CheckImageValid(List<IFormFile> photos)
        {
            foreach (var photo in photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    _errorMesage= $"{photo.FileName} 'image' tipində olmalıdır.";
                    return false;
                }
                if (!photo.CheckFileSize(200))
                {
                    _errorMesage= $"{photo.FileName} ölçüsü 200kb dan çoxdur.";
                    return false;
                }
            }
            return true;
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


        public async Task<IActionResult> Update(int id)
        {
            Slider slider = await _context.Slider.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Slider slider)
        {
            if (id != slider.Id) return BadRequest(); 
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) { return View(nameof(Index)); }
            Slider dbSlider = await _context.Slider.FindAsync(id);
            if (dbSlider == null) return NotFound();
            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Fayl 'image' tipində olmalıdır.");
                return View(dbSlider);
            }
            if (!slider.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Şəklin ölçüsü 200kb dan çoxdur.");
                return View(dbSlider);
            }
            Helper.RemoveFile(_env.WebRootPath, "img", dbSlider.Image);


            string updatedName = await slider.Photo.SaveFileAsync(_env.WebRootPath, "img");
            dbSlider.Image = updatedName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
