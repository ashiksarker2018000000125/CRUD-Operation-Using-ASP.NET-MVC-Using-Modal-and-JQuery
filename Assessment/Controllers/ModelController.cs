using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assessment.Models;

namespace Assessment.Controllers
{
    public class ModelController : Controller
    {
        private inventory_dbEntitiestest db = new inventory_dbEntitiestest();

        // GET: Model/Create
        public ActionResult Create()
        {
            List<brand> list = db.brands.ToList();
            ViewBag.Brandlist = new SelectList(list).ToList();
            var models = db.models.Include(m => m.brand);
            ViewBag.ModelData = db.models.OrderByDescending(x => x.entry_date).ToList();
            ViewBag.brand_id = new SelectList(db.brands, "id", "name");
            return View();
        }

        // POST: Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,brand_id,name,entry_date")] model model)
        {
            if (ModelState.IsValid)
            {        
                if (model.name != null)
                {
                    var name = db.models.Where(x => x.name == model.name && x.brand.id == model.brand_id).ToList();
                    if (name.Count() == 0)
                    {
                        model.entry_date = DateTime.Now;
                        db.models.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("Create");
                    }
                    
                }

            }
            TempData["AlertMsg"] = "Create Faield... Already have this model name of same brand";
            ViewBag.ModelData = db.models.OrderByDescending(x => x.entry_date).ToList();
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", model.brand_id);
            return View(model);
        }

        // GET: Model/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model model = db.models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", model.brand_id);
            return View(model);
        }

        // POST: Model/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,brand_id,name,entry_date")] model model)
        {
            if (ModelState.IsValid)
            {
                var name = db.models.Where(x => x.name == model.name && x.brand.id == model.brand_id).ToList();
                if (name.Count() == 0)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            TempData["AlertMsg"] = "Already have this model name of same brand";
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", model.brand_id);
            return View(model);
        }

        // GET: Model/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model model = db.models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            model model = db.models.Find(id);
            db.models.Remove(model);
            db.SaveChanges();
            TempData["AlertMsg"] = "Create item successfully";
            return RedirectToAction("Create");
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
