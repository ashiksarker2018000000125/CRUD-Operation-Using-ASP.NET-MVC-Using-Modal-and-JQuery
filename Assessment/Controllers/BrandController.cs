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
    public class BrandController : Controller
    {
        private inventory_dbEntitiestest db = new inventory_dbEntitiestest();
        brand bnd = new brand();

        // GET: Brand/Create
        public ActionResult Create()
        {
            ViewBag.BrandData = db.brands.OrderByDescending(x => x.entry_date).ToList();
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,name,entry_date")] brand brand)
        {
            if (ModelState.IsValid)
            {              
                if (brand.name != null)
                {
                    var name = db.brands.Where(x => x.name == brand.name).ToList();
                    if (name.Count()==0)
                    {
                        brand.entry_date = DateTime.Now;
                        db.brands.Add(brand);
                        db.SaveChanges();
                        TempData["AlertMsg"] = "Create brand successfully";
                        return RedirectToAction("Create");
                    }
                }
            }
            TempData["AlertMsg"] = "Create Faield... Already have this brand Name";
            ViewBag.BrandData = db.brands.OrderByDescending(x => x.entry_date).ToList();
            return View(brand);
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brand brand = db.brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,entry_date")] brand brand)
        {
            if (ModelState.IsValid)
            {
                var name = db.brands.Where(x => x.name == brand.name).ToList();
                if (name.Count() == 0)
                {
                    db.Entry(brand).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            TempData["AlertMsg"] = "Already have this brand name Duplicate brand name will not allow";
            return View(brand);
        }

        // GET: Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brand brand = db.brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            brand brand = db.brands.Find(id);
            db.brands.Remove(brand);
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
