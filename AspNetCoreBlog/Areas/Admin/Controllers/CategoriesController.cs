using AspNetCoreBlog.Data;
using AspNetCoreBlog.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCoreBlog.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class CategoriesController : Controller
    {
        // private readonly DatabaseContext _databaseContext;
        DatabaseContext context = new DatabaseContext();


        // GET: CategoriesController
        public ActionResult Index()
        {

            var model = context.Categories.ToList();

            return View(model);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(category);
        }

        // GET: CategoriesController/Edit/5 
        public ActionResult Edit(int id)
        {
            var kategori = context.Categories.FirstOrDefault(c => c.Id == id);

            return View(kategori);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                context.Update(category);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu");
            }
            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            var kategori = context.Categories.Find(id);
            return View(kategori);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                // 1.Silme yöntemi
                //context.Categories.Remove(category);
                //context.SaveChanges();
                // 2. Silme Yönemi
                context.Entry(category).State = EntityState.Deleted;
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
