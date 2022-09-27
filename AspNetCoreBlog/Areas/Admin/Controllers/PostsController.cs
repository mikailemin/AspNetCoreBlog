using AspNetCoreBlog.Data;
using AspNetCoreBlog.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly DatabaseContext _context; // Burada DatabaseContext ikendimiz new lemek yerine 

        public PostsController(DatabaseContext context) // Dependency Injection : Bağımlılık enjeksiyonu
        {
            _context = context;
        }

        // GET: PostsController
        public async Task<ActionResult> Index()
        {
            var model = await _context.Posts.ToListAsync();
            return View(model);
        }

        // GET: PostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post posts, IFormFile? Image)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                    {
                        string directory = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + Image.FileName;
                        using var stream = new FileStream(directory, FileMode.Create); // Buradaki using ifadesi stream isimli değişkenin işinin bittikten sonra bellekten atılmasını sağlar.
                        await Image.CopyToAsync(stream);
                        posts.Image = Image.FileName; // resmi asenkron olarak yükledik.
                    }
                    await _context.Posts.AddAsync(posts);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(posts);
        }

        // GET: PostsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var kayit = await _context.Posts.FindAsync(id);
            return View(kayit);
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, Post post,IFormFile? Image)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                    {
                        string directory = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + Image.FileName;
                        using var stream = new FileStream(directory, FileMode.Create); // Buradaki using ifadesi stream isimli değişkenin işinin bittikten sonra bellekten atılmasını sağlar.
                        await Image.CopyToAsync(stream);
                        post.Image = Image.FileName; // resmi asenkron olarak yükledik.
                    }
                    _context.Posts.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
       
            return View(post);
        }

        // GET: PostsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var kayit = await _context.Posts.FindAsync(id);
            return View(kayit);
        }

        // POST: PostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Post post)
        {
            try
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }
    }
}
