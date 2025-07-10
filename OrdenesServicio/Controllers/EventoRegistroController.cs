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
    [AllowAnonymous]
    public class EventoRegistroController : Controller
    {
        private OSContext db = new OSContext();

        //
        // GET: /EventoRegistro/
        [AllowAnonymous]
        public ViewResult Index()
        {
            var eventoasistente = db.EventoAsistente.Include(e => e.Evento);
            return View(eventoasistente.ToList());
        }

        //
        // GET: /EventoRegistro/Details/5

        public ViewResult Details(short id)
        {
            EventoAsistente eventoasistente = db.EventoAsistente.Find(id);
            return View(eventoasistente);
        }

        //
        // GET: /EventoRegistro/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            //ViewBag.EventoId = new SelectList(db.Evento, "EventoId", "EventoDescr");
            ViewBag.EventoId = 1;
            return View();
        } 

        //
        // POST: /EventoRegistro/Create

        [HttpPost]
        public ActionResult Create(EventoAsistente eventoasistente)
        {
            if (ModelState.IsValid)
            {
                eventoasistente.EventoId = 1;
                db.EventoAsistente.Add(eventoasistente);
                db.SaveChanges();
                ZOE.OrdenesServicio.Negocio.OrdenServicioBC.EnviarCorreoRegistroEvento(eventoasistente.Email, eventoasistente.Nombre, "Taller Cumplimiento IVA IEPS, para las Certificaciones A, AA, AAA y NEEC L III");
                return View("Index", eventoasistente);  
            }

            //ViewBag.EventoId = new SelectList(db.Evento, "EventoId", "EventoDescr", eventoasistente.EventoId);
            ViewBag.EventoId = 1;
            return View(eventoasistente);
        }
        
        //
        // GET: /EventoRegistro/Edit/5
 
        public ActionResult Edit(short id)
        {
            EventoAsistente eventoasistente = db.EventoAsistente.Find(id);
            ViewBag.EventoId = new SelectList(db.Evento, "EventoId", "EventoDescr", eventoasistente.EventoId);
            return View(eventoasistente);
        }

        //
        // POST: /EventoRegistro/Edit/5

        [HttpPost]
        public ActionResult Edit(EventoAsistente eventoasistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventoasistente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventoId = new SelectList(db.Evento, "EventoId", "EventoDescr", eventoasistente.EventoId);
            return View(eventoasistente);
        }

        //
        // GET: /EventoRegistro/Delete/5
 
        public ActionResult Delete(short id)
        {
            EventoAsistente eventoasistente = db.EventoAsistente.Find(id);
            return View(eventoasistente);
        }

        //
        // POST: /EventoRegistro/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            EventoAsistente eventoasistente = db.EventoAsistente.Find(id);
            db.EventoAsistente.Remove(eventoasistente);
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