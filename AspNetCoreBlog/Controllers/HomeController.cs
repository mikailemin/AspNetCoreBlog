using AspNetCoreBlog.Data;
using AspNetCoreBlog.Entities;
using AspNetCoreBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AspNetCoreBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var model = new HomePageViewModel();
            model.Posts = await _context.Posts.Where(p => p.IsActive && p.IsHome).ToListAsync();
            model.Sliders = await _context.sliders.ToListAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("iletisim")]
        public IActionResult ContactUs()
        {
            return View(); 
        }
        public async Task<IActionResult> ContactUsAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Contacts.AddAsync(contact);
                    await _context.SaveChangesAsync();
                    TempData["Mesaj"] = "<div class='alert alert-success'>Mesajını iletildi. Teşekkürler</div>";
                    return RedirectToAction(nameof(ContactUs));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                    throw;
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}