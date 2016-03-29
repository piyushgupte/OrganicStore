using OrganicStore.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace OrganicStore.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(from p in db.Categories
                        join r in db.Categories on p.PrarentId equals r.CategoryId
                        select new CategoriesListViewModel
                        { CategoryId = p.CategoryId,
                            CategoryName = p.CategoryName,
                            CategoryDetails = p.CategoryDetails,
                            ParentCategory = r.CategoryName,
                            ParentCategoryId = p.PrarentId });
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            var CategoryDetails = new CategoriesListViewModel
            {
                CategoryName = category.CategoryName,
                CategoryDetails = category.CategoryDetails,
                ParentCategory = db.Categories.Where(p => p.CategoryId == category.PrarentId).Select(q => q.CategoryName).FirstOrDefault()
        };
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(CategoryDetails);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            var Categories = db.Categories.OrderBy(p => p.CategoryName).ToList().Select(rr => new SelectListItem { Value = rr.CategoryId.ToString(), Text = rr.CategoryName }).DefaultIfEmpty(new SelectListItem {Text="",Value="" });
            ViewBag.Categories = Categories;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,PrarentId,CategoryDetails")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(category.PrarentId.ToString()))
                    category.PrarentId = 0;

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = from p in db.Categories
                                               where p.CategoryId == id
                                               select new CategoriesListViewModel
                                               {
                                                   CategoryName=p.CategoryName,
                                                   CategoryId=p.CategoryId,
                                                   CategoryDetails = p.CategoryDetails,
                                                   ParentCategoryId=  p.PrarentId,
                                                   ParentCategory = db.Categories.Where(rr=> rr.CategoryId==p.PrarentId).Select(q=> q.CategoryName).FirstOrDefault()
                                                  

                                               };
            CategoriesListViewModel singleCategory = category.First();

            singleCategory.Categories = db.Categories.OrderBy(p => p.CategoryName).Select(q => new SelectListItem { Text = q.CategoryName, Value = q.CategoryId.ToString() });

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(singleCategory);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,ParentCategory,ParentCategoryId,CategoryDetails,Categories")] CategoriesListViewModel category)
        {
            var categoryToEdit = new Category
            {
                CategoryId = category.CategoryId,
                CategoryDetails = category.CategoryDetails,
                CategoryName = category.CategoryName,
                PrarentId = category.ParentCategoryId

            };
            
            if (ModelState.IsValid)
            {
                db.Entry(categoryToEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}