using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Seguridad;

namespace ZOE.OrdenesServicio.Controllers
{ 
    public class AsesorController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /Asesor/

        public ViewResult Index()
        {
            var asesores = db.Asesores.OrderBy(o=>o.Nombre).ThenBy(o=>o.Paterno).ThenBy(o=>o.Materno).Include(a => a.Area);
            return View(asesores.ToList());
        }

        //
        // GET: /Asesor/Details/5

        public ViewResult Details(int id)
        {
            Asesor asesor = db.Asesores.Find(id);
            return View(asesor);
        }

        //
        // GET: /Asesor/Create

        public ActionResult Create()
        {
            ViewBag.AreaId = new SelectList(db.Areas.Where(a=>a.TipoArea==(short)TiposArea.Zoe).ToList(), "AreaId", "AreaDescr");
            return View();
        } 

        //
        // POST: /Asesor/Create

        [HttpPost]
        public ActionResult Create(Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                //db.Asesores.Add(asesor);
                //db.SaveChanges();

                ZOE.OrdenesServicio.Negocio.Seguridad.CrearAsesor(asesor);

                return RedirectToAction("Index");  
            }

            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Zoe).OrderBy(o => o.AreaDescr).ToList(), "AreaId", "AreaDescr", asesor.AreaId);
            return View(asesor);
        }
        
        //
        // GET: /Asesor/Edit/5
 
        public ActionResult Edit(int id)
        {
            Asesor asesor = db.Asesores.Find(id);
            asesor.UserName = "temporal";
            asesor.Password = "temporal";
            asesor.ConfirmPassword = "temporal";
            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Zoe).OrderBy(o=>o.AreaDescr).ToList(), "AreaId", "AreaDescr", asesor.AreaId);
            return View(asesor);
        }

        //
        // POST: /Asesor/Edit/5

        [HttpPost]
        public ActionResult Edit(Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asesor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Zoe).OrderBy(o => o.AreaDescr).ToList(), "AreaId", "AreaDescr", asesor.AreaId);
            return View(asesor);
        }

        //
        // GET: /Asesor/Delete/5
 
        public ActionResult Delete(int id)
        {
            Asesor asesor = db.Asesores.Find(id);
            return View(asesor);
        }

        //
        // POST: /Asesor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Asesor asesor = db.Asesores.Find(id);
            db.Asesores.Remove(asesor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CambiarContrasena(int id)
        {
            Asesor asesor = db.Asesores.Find(id);

            if (db.Usuarios.Where(u => u.AsesorId == id).Any())
            {
                asesor.UserName = db.Usuarios.Where(u => u.AsesorId == id).FirstOrDefault().UserName;
            }
            return View(asesor);
        }

        [HttpPost]
        public ActionResult CambiarContrasena(Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                UsuarioWeb.ResetPassword(asesor.UserName, asesor.Password);
                return RedirectToAction("Index");
            }

            if (db.Usuarios.Where(u => u.AsesorId == asesor.AsesorId).Any())
            {
                asesor.UserName = db.Usuarios.Where(u => u.AsesorId == asesor.AsesorId).FirstOrDefault().UserName;
            }
            return View(asesor);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}