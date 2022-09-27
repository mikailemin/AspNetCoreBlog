using AspNetCoreBlog.Data;
using AspNetCoreBlog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetCoreBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kullanici = await _context.Users.FirstOrDefaultAsync(u=>u.IsAdmin && u.IsActive && u.Username==model.Username && u.Password==model.Password);
                    if (kullanici is null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız! Kullanıcı Adı veya Şifreniz yanlış");
                    }
                    else
                    {
                        var kullaniciHaklari = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, model.Username),
                        };
                        var kullaniciKimligi = new ClaimsIdentity(kullaniciHaklari,"Login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(kullaniciKimligi);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Index", "Default");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata oluştu!!");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");

        }
    }
}
