using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Controllers
{ 
    public class AreaController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /Area/

        public ViewResult Index()
        {
            return View(db.Areas.ToList());
        }

        //
        // GET: /Area/Details/5

        public ViewResult Details(short id)
        {
            Area area = db.Areas.Find(id);
            return View(area);
        }

        //
        // GET: /Area/Create

        public ActionResult Create()
        {
            Area area = new Area();
            return View(area);
        } 

        //
        // POST: /Area/Create

        [HttpPost]
        public ActionResult Create(Area area)
        {
            if (ModelState.IsValid)
            {
                db.Areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(area);
        }
        
        //
        // GET: /Area/Edit/5
 
        public ActionResult Edit(short id)
        {
            Area area = db.Areas.Find(id);
            return View(area);
        }

        //
        // POST: /Area/Edit/5

        [HttpPost]
        public ActionResult Edit(Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        //
        // GET: /Area/Delete/5
 
        public ActionResult Delete(short id)
        {
            Area area = db.Areas.Find(id);
            return View(area);
        }

        //
        // POST: /Area/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            Area area = db.Areas.Find(id);
            db.Areas.Remove(area);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}