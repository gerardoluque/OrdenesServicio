using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OrdenesServicio.Negocio;
using ZOE.OS.Modelo.Model.Complex;
using Infragistics.Web.Mvc;
using ZOE.OrdenesServicio.Model.OrdenServicio;
using ZOE.OS.Modelo;
using System.Data;
using System.Text;
using System.Data.Entity.Infrastructure;
using ZOE.OrdenesServicio.Modelo;
using OrdenesServicio.Filters;

namespace ZOE.OrdenesServicio.Controllers
{
    [Authorize]
    public class AsesorSeguimientoController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /AsesorSeguimiento/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AsesorSeguimiento/Details/5
        [GridDataSourceAction()]
        public ActionResult ListaServicios()
        {
            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoPorAsesor(usuario.AsesorId.Value);

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosSinAtender()
        {
            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoSinAtender();

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosPropias()
        {
            //throw new Exception("Prueba de manejo de error");

            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoPropias(usuario.AsesorId.Value);

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosSinProyecto()
        {
            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoSinProyecto();

            return View(listaOS);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosTerminados()
        {
            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoPropiasTerminadas(usuario.AsesorId.Value);

            return View(listaOS);
        }


        // Registrar actividades a una orden de servicio
        // POST: /AsesorSeguimiento/VerSeguimiento 
        [HttpPost]
        public ActionResult VerSeguimiento(string btnSubmit, FormCollection collection)
        {
            string ticketParam = collection.Get("ticketid");

            if (!string.IsNullOrEmpty(ticketParam))
            {
                long ticket = Convert.ToInt64(ticketParam);
                Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                int asesor = usuario.AsesorId.Value;

                OrdenServicioBC.CambiarStatus(ticket, string.Empty, OrdenServicioStatus.EnSeguimiento, usuario.UsuarioId);

                switch (btnSubmit)
                {
                    case "Editar":
                        return RedirectToAction("EditarOrden", "AsesorSeguimiento", new { ticketId = ticket, asesorId = asesor });
                    case "Seguimiento":
                        return RedirectToAction("RegistrarActividad", "AsesorSeguimiento", new { ticketId = ticket, asesorId = asesor });
                    case "Reasignar":
                        return RedirectToAction("Reasignar", "OrdenServicio", new { ticketId = ticket, asesorId = asesor });
                    default:
                        break;
                }
            }
            return View();
        }

        public ActionResult RegistrarActividad(long ticketId, int asesorId)
        {
            var ordenServicio = OrdenServicioBC.Obtener(ticketId);

            AgregarModificarDetalleViewModel detalleMV = new AgregarModificarDetalleViewModel();
            detalleMV.OrdenServicio = ordenServicio;
            detalleMV.Actividad = new OS.Modelo.OSDetalle();
            detalleMV.Actividad.Ticket = ticketId;
            detalleMV.Actividad.AsesorId = asesorId;
            detalleMV.Actividad.StatusId = 1;
            detalleMV.Actividad.FechaRegistro = System.DateTime.Now;
            detalleMV.Actividad.FechaCerrado = System.DateTime.Now;
            detalleMV.Actividad.FechaComp = System.DateTime.Now;
            detalleMV.Actividad.AreaRespId = OrdenServicioBC.ObtenerAreaAsesor(asesorId);

            ViewBag.TicketId = ticketId;
            ViewBag.AsesorId = asesorId;
            ViewBag.TotalActividades = OrdenServicioBC.ObtenerTotalActividades(ticketId);
            ViewBag.TotalComentarios = OrdenServicioBC.ObtenerTotalComentarios(ticketId);
            ViewBag.ContactoId = new SelectList(OrdenServicioBC.ObtenerContactosPorTicket(ticketId), "ContactoId", "NombreCompleto");
            ViewBag.StatusId = new SelectList(db.OSDetalleStatus, "OSDetalleSTId", "OSDetalleSTDescr");
            ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "ServicioDescr");
            ViewBag.TipoServicioId = new SelectList(db.TiposServicio, "TipoServicioId", "TipoServicioDescr");
            ViewBag.ViaComunicacionId = new SelectList(db.ViasComunicacion, "ViaComId", "ViaComDescr");
            ViewBag.ProyectoId = new SelectList(OrdenServicioBC.ObtenerProyectosPorTicket(ticketId), "ProyectoId", "Descr");
            
            return View(detalleMV);
        }


        [HttpPost]
        public ActionResult RegistrarActividad(AgregarModificarDetalleViewModel model)
        {                        
            //Crear o actualizar la actividad
            if (ModelState.IsValid)
            {
                Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                OrdenServicioBC.CrearActividad(model.Actividad, usuario.UsuarioId);
            }

            return RedirectToAction("RegistrarActividad", "AsesorSeguimiento", new { ticketId = model.Actividad.Ticket, asesorId = model.Actividad.AsesorId });

        }

        [GridDataSourceAction()]
        public ActionResult ObtenerActividades(int id)
        {
            var listaActividades = OrdenServicioBC.ObtenerActividades(id);

            return View(listaActividades);
        }

        [OutputCache(Duration = 0, VaryByParam = "None")]
        public ActionResult ObtenerActividadesBusqueda(int id)
        {
            var listaActividades = OrdenServicioBC.ObtenerActividades(id).ToList();

            return Json(listaActividades, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0, VaryByParam = "None")]
        public ActionResult ObtenerActividad(long ticketId, long actividadId)
        {
            var actividad = OrdenServicioBC.ObtenerActividad(ticketId, actividadId);

            return Json(actividad, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CambiarStatus(CambiarStatusViewModel cambioStatusMV)
        {
            bool cambioValido;
            short statusIdActual;

            statusIdActual = db.DetallesOrdenServicio.Where(a => a.Ticket == cambioStatusMV.Ticket && a.DetalleId == cambioStatusMV.DetalleId).FirstOrDefault().StatusId;

            cambioStatusMV.StatusActualDescr = db.OSDetalleStatus.Find(statusIdActual).OSDetalleSTDescr;
            cambioValido = OrdenServicioBC.VerificarCambioStatusActividadValido(cambioStatusMV.Ticket, cambioStatusMV.DetalleId, cambioStatusMV.StatusId);

            if (cambioValido)
            {
                Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                OrdenServicioBC.CambiarStatusActividad(cambioStatusMV.Ticket, cambioStatusMV.DetalleId, cambioStatusMV.Observaciones, cambioStatusMV.StatusId, statusIdActual, usuario.UsuarioId);
            }

            cambioStatusMV.StatusCambioDescr = db.OSDetalleStatus.Find(cambioStatusMV.StatusId).OSDetalleSTDescr;
            cambioStatusMV.success = cambioValido;

            return Json(new { cambioStatusMV, success = cambioValido }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CambiarStatusOrdenServicio(CambiarStatusViewModel cambioStatusMV)
        {
            OrdenServicioValidacionInfo validacionCambioStatus = new OrdenServicioValidacionInfo();
            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            if (OrdenServicioBC.VerificarEsResponsableOrdenServicio(cambioStatusMV.Ticket, usuario.UsuarioId))
            {
                validacionCambioStatus = OrdenServicioBC.CambiarStatus(cambioStatusMV.Ticket, cambioStatusMV.Observaciones, (OrdenServicioStatus)cambioStatusMV.StatusId, usuario.UsuarioId);

                //Si se cambia la os a terminada notificar al contacto
                if (validacionCambioStatus.Valido && cambioStatusMV.StatusId == (short)OrdenServicioStatus.Terminada && cambioStatusMV.NotificarPorCorreoContacto)
                {
                    try
                    {
                        Negocio.OrdenServicioBC.EnviarCorreoAsesorTermino(cambioStatusMV.Ticket);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                validacionCambioStatus.Valido = false;
                validacionCambioStatus.Mensaje = "Cambio de status Cancelado, no es el responsable de la orden de servicio";
            }

            cambioStatusMV.StatusCambioDescr = db.OSStatus.Find(cambioStatusMV.StatusId).Descr;
            cambioStatusMV.Mensaje = validacionCambioStatus.Mensaje;
            cambioStatusMV.success = validacionCambioStatus.Valido;

            return Json(new { cambioStatusMV, success = validacionCambioStatus.Valido }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CambiarMinutos(OrdenServicioCambioMinutosViewModel cambioMinutos)
        {
            bool cambioValido = false;

            if (ModelState.IsValid)
            {
                cambioValido = OrdenServicioBC.CambiarMinutos(cambioMinutos.TicketId, cambioMinutos.Minutos);
            }

            return Json(new { Minutos = cambioMinutos.Minutos, success = cambioValido }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CambiarProyecto(OrdenServicioCambioProyectoViewModel cambioProyecto)
        {
            bool cambioValido = false;
            string descrProyecto = string.Empty;

            if (ModelState.IsValid)
            {
                cambioValido = OrdenServicioBC.CambiarProyecto(cambioProyecto.TicketId, cambioProyecto.ProyectoId);
                if (cambioValido)
                {
                    Proyecto proy = db.Proyectos.Find(cambioProyecto.ProyectoId);
                    descrProyecto = string.Format("{0} ({1})", proy.Descr, proy.TipoProyecto.TipoProyectoDescr);
                }
            }

            return Json(new { ProyectoDescr = descrProyecto, success = cambioValido }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AgregarComentario(OrdenServicioComentarioViewModel comentarioNuevo)
        {
            bool exito = false;

            if (ModelState.IsValid)
            {
                Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                exito = OrdenServicioBC.AgregarComentario(comentarioNuevo.TicketId, comentarioNuevo.Comentario, usuario.UsuarioId);                
            }
            return Json(new { success = exito }, JsonRequestBehavior.AllowGet);
        }

        [GridDataSourceAction()]
        public ActionResult ObtenerComentarios(int id)
        {
            var listaComentarios = OrdenServicioBC.ObtenerComentarios(id);

            return View(listaComentarios);
        }

        [GridDataSourceAction()]
        public ActionResult ObtenerHistoria(int id)
        {
            var listaHistoria = OrdenServicioBC.ObtenerHistoria(id);

            return View(listaHistoria);
        }
    }
}
