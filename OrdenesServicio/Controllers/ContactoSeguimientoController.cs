using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infragistics.Web.Mvc;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Negocio;
using ZOE.OrdenesServicio.Model.OrdenServicio;
using ZOE.OrdenesServicio.Modelo;

namespace ZOE.OrdenesServicio.Controllers
{
    public class ContactoSeguimientoController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /ContactoSeguimiento/

        public ActionResult Index()
        {
            return View();
        }

        [GridDataSourceAction()]
        public ActionResult ListaServicios()
        {
            //Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            int contactoId = Convert.ToInt32(Session[0]);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoPorContacto(contactoId);

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosPropias()
        {
            //Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            int contactoId = Convert.ToInt32(Session[0]);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoPropiasPorContacto(contactoId);

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosSinAtender()
        {
            //Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            int contactoId = Convert.ToInt32(Session[0]);
            Contacto contacto = db.Contactos.Find(contactoId);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoSinAtenderPorCliente(contacto.ClienteId);

            return View(listaOS);
        }

        // Consultar actividades de una orden de servicio
        // POST: /ContactoSeguimiento/VerSeguimiento 
        [HttpPost]
        public ActionResult VerSeguimiento(string btnSubmit, FormCollection collection)
        {
            string ticketParam = collection.Get("ticketid");

            if (!string.IsNullOrEmpty(ticketParam))
            {
                long ticket = Convert.ToInt64(ticketParam);
                
                if (btnSubmit == Resources.SOResource.Seguimiento)
                    return RedirectToAction("ConsultarActividad", "ContactoSeguimiento", new { ticketId = ticket });
            }
            return View();
        }

        public ActionResult ConsultarActividad(long ticketId)
        {
            var ordenServicio = OrdenServicioBC.Obtener(ticketId);

            AgregarModificarDetalleViewModel detalleMV = new AgregarModificarDetalleViewModel();
            detalleMV.OrdenServicio = ordenServicio;
            detalleMV.Actividad = new OS.Modelo.OSDetalle();
            detalleMV.Actividad.Ticket = ticketId;
            //detalleMV.Actividad.AsesorId = asesorId;
            detalleMV.Actividad.StatusId = 1;
            detalleMV.Actividad.FechaRegistro = System.DateTime.Now;
            //detalleMV.Actividad.AreaRespId = OrdenServicioBC.ObtenerAreaAsesor(asesorId);

            ViewBag.TicketId = ticketId;
            //ViewBag.AsesorId = asesorId;
            ViewBag.TotalActividades = OrdenServicioBC.ObtenerTotalActividades(ticketId);
            ViewBag.TotalComentarios = OrdenServicioBC.ObtenerTotalComentarios(ticketId);
            //ViewBag.ContactoId = new SelectList(OrdenServicioBC.ObtenerContactosPorTicket(ticketId), "ContactoId", "NombreCompleto");
            //ViewBag.StatusId = new SelectList(db.OSDetalleStatus, "OSDetalleSTId", "OSDetalleSTDescr");
            //ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "ServicioDescr");
            //ViewBag.TipoServicioId = new SelectList(db.TiposServicio, "TipoServicioId", "TipoServicioDescr");
            //ViewBag.ViaComunicacionId = new SelectList(db.ViasComunicacion, "ViaComId", "ViaComDescr");
            //ViewBag.ProyectoId = new SelectList(OrdenServicioBC.ObtenerProyectosPorTicket(ticketId), "ProyectoId", "Descr");

            return View(detalleMV);
        }

        [HttpPost]
        public ActionResult CambiarStatusOrdenServicio(CambiarStatusViewModel cambioStatusMV)
        {
            //Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            int contactoId = Convert.ToInt32(Session[0]);
            Usuario usuario = db.Usuarios.Where(u => u.ContactoId == contactoId).First();

            OrdenServicioValidacionInfo validacionCambioStatus = OrdenServicioBC.CambiarStatus(cambioStatusMV.Ticket, cambioStatusMV.Observaciones, (OrdenServicioStatus)cambioStatusMV.StatusId, usuario.UsuarioId);

            if (validacionCambioStatus.Valido)
            {
                try
                {
                    OrdenServicioBC.EnviarCorreoContactoCerroTicket(cambioStatusMV.Ticket, contactoId);
                }
                catch (Exception)
                {                    
                }
            }

            cambioStatusMV.StatusCambioDescr = db.OSStatus.Find(cambioStatusMV.StatusId).Descr;
            cambioStatusMV.Mensaje = validacionCambioStatus.Mensaje;
            cambioStatusMV.success = validacionCambioStatus.Valido;

            return Json(new { cambioStatusMV, success = validacionCambioStatus.Valido }, JsonRequestBehavior.AllowGet);
        }
    }
}
