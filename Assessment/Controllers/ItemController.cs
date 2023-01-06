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
    public class ItemController : Controller
    {
        private inventory_dbEntitiestest db = new inventory_dbEntitiestest();

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.ItemData = db.items.OrderByDescending(x => x.entry_date).ToList();
            ViewBag.brand_id = new SelectList(db.brands, "id", "name");
            ViewBag.model_id = new SelectList(db.models, "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,brand_id,model_id,name,entry_date")] item item)
        {          
            if (ModelState.IsValid)
            {
                if (item.name != null)
                {
                    if (item.name != null)
                    {
                        var name = db.items.Where(x => x.name == item.name && x.brand.id == item.brand_id && x.model.id == item.model_id).ToList();
                        if (name.Count() == 0)
                        {
                            item.entry_date = DateTime.Now;
                            db.items.Add(item);
                            db.SaveChanges();
                            TempData["AlertMsg"] = "Create item successfully";
                            return RedirectToAction("Create");
                        }

                    }               
                }
            }
            TempData["AlertMsg"] = "Create Faield... Already have this item name of same brand and model";
            ViewBag.ItemData = db.items.OrderByDescending(x => x.entry_date).ToList();
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", item.brand_id);
            ViewBag.model_id = new SelectList(db.models, "id", "name", item.model_id);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", item.brand_id);
            ViewBag.model_id = new SelectList(db.models, "id", "name", item.model_id);
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,brand_id,model_id,name,entry_date")] item item)
        {
            if (ModelState.IsValid)
            {
                var name = db.items.Where(x => x.name == item.name && x.brand.id == item.brand_id && x.model.id == item.model_id).ToList();
                if (name.Count() == 0)
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            TempData["AlertMsg"] = "Already have this item name of same brand and model";
            ViewBag.brand_id = new SelectList(db.brands, "id", "name", item.brand_id);
            ViewBag.model_id = new SelectList(db.models, "id", "name", item.model_id);
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item item = db.items.Find(id);
            db.items.Remove(item);
            db.SaveChanges();
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
