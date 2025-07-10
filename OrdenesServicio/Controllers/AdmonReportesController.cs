using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Controllers
{
    public class AdmonReportesController : BaseController
    {
        //
        // GET: /AdmonReporte/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClienteIndex()
        {
            int contactoId = Convert.ToInt32(Session[0]);
            Contacto usuario;

            using (OSContext db = new OSContext())
            {
                usuario = db.Contactos.Where(u => u.ContactoId == contactoId).First();
                ViewBag.ClienteId = usuario.ClienteId;
            }

            return View();
        }
    }
}
