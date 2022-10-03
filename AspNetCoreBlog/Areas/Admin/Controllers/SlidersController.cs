using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreBlog.Data;
using AspNetCoreBlog.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreBlog.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class SlidersController : Controller
    {
        private readonly DatabaseContext _context;

        public SlidersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Sliders
        public async Task<IActionResult> Index()
        {
              return View(await _context.sliders.ToListAsync());
        }

        // GET: Admin/Sliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider,IFormFile? Image)
        {
            if (Image is not null)
            {
                string directory = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + Image.FileName;
                using var stream = new FileStream(directory, FileMode.Create); // Buradaki using ifadesi stream isimli değişkenin işinin bittikten sonra bellekten atılmasını sağlar.
                await Image.CopyToAsync(stream);
                slider.Image = Image.FileName; // resmi asenkron olarak yükledik.
            }


            if (ModelState.IsValid)
            {
                
                await _context.AddAsync(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider, IFormFile? Image)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                    {
                        string directory = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + Image.FileName;
                        using var stream = new FileStream(directory, FileMode.Create); // Buradaki using ifadesi stream isimli değişkenin işinin bittikten sonra bellekten atılmasını sağlar.
                        await Image.CopyToAsync(stream);
                        slider.Image = Image.FileName; // resmi asenkron olarak yükledik.
                    }
                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sliders == null)
            {
                return Problem("Entity set 'DatabaseContext.sliders'  is null.");
            }
            var slider = await _context.sliders.FindAsync(id);
            if (slider != null)
            {
                _context.sliders.Remove(slider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
          return _context.sliders.Any(e => e.Id == id);
        }
    }
}
