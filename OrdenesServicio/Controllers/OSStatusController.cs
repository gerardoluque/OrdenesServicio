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
    public class OSStatusController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /OSStatus/

        public ViewResult Index()
        {
            return View(db.OSStatus.ToList());
        }

        //
        // GET: /OSStatus/Details/5

        public ViewResult Details(short id)
        {
            OSStatus osstatus = db.OSStatus.Find(id);
            return View(osstatus);
        }

        //
        // GET: /OSStatus/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /OSStatus/Create

        [HttpPost]
        public ActionResult Create(OSStatus osstatus)
        {
            if (ModelState.IsValid)
            {
                db.OSStatus.Add(osstatus);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(osstatus);
        }
        
        //
        // GET: /OSStatus/Edit/5
 
        public ActionResult Edit(short id)
        {
            OSStatus osstatus = db.OSStatus.Find(id);
            return View(osstatus);
        }

        //
        // POST: /OSStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(OSStatus osstatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(osstatus).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(osstatus);
        }

        //
        // GET: /OSStatus/Delete/5
 
        public ActionResult Delete(short id)
        {
            OSStatus osstatus = db.OSStatus.Find(id);
            return View(osstatus);
        }

        //
        // POST: /OSStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            OSStatus osstatus = db.OSStatus.Find(id);
            db.OSStatus.Remove(osstatus);
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