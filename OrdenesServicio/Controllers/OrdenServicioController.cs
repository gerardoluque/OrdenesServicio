using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Modelo;
using ZOE.OrdenesServicio.Negocio;
using ZOE.OrdenesServicio.Model.OrdenServicio;
using Infragistics.Web.Mvc;
using ZOE.OS.Modelo.Model.Complex;
using System.ComponentModel;

namespace ZOE.OrdenesServicio.Controllers
{
    //[Authorize]
    public class OrdenServicioController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /OrdenServicio/

        public ViewResult Index()
        {
            var ordenesservicio = db.OrdenesServicio.Include(o => o.Status).Include(o => o.Contacto).Include(o => o.Asesor).Include(o => o.AreaResponsable).Include(o => o.Proyecto);
            return View(ordenesservicio.ToList());
        }

        //
        // GET: /OrdenServicio/Details/5

        public ViewResult Details(long id)
        {
            OrdenServicio ordenservicio = db.OrdenesServicio.Find(id);
            return View(ordenservicio);
        }
        
        public ActionResult Create()
        {
            //Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            int contactoId = Convert.ToInt32(Session[0]);

            CrearOrdenServicioViewModel ordenServicioVM = new CrearOrdenServicioViewModel();
            ordenServicioVM.OrdenServicio = new OrdenServicio();
            
            ordenServicioVM.AsesorCreando = false;

            ordenServicioVM.ContacoIdSeleccionado = contactoId;
            ordenServicioVM.OrdenServicio.ContactoId = contactoId;
            int clienteId = db.Contactos.Find(contactoId).ClienteId;
            
            ordenServicioVM.ClienteIdSeleccionado = clienteId;

            ordenServicioVM.Asesores = new SelectList(db.Asesores, "AsesorId", "NombreCompleto");

            return View(ordenServicioVM);
        }

        public ActionResult CreaAsesor()
        {
            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            CrearOrdenServicioViewModel ordenServicioVM = new CrearOrdenServicioViewModel();
            ordenServicioVM.AsesorCreando = true;
            ordenServicioVM.NotificarContactoEmail = false;
            ordenServicioVM.OrdenServicio = new OrdenServicio();

            ordenServicioVM.OrdenServicio.AsesorId = usuario.AsesorId.Value;

            ordenServicioVM.Clientes = new SelectList(db.Clientes.OrderBy(o=>o.Nombre), "ClienteId", "Nombre");
            ordenServicioVM.Asesores = new SelectList(db.Asesores.OrderBy(o=>o.Nombre).ThenBy(o=>o.Paterno).ThenBy(o=>o.Materno), "AsesorId", "NombreCompleto", ordenServicioVM.OrdenServicio.AsesorId);
            ordenServicioVM.Prioridades = OrdenServicio.GetPrioridadesSelectList();

            return View("Create", ordenServicioVM);
        }

        //
        // POST: /OrdenServicio/Create

        [HttpPost]
        public ActionResult Create(CrearOrdenServicioViewModel crearOrdenServicioViewModel)
        {
            short indAsesorCreo = 0;
            Usuario usuario = null;

            if (ModelState.IsValid)
            {
                if (crearOrdenServicioViewModel.AsesorCreando)
                {
                    indAsesorCreo = 1;
                    usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                    crearOrdenServicioViewModel.OrdenServicio.ContactoId = crearOrdenServicioViewModel.ContacoIdSeleccionado;
                    crearOrdenServicioViewModel.OrdenServicio.ProyectoId = crearOrdenServicioViewModel.ProyectoIdSeleccionado;
                    crearOrdenServicioViewModel.OrdenServicio.Prioridad = crearOrdenServicioViewModel.ProyectoIdSeleccionado;
                }
                else
                {
                    usuario = Negocio.Seguridad.ObtenerUsuarioPorContactoId(crearOrdenServicioViewModel.OrdenServicio.ContactoId);
                }

                long ticketId = OrdenServicioBC.Crear(crearOrdenServicioViewModel.OrdenServicio, usuario.UsuarioId);

                string userState = "Envio de mensaje";
                Negocio.OrdenServicioBC.EnviarCorreoPorCreacionTicket(ticketId, usuario.UsuarioId, crearOrdenServicioViewModel, userState);

                return RedirectToAction("TicketReporte", new { id = ticketId, asesorCreo = indAsesorCreo });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Verifique los campos de captura");
            }

            crearOrdenServicioViewModel.Clientes = new SelectList(db.Clientes, "ClienteId", "Nombre");
            crearOrdenServicioViewModel.Asesores = new SelectList(db.Asesores, "AsesorId", "NombreCompleto", usuario.AsesorId);
            crearOrdenServicioViewModel.Prioridades = OrdenServicio.GetPrioridadesSelectList();

            return View(crearOrdenServicioViewModel);
        }

        internal void EnvioCorreoTerminado(object sender, AsyncCompletedEventArgs e)
        {
            return;
        }

        public ActionResult TicketReporte(long id, int asesorCreo)
        {
            OrdenServicio ordenservicio = OrdenServicioBC.Obtener(id);
            ViewBag.AsesorCreo = asesorCreo;
            return View(ordenservicio);
        }

        [GridDataSourceAction()]
        public ActionResult ListaServiciosSinAsesor()
        {
            var listaOS = OrdenServicioBC.ObtenerListaSeguimientoSinAsesor();

            return View(listaOS);
        }   
        
        public ActionResult Buscar()
        {
            OrdenServicioBuscarViewModel buscarVM = new OrdenServicioBuscarViewModel();
            buscarVM.Clientes = new SelectList(db.Clientes.OrderBy(p=>p.Nombre), "ClienteId", "Nombre");
            buscarVM.Asesores = new SelectList(db.Asesores.OrderBy(o => o.Nombre).ThenBy(o => o.Paterno).ThenBy(o => o.Materno), "AsesorId", "NombreCompleto");
            buscarVM.Status = new SelectList(db.OSStatus, "OSStatusId", "Descr");
            return View(buscarVM);
        }

        
        [HttpPost]
        public ActionResult EjecutarBusqueda(OrdenServicioBuscarViewModel buscarVM)
        {
            IQueryable<OSSeguimiento> resultado = null;
            Usuario usuario;

            switch (buscarVM.Tipo)
            {
                case 1: //Ticket
                    resultado = OrdenServicioBC.Buscar(p => p.Ticket == buscarVM.Ticket);
                    break;
                case 2: //Descripcion
                    resultado = OrdenServicioBC.Buscar(p => p.Obs == buscarVM.OrdenServicioDescr || p.Obs.Contains(buscarVM.OrdenServicioDescr) ||String.IsNullOrEmpty(buscarVM.OrdenServicioDescr));
                    break;
                case 3: //Cliente
                    resultado = OrdenServicioBC.Buscar(p => p.Contacto.ClienteId == buscarVM.ClienteId);
                    break;
                case 4: //Asesor
                    resultado = OrdenServicioBC.Buscar(p => p.AsesorId == buscarVM.AsesorId);
                    break;
                case 5: //Fechas
                    resultado = OrdenServicioBC.Buscar(p => p.Fecha >= buscarVM.FechaInicial && p.Fecha <= buscarVM.FechaFinal);
                    break;
                case 6: //Status
                    resultado = OrdenServicioBC.Buscar(p => p.OSStatusId == buscarVM.StatusId);
                    break;
                default:
                    break;
            }

            usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);

            if (usuario.TipoUsuario == (short)TiposUsuario.MesaAsignacion)
                ViewBag.MostrarReasignar = 1;
            else
                ViewBag.MostrarReasignar = 0;

            return View("_BuscarResultado", resultado);
        }

        public ActionResult ObtenerContactosPorCliente(string ClienteIdSeleccionado)
        {
            int clienteId;

            clienteId = Convert.ToInt16(ClienteIdSeleccionado);

            var contactos = db.Clientes.Find(clienteId).Contactos.OrderBy(o => o.Nombre).ThenBy(o => o.Paterno).ThenBy(o => o.Materno).ToList().Select(a => new SelectListItem()
            {
                Text = a.NombreCompleto,
                Value = a.ContactoId.ToString()
            });
            return Json(contactos);
        }

        public ActionResult ObtenerProyectosPorCliente(string ClienteIdSeleccionado)
        {
            int clienteId;

            clienteId = Convert.ToInt16(ClienteIdSeleccionado);

            var proyectos = db.Clientes.Find(clienteId).Proyectos.OrderBy(o=>o.Descr).ToList().Select(a => new SelectListItem()
            {
                Selected = false,
                Text = a.Descr,
                Value = a.ProyectoId.ToString()
            });
            return Json(proyectos);
        }


        [OutputCache(Duration = 0, VaryByParam = "None")]
        public ActionResult ObtenerProyectosPorTicket(string ticket)
        {
            int ticketId;

            ticketId = Convert.ToInt16(ticket);
            
            var proyectos = OrdenServicioBC.ObtenerProyectosPorTicket(ticketId).ToList().Select(a => new SelectListItem()
            {
                Selected = false,
                Text = a.Descr,
                Value = a.ProyectoId.ToString()
            });            

            return Json(proyectos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAsesores()
        {
            var asesores = db.Asesores.OrderBy(o => o.Nombre).ThenBy(o => o.Paterno).ThenBy(o => o.Materno).ToList().Select(a => new SelectListItem()
            {
                Text = a.NombreCompleto,
                Value = a.AsesorId.ToString()
            });
            return Json(asesores, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AsignarAsesor(OrdenServicioAsignarAsesorViewModel asignarAsesorMV)
        {
            Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
            OrdenServicioValidacionInfo validacionAsignacionAsesor = new OrdenServicioValidacionInfo();
            validacionAsignacionAsesor.Valido = true;

            validacionAsignacionAsesor = OrdenServicioBC.AsignarAsesor(asignarAsesorMV.TicketId, asignarAsesorMV.AsesorId, asignarAsesorMV.ProyectoId, usuario.UsuarioId);

            if (validacionAsignacionAsesor.Valido)
            {
                try
                {
                    OrdenesServicio.Negocio.OrdenServicioBC.EnviarCorreoAsignacionAsesor(asignarAsesorMV.TicketId, asignarAsesorMV.AsesorId);
                }
                catch (Exception)
                {                    
                }
            }

            return Json(new { success = validacionAsignacionAsesor.Valido }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reasignar(long ticketId, int asesorId)
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
            ViewBag.AsesorId = new SelectList(db.Asesores, "AsesorId", "NombreCompleto");

            //ViewBag.StatusId = new SelectList(db.OSDetalleStatus, "OSDetalleSTId", "OSDetalleSTDescr");
            //ViewBag.ServicioId = new SelectList(db.Servicios, "ServicioId", "ServicioDescr");
            //ViewBag.TipoServicioId = new SelectList(db.TiposServicio, "TipoServicioId", "TipoServicioDescr");
            //ViewBag.ViaComunicacionId = new SelectList(db.ViasComunicacion, "ViaComId", "ViaComDescr");
            //ViewBag.ProyectoId = new SelectList(OrdenServicioBC.ObtenerProyectosPorTicket(ticketId), "ProyectoId", "Descr");

            return View(detalleMV);
        }

        [HttpPost]
        public ActionResult ReasignarASesor(OrdenServicioCambioAsesorViewModel cambioAsesor)
        {
            bool cambioValido = false;
            string nombreCompleto = string.Empty;

            if (ModelState.IsValid)
            {
                Usuario usuario = Negocio.Seguridad.ObtenerUsuario(User.Identity.Name);
                cambioValido = OrdenServicioBC.Reasignar(cambioAsesor.TicketId, cambioAsesor.AsesorId, cambioAsesor.Observacion, usuario.UsuarioId);
                if (cambioValido)
                {
                    Asesor asesor = db.Asesores.Find(cambioAsesor.AsesorId);
                    nombreCompleto = asesor.NombreCompleto;

                    try
                    {
                        OrdenesServicio.Negocio.OrdenServicioBC.EnviarCorreoReAsignacionAsesor(cambioAsesor.TicketId);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return Json(new { NombreCompleto = nombreCompleto, success = cambioValido }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}