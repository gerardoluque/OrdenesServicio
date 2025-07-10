using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Negocio;

namespace ZOE.OrdenesServicio.Controllers
{
    //[Authorize]
    public class ProyectoController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /Proyecto/

        public ViewResult Index()
        {
            var proyectos = db.Proyectos.Include(p => p.Cliente).OrderBy(p => p.Cliente.Nombre);
            var tipoProy = db.Proyectos.Include(t => t.TipoProyecto);
            return View(proyectos.ToList());
        }

        //
        // GET: /Proyecto/Details/5

        public ViewResult Details(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            return View(proyecto);
        }

        //
        // GET: /Poliza/Create

        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(t => t.Nombre).ToList(), "ClienteId", "Nombre");
            ViewBag.TipoProyectoId = new SelectList(db.TiposProyecto.OrderBy(t => t.TipoProyectoDescr).ToList(), "TipoProyectoId", "TipoProyectoDescr");
            return View();
        } 

        //
        // POST: /Poliza/Create

        [HttpPost]
        public ActionResult Create(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                proyecto.UsuarioIdRegistro = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name).UsuarioId;
                db.Proyectos.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.TipoProyectoId = new SelectList(db.TiposProyecto.OrderBy(t => t.TipoProyectoDescr).ToList(), "TipoProyectoId", "TipoProyectoDescr", proyecto.TipoProyectoId);
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(t => t.Nombre).ToList(), "ClienteId", "Nombre", proyecto.ClienteId);
            return View(proyecto);
        }
        
        //
        // GET: /Poliza/Edit/5
 
        public ActionResult Edit(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            ViewBag.TipoProyectoId = new SelectList(db.TiposProyecto.OrderBy(t => t.TipoProyectoDescr).ToList(), "TipoProyectoId", "TipoProyectoDescr", proyecto.TipoProyectoId);
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(t => t.Nombre).ToList(), "ClienteId", "Nombre", proyecto.ClienteId);
            return View(proyecto);
        }

        //
        // POST: /Poliza/Edit/5

        [HttpPost]
        public ActionResult Edit(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoProyectoId = new SelectList(db.TiposProyecto.OrderBy(t=>t.TipoProyectoDescr).ToList(), "TipoProyectoId", "TipoProyectoDescr", proyecto.TipoProyectoId);
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(t => t.Nombre).ToList(), "ClienteId", "Nombre", proyecto.ClienteId);
            return View(proyecto);
        }

        public ActionResult Abonar(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.Cliente = proyecto.Cliente.Nombre;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;            
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(id);
            return View();
        }

        [HttpPost]
        public ActionResult Abonar(ProyectoAbono abono)
        {
            if (ModelState.IsValid)
            {
                abono.UsuarioId = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name).UsuarioId;

                ProyectoBC.Abonar(abono);

                return RedirectToAction("Index");
            }

            Proyecto proyecto = db.Proyectos.Find(abono.ProyectoId);
            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(abono.ProyectoId);

            return View();
        }

        public ActionResult VerAbonos(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);

            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.Cliente = proyecto.Cliente.Nombre;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(id);

            List<ProyectoAbono> abonos = ProyectoBC.ObtenerAbonos(id);

            return View(abonos);
        }

        public ActionResult EditarAbono(int id, int proyectoId)
        {
            Proyecto proyecto = db.Proyectos.Find(proyectoId);

            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(proyectoId);

            ProyectoAbono abono = db.ProyectoAbonos.Find(id);

            return View(abono);
        }

        [HttpPost]
        public ActionResult EditarAbono(ProyectoAbono abono)
        {
            if (ModelState.IsValid)
            {
                ProyectoBC.ActualizarAbono(abono);

                return RedirectToAction("Index");
            }

            Proyecto proyecto = db.Proyectos.Find(abono.ProyectoId);
            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(abono.ProyectoId);

            return View(abono);
        }

        public ActionResult VerActividades(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);

            ViewBag.ProyectoId = proyecto.ProyectoId;
            ViewBag.Cliente = proyecto.Cliente.Nombre;
            ViewBag.ProyectoDescr = proyecto.Descr;
            ViewBag.TipoProyectoDescr = proyecto.TipoProyecto.TipoProyectoDescr;
            ViewBag.Saldo = ProyectoBC.ObtenerSaldo(id);

            List<ProyectoAbono> abonos = ProyectoBC.ObtenerAbonos(id);

            return View(abonos);
        }

        //
        // GET: /Poliza/Delete/5
 
        public ActionResult Delete(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            return View(proyecto);
        }

        //
        // POST: /Poliza/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Proyecto proyecto = db.Proyectos.Find(id);
            db.Proyectos.Remove(proyecto);
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