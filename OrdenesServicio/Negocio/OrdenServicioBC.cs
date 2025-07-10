using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo.Model.Complex;
using ZOE.OS.Modelo;
using System.Data;
using System.Data.Entity.Infrastructure;
using ZOE.OrdenesServicio.Modelo;
using System.Net.Mail;
using System.Text;

namespace ZOE.OrdenesServicio.Negocio
{
    public class OrdenServicioBC
    {
        public static long Crear(OrdenServicio nuevaOrdenServicio, int usuarioId)
        {
            using (OSContext ctx = new OSContext())
            {
                if (nuevaOrdenServicio.AsesorId.HasValue)
                {
                    Asesor asesor = ctx.Asesores.Find(nuevaOrdenServicio.AsesorId);
                    nuevaOrdenServicio.AreaRespId = asesor.AreaId;
                }

                if (nuevaOrdenServicio.ProyectoId == 0)
                    nuevaOrdenServicio.ProyectoId = null;

                nuevaOrdenServicio.Fecha = System.DateTime.Now;
                nuevaOrdenServicio.UsuarioIdRegistro = usuarioId;

                ctx.OrdenesServicio.Add(nuevaOrdenServicio);
                                
                ctx.SaveChanges();

                CambiarStatus(nuevaOrdenServicio.Ticket, "Creacion de una nuevo orden de servicio", OrdenServicioStatus.Creada, usuarioId);
            }

            return nuevaOrdenServicio.Ticket;
        }

        public static OrdenServicio Obtener(long ticketId)
        {
            OrdenServicio os = new OrdenServicio();

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = true;
                os = ctx.OrdenesServicio.Include("Proyecto").Include("Contacto").Include("Asesor").Include("Status").First(p => p.Ticket == ticketId);
                os.Contacto.Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == os.Contacto.ClienteId);

                if (os.ProyectoId.HasValue)
                    os.Proyecto.TipoProyecto = ctx.TiposProyecto.FirstOrDefault(t => t.TipoProyectoId == os.Proyecto.TipoProyectoId);
            }

            return os;
        }

        public static IQueryable<OSDetalle> ObtenerActividades(long ticketId)
        {
            List<OSDetalle> osActividades = new List<OSDetalle>();

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = true;

                osActividades = ctx.DetallesOrdenServicio
                    .Include("Contacto")
                    .Include("Asesor")
                    .Include("Status")
                    .Include("Servicio")
                    .Include("TipoServicio")
                    .Where(p => p.Ticket == ticketId).OrderBy(o => o.DetalleId).ToList();
            }

            return osActividades.AsQueryable<OSDetalle>();
        }


        public static OSDetalle ObtenerActividad(long ticketId, long actividadId)
        {
            OSDetalle actividad = new OSDetalle();

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = false;

                actividad = ctx.DetallesOrdenServicio
                    .Include("Contacto")
                    .Include("Asesor")
                    .Include("Status")
                    .Include("Servicio")
                    .Include("TipoServicio")
                    .Where(p => p.Ticket == ticketId && p.DetalleId == actividadId).FirstOrDefault();
            }

            return actividad;
        }

        public static int ObtenerTotalActividades(long ticketId)
        {
            int count = 0;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = false;

                var qCount = ctx.DetallesOrdenServicio.Where(p => p.Ticket == ticketId).ToList();

                count = qCount.Count;
            }

            return count;
        }

        public static IQueryable<OSSeguimiento> ObtenerListaSeguimientoPorAsesor(int asesorId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio
                    .Include("Proyecto")
                    .Include("Contacto")
                    .Include("Asesor")
                    .Include("Status")
                    .Where(p => p.AsesorId == asesorId && (p.OSStatusId == (short)OrdenServicioStatus.Creada || p.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento) || p.OSDetalles.Any(d => d.AsesorId == asesorId && (d.OrdenServicio.OSStatusId == (short)OrdenServicioStatus.Creada || d.OrdenServicio.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento))).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = asesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        public static IQueryable<OSSeguimiento> ObtenerListaSeguimientoSinAtender()
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto").Include("Asesor").Include("Status").Where(p => p.OSStatusId == (short)OrdenServicioStatus.Creada && p.AsesorId.HasValue).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr
                    };

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoPropias(int asesorId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto")
                    .Include("Asesor")
                    .Include("Status")
                    .Where(p => p.AsesorId == asesorId && (p.OSStatusId == (short)OrdenServicioStatus.Creada || p.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento)).OrderBy(o=>o.Fecha).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoPropiasTerminadas(int asesorId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto")
                    .Include("Asesor")
                    .Include("Status")
                    .Where(p => p.AsesorId == asesorId && (p.OSStatusId == (short)OrdenServicioStatus.Terminada)).OrderBy(o => o.Fecha).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoSinProyecto()
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto").Include("Asesor").Include("Status").Where(p => p.ProyectoId.HasValue == false).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr
                    };

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoSinAsesor()
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto").Include("Status").Where(p => p.AsesorId.HasValue == false && p.OSStatusId == (short)OrdenServicioStatus.Creada).ToList();
                int? totalMinutosPoliza = 0;
                int? totalMinAbonadosPoliza = 0;
                int saldoPoliza = 0;

                foreach (var ordenServicio in ordenes)
                {
                    totalMinutosPoliza = ctx.DetallesOrdenServicio.Where(d => d.Contacto.ClienteId == ordenServicio.Contacto.ClienteId && d.OrdenServicio.Proyecto.TipoProyectoId == 1 && d.TipoServicio.AfectaPoliza == "S").Sum(m => (Int32?)m.Minutos) ?? 0;
                    totalMinAbonadosPoliza = ctx.ProyectoAbonos.Where(p => p.Proyecto.ClienteId == ordenServicio.Contacto.ClienteId && p.Proyecto.TipoProyectoId == 1).Sum(a => (Int32?)a.Minutos) ?? 0;
                    saldoPoliza =  (totalMinAbonadosPoliza.HasValue ? totalMinAbonadosPoliza.Value : 0) - (totalMinutosPoliza.HasValue ? totalMinutosPoliza.Value : 0);

                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr,
                        SaldoPoliza = saldoPoliza
                    };

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }        
        
        
        public static List<Contacto> ObtenerContactosPorTicket(long ticketID)
        {
            List<Contacto> contactos = null;

            using (OSContext ctx = new OSContext())
            {
                contactos = ctx.OrdenesServicio.Include("Contacto").FirstOrDefault(p => p.Ticket == ticketID).Contacto.Cliente.Contactos.ToList();
            }

            return contactos;

        }

        public static List<Proyecto> ObtenerProyectosPorTicket(long ticketID)
        {
            List<Proyecto> proyectos = null;

            using (OSContext ctx = new OSContext())
            {
                proyectos = ctx.OrdenesServicio.Include("Proyecto").FirstOrDefault(p => p.Ticket == ticketID).Contacto.Cliente.Proyectos.ToList();
            }

            return proyectos;

        }

        public static List<OSStatus> ObtenerStatus()
        {
            List<OSStatus> status = new List<OSStatus>();

            using (OSContext ctx = new OSContext())
            {
                status = ctx.OSStatus.OrderBy(o=>o.Descr).ToList();
            }

            return status;
        }

        public static List<Asesor> ObtenerAsesores()
        {
            List<Asesor> asesores = new List<Asesor>();
            
            using (OSContext ctx = new OSContext())
            {
                asesores = ctx.Asesores.OrderBy(o => o.Nombre).ThenBy(o => o.Paterno).ThenBy(o => o.Materno).ToList();
            }

            return asesores;
        }

        public static IQueryable<Cliente> ObtenerClientes()
        {
            OSContext ctx = new OSContext();

            return ctx.Clientes.OrderBy(s => s.Nombre).ToList().AsQueryable();
        }

        public static IQueryable<Area> ObtenerAreas()
        {
            OSContext ctx = new OSContext();

            return ctx.Areas.ToList().AsQueryable();
        }

        internal static short ObtenerAreaAsesor(int asesorId)
        {
            short areaResp;

            using (OSContext ctx = new OSContext())
            {
                areaResp = ctx.Asesores.First(p => p.AsesorId == asesorId).AreaId;
            }

            return areaResp;
        }

        public static IQueryable<OSSeguimiento> Buscar(System.Linq.Expressions.Expression<Func<OrdenServicio, bool>> expresion)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Contacto").Include("Asesor").Include("Status").Where(expresion).OrderBy(o => o.Fecha).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Cliente = ctx.Clientes.FirstOrDefault(p => p.ClienteId == ordenServicio.Contacto.ClienteId).Nombre,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Status = ordenServicio.Status.Descr,
                        FechaUlitmoMovimiento = System.DateTime.Now
                    };

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        #region Metodos para el contacto
        public static IQueryable<OSSeguimiento> ObtenerListaSeguimientoPorContacto(int contactoId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                //List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Asesor").Include("Status").Where(p => ((p.ContactoId == contactoId && p.Proyecto.TipoProyectoId == 1) || p.OSDetalles.Any(d => d.ContactoId == contactoId && d.OrdenServicio.Proyecto.TipoProyectoId == 1) && (p.OSStatusId == (short)OrdenServicioStatus.Creada || p.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento))).ToList();
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Asesor").Include("Status").Where(p => ((p.ContactoId == contactoId) || p.OSDetalles.Any(d => d.ContactoId == contactoId) && (p.OSStatusId == (short)OrdenServicioStatus.Creada || p.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento))).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Proyecto = (ordenServicio.Proyecto == null ? Resources.SOResource.SinProyecto : ordenServicio.Proyecto.Descr),
                        Status = ordenServicio.Status.Descr 
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoPropiasPorContacto(int contactoId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();
            int usuarioId;

            using (OSContext ctx = new OSContext())
            {
                usuarioId = ctx.Usuarios.Where(u => u.ContactoId == contactoId).FirstOrDefault().UsuarioId;
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Asesor").Include("Status").Where(p => p.UsuarioIdRegistro == usuarioId || (p.ContactoId == contactoId)).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Asesor = (ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto),
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Proyecto = (ordenServicio.Proyecto == null ? Resources.SOResource.SinProyecto : ordenServicio.Proyecto.Descr),
                        Status = ordenServicio.Status.Descr 
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }

        internal static IQueryable<OSSeguimiento> ObtenerListaSeguimientoSinAtenderPorCliente(int clienteId)
        {
            List<OSSeguimiento> listaOS = new List<OSSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                List<OrdenServicio> ordenes = ctx.OrdenesServicio.Include("Asesor").Include("Status").Where(p => p.Contacto.ClienteId == clienteId && p.Proyecto.TipoProyectoId == 1 && p.OSStatusId == (short)OrdenServicioStatus.Creada).ToList();

                foreach (var ordenServicio in ordenes)
                {
                    OSSeguimiento seguimiento = new OSSeguimiento()
                    {
                        Ticket = ordenServicio.Ticket,
                        Descripcion = ordenServicio.Obs,
                        AsesorId = ordenServicio.AsesorId,
                        Asesor = ordenServicio.Asesor == null ? Resources.SOResource.SinAsesor : ordenServicio.Asesor.NombreCompleto,
                        Contacto = ordenServicio.Contacto.NombreCompleto,
                        Fecha = ordenServicio.Fecha,
                        Obs = ordenServicio.Obs,
                        Proyecto = (ordenServicio.Proyecto == null ? Resources.SOResource.SinProyecto : ordenServicio.Proyecto.Descr),
                        Status = ordenServicio.Status.Descr 
                    };

                    if (ordenServicio.OSDetalles.Any())
                        seguimiento.FechaUlitmoMovimiento = ordenServicio.OSDetalles.Max(p => p.FechaRegistro);

                    listaOS.Add(seguimiento);
                }

            }

            return listaOS.AsQueryable<OSSeguimiento>();
        }
        #endregion

        internal static void CrearActividad(OSDetalle actividad, int usuarioId)
        {
            bool crearNuevaActividad = true;

            crearNuevaActividad = (actividad.DetalleId == 0);
            OSDetalle actividadValoresActuales;

            using (OSContext ctx = new OSContext())
            {
                actividadValoresActuales = ctx.DetallesOrdenServicio.Where(p => p.Ticket == actividad.Ticket && p.DetalleId == actividad.DetalleId).FirstOrDefault();
            }

            using (OSContext ctx = new OSContext())
            {
                if (crearNuevaActividad)
                {
                    long? actividadId;
                    TipoServicio tipoServicio = ctx.TiposServicio.Find(actividad.TipoServicioId);
                    var qActividades = ctx.DetallesOrdenServicio.Where(o => o.Ticket == actividad.Ticket);

                    if (qActividades.Count() > 0)
                    {
                        actividadId = qActividades.Max(m => m.DetalleId);
                        actividadId = actividadId + 1;
                    }
                    else
                        actividadId = 1;

                    actividad.DetalleId = actividadId.Value;
                    actividad.AfectaPoliza = tipoServicio.AfectaPoliza;

                    ctx.DetallesOrdenServicio.Add(actividad);

                    //Actualizar saldo del proyecto
                    if (tipoServicio.AfectaPoliza == "S")
                    {
                        OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(actividad.Ticket);
                        Proyecto proyectoAfectar = ctx.Proyectos.Find(ordenServicio.ProyectoId);
                        if (proyectoAfectar != null)
                        {
                            proyectoAfectar.Saldo = (proyectoAfectar.Saldo - actividad.Minutos);
                            ctx.Entry(proyectoAfectar).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
                else
                {
                    //Actualizar saldo del proyecto
                    TipoServicio tipoServicio = ctx.TiposServicio.Find(actividad.TipoServicioId);
                    actividad.AfectaPoliza = tipoServicio.AfectaPoliza;
                    if (tipoServicio.AfectaPoliza == "S")
                    {
                        if (actividadValoresActuales.Minutos != actividad.Minutos)
                        {
                            OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(actividad.Ticket);
                            Proyecto proyectoAfectar = ctx.Proyectos.Find(ordenServicio.ProyectoId);

                            if (proyectoAfectar != null)
                            {
                                if (actividad.Minutos < actividadValoresActuales.Minutos)
                                    proyectoAfectar.Saldo = (proyectoAfectar.Saldo + (actividadValoresActuales.Minutos - actividad.Minutos));
                                else
                                    proyectoAfectar.Saldo = (proyectoAfectar.Saldo - (actividad.Minutos - actividadValoresActuales.Minutos));

                                ctx.Entry(proyectoAfectar).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }

                    ctx.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
                }

                try
                {
                    ctx.SaveChanges();

                    //Agregar bitacora de creacion de la actividad
                    if (crearNuevaActividad)
                        CambiarStatusActividad(actividad.Ticket, actividad.DetalleId, "Creación de la actividad", actividad.StatusId, (short)ActividadStatus.Registrada, usuarioId);
                }
                catch (DbUpdateConcurrencyException updConcurrencyExp)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        internal static bool VerificarCambioStatusActividadValido(long ticketId, long detalleId, short statusIdCambiar)
        {
            bool valido = true;

            using (OSContext ctx = new OSContext())
            {
                OSDetalle actividad = ctx.DetallesOrdenServicio.Where(a => a.Ticket == ticketId && a.DetalleId == detalleId).FirstOrDefault();

                switch ((ActividadStatus)statusIdCambiar)
                {
                    case ActividadStatus.Activa:
                        valido = (actividad.StatusId == (short)ActividadStatus.Registrada ||
                                  actividad.StatusId == (short)ActividadStatus.Pendiente);
                        break;
                    case ActividadStatus.Pendiente:
                        valido = (actividad.StatusId == (short)ActividadStatus.Registrada ||
                                  actividad.StatusId == (short)ActividadStatus.Activa);
                        break;
                    case ActividadStatus.Terminada:
                        valido = (actividad.StatusId == (short)ActividadStatus.Registrada ||
                                  actividad.StatusId == (short)ActividadStatus.Activa);
                        break;
                    case ActividadStatus.Cancelada:
                        valido = (actividad.StatusId == (short)ActividadStatus.Registrada ||
                                  actividad.StatusId == (short)ActividadStatus.Pendiente ||
                                  actividad.StatusId == (short)ActividadStatus.Activa);
                        break;
                    default:
                        valido = true;
                        break;
                }

            }

            return valido;
        }

        internal static void CambiarStatusActividad(long ticketId, long detalleId, string obs, short statusIdCambiar, short statusIdActual, int usuarioId)
        {
            using (OSContext ctx = new OSContext())
            {
                CambiarStatusActividad(ticketId, detalleId, obs, statusIdCambiar, statusIdActual, usuarioId, ctx);
                ctx.SaveChanges();
            }
        }

        internal static void CambiarStatusActividad(long ticketId, long detalleId, string obs, short statusIdCambiar, short statusIdActual, int usuarioId, OSContext ctx)
        {
            OSDetalle actividad = ctx.DetallesOrdenServicio.Where(a => a.Ticket == ticketId && a.DetalleId == detalleId).FirstOrDefault();
            Usuario usuario = ctx.Usuarios.Find(usuarioId);

            actividad.StatusId = statusIdCambiar;

            switch ((ActividadStatus)statusIdCambiar)
            {
                case ActividadStatus.Pendiente:
                    actividad.FechaPendiente = System.DateTime.Now;
                    actividad.ObsPendiente = obs;
                    actividad.UsuarioPendiente = usuario;
                    break;
                case ActividadStatus.Terminada:
                    actividad.FechaTerminada = System.DateTime.Now;
                    actividad.ObsTerminada = obs;
                    actividad.UsuarioTermino = usuario;
                    break;
                case ActividadStatus.Cancelada:
                    actividad.FechaCancelada = System.DateTime.Now;
                    actividad.ObsCancelada = obs;
                    actividad.UsuarioCancelo = usuario;
                    break;
                default:
                    break;
            }

            BitOSDetalleStatus bitacoraCambioStatus = new BitOSDetalleStatus
            {
                Fecha = System.DateTime.Now,
                Observacion = obs,
                StatusAnteriorId = statusIdActual,
                StatusCambioId = statusIdCambiar,
                Usuario = usuario,
                OSDetalle = actividad
            };

            try
            {
                ctx.BitacoraStatusDetalleOrdenServicio.Add(bitacoraCambioStatus);

                ctx.Entry(actividad).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }

        }

        internal static OrdenServicioValidacionInfo CambiarStatus(long ticket, string obs, OrdenServicioStatus statusCambiar, int usuarioId)
        {
            OrdenServicioValidacionInfo validacionCambioStatus = VerificarCambioStatusValido(ticket, statusCambiar);

            if (validacionCambioStatus.Valido)
            {
                using (OSContext ctx = new OSContext())
                {
                    CambiarStatus(ticket, obs, statusCambiar, usuarioId, ctx);

                    ctx.SaveChanges();
                }
            }

            return validacionCambioStatus;
        }

        internal static OrdenServicioValidacionInfo CambiarStatus(long ticket, string obs, OrdenServicioStatus statusCambiar, int usuarioId, OSContext ctx)
        {
            OrdenServicioValidacionInfo validacionCambioStatus = VerificarCambioStatusValido(ticket, statusCambiar);

            if (validacionCambioStatus.Valido)
            {
                Usuario usuario = ctx.Usuarios.Find(usuarioId);
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticket);
                OSStatus statusCambia = ctx.OSStatus.Find((short)statusCambiar);
                OSStatus statusActual = ctx.OSStatus.Find(ordenServicio.OSStatusId);

                BitOSStatus bitacoraStatus = new BitOSStatus()
                {
                    Fecha = System.DateTime.Now,
                    Ticket = ticket,
                    StatusCambio = statusCambia,
                    StatusAnterior = statusActual,
                    StatusAnteriorId = ordenServicio.OSStatusId,
                    StatusCambioId = (short)statusCambiar,
                    Observacion = obs,
                    Usuario = usuario
                };

                ordenServicio.OSStatusId = (short)statusCambiar;
                ctx.Entry(ordenServicio).State = System.Data.Entity.EntityState.Modified;

                ctx.BitacoraStatusOrdenServicio.Add(bitacoraStatus);
            }

            return validacionCambioStatus;
        }

        internal static bool VerificarEsResponsableOrdenServicio(long ticket, int usuarioId)
        {
            bool valido = true;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.LazyLoadingEnabled = true;
                ctx.Configuration.ProxyCreationEnabled = false;

                Usuario usuario = ctx.Usuarios.Find(usuarioId);
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticket);

                valido = (usuario.AsesorId == ordenServicio.AsesorId);
            }

            return valido;
                
        }

        internal static OrdenServicioValidacionInfo VerificarCambioStatusValido(long ticket, OrdenServicioStatus statusCambiar)
        {
            bool cambioValido = true;
            OrdenServicioValidacionInfo validacionRetorno = new OrdenServicioValidacionInfo();

            using (OSContext ctx = new OSContext())
            {
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticket);


                switch (statusCambiar)
                {
                    case OrdenServicioStatus.EnSeguimiento:
                        cambioValido = (ordenServicio.OSStatusId == (short)OrdenServicioStatus.Creada);
                        break;
                    case OrdenServicioStatus.Cerrada:
                    case OrdenServicioStatus.Terminada:

                        if (statusCambiar == OrdenServicioStatus.Terminada)
                        {
                            cambioValido = (ordenServicio.OSStatusId == (short)OrdenServicioStatus.Creada ||
                                            ordenServicio.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento);

                            if (!cambioValido)
                                validacionRetorno.Mensaje = Resources.SOResource.MsgTerminarOSNoValido; // "La orden de servicio debe estar creada o en seguimiento para ponerla como terminada";
                        }
                        else
                        {
                            //cambioValido = (ordenServicio.OSStatusId == (short)OrdenServicioStatus.Creada ||
                            //                ordenServicio.OSStatusId == (short)OrdenServicioStatus.EnSeguimiento ||
                            //                ordenServicio.OSStatusId == (short)OrdenServicioStatus.Terminada);
                            cambioValido = (ordenServicio.OSStatusId == (short)OrdenServicioStatus.Terminada);

                            if (!cambioValido)
                                validacionRetorno.Mensaje = Resources.SOResource.MsgCerrarOSNoValido; // "La orden de servicio debe estar Terminada para ponerla como Cerrada";
                        }

                        if (cambioValido)
                        {
                            var qVerifAct = ordenServicio.OSDetalles.Where(p => p.StatusId == (short)ActividadStatus.Registrada ||
                                                                   p.StatusId == (short)ActividadStatus.Activa ||
                                                                   p.StatusId == (short)ActividadStatus.Pendiente);

                            cambioValido = (qVerifAct.Count() <= 0);

                            if (!cambioValido)
                                validacionRetorno.Mensaje = Resources.SOResource.MsgOSActNoTerminadaCancelada; // "La orden de servicio debe tener sus actividades terminadas y/o canceladas";
                        }
                        break;
                    default:
                        break;
                }
            }

            validacionRetorno.Valido = cambioValido;

            return validacionRetorno;
        }


        internal static bool CambiarProyecto(long ticketId, short nuevoProyectoId)
        {
            bool cambioValido = true;

            using (OSContext ctx = new OSContext())
            {
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticketId);
                if (ordenServicio != null)
                {
                    ordenServicio.ProyectoId = nuevoProyectoId;
                    ctx.Entry(ordenServicio).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
            }

            return cambioValido;
        }

        public static IQueryable<OSNotaSeguimiento> ObtenerComentarios(long ticketId)
        {
            List<OSNotaSeguimiento> notas = new List<OSNotaSeguimiento>();

            using (OSContext ctx = new OSContext())
            {
                var qNotas = ctx.NotasOrdenServicio.Include("Usuario").Where(p => p.Ticket == ticketId).OrderBy(o => o.FechaRegistro).ToList();
                foreach (var item in qNotas)
                {
                    notas.Add(new OSNotaSeguimiento()
                    {
                        Ticket=item.Ticket,
                        OSNotaId=item.OSNotaId,
                        FechaRegistro=item.FechaRegistro,
                        Nota=item.Nota,
                        UsuarioId=item.UsuarioId,
                        NombreUsuario=item.Usuario.AsesorId.HasValue ? item.Usuario.Asesor.NombreCompleto : item.Usuario.Contacto.NombreCompleto
                    }
                    );
                }
            }

            if (notas.Count == 0)
            {
                notas.Add(new OSNotaSeguimiento()
                {
                    Ticket = null,
                    OSNotaId = null,
                    FechaRegistro = null,
                    UsuarioId = null
                }
               );
            }

            return notas.AsQueryable();

            //using (OSContext ctx = new OSContext())
            //{
            //    var qNota = (from nota in ctx.NotasOrdenServicio.Include("Usuario")
            //                 join usuario in ctx.Usuarios.Include("Asesor").Include("Contacto")
            //                 on nota.UsuarioId equals usuario.UsuarioId
            //                 orderby nota.FechaRegistro
            //                 where nota.Ticket == ticketId
            //                 select new OSNotaSeguimiento() 
            //                 {
            //                     OSNotaId=nota.OSNotaId,
            //                     FechaRegistro=nota.FechaRegistro,
            //                     Nota=nota.Nota,
            //                     Ticket=nota.Ticket,
            //                     NombreUsuario = usuario.AsesorId.HasValue ? usuario.Asesor.NombreCompleto : usuario.Contacto.NombreCompleto
            //                 });

            //    notas = qNota.ToList();

            //    foreach (var item in notas)
            //    {
            //        item.NombreUsuario = item.Usuario.AsesorId.HasValue ? item.Usuario.Asesor.NombreCompleto : item.Usuario.Contacto.NombreCompleto;
            //    }

            //    return notas.AsQueryable();
            //}
            
        }

        internal static int ObtenerTotalComentarios(long ticketId)
        {
            int count = 0;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                ctx.Configuration.LazyLoadingEnabled = false;

                var qCount = ctx.NotasOrdenServicio.Where(p => p.Ticket == ticketId).ToList();

                count = qCount.Count;
            }

            return count;
        }

        internal static bool AgregarComentario(long ticketId, string comentario, int usuarioId)
        {
            bool exito = true;
            short notaId = 0;
            
            using (OSContext ctx = new OSContext())
            {
                var qNotas = ctx.NotasOrdenServicio.Where(o => o.Ticket == ticketId);

                if (qNotas.Count() > 0)
                    notaId = qNotas.Max(p => p.OSNotaId);

                notaId = Convert.ToInt16(notaId + 1);

                OSNota nuevaNota = new OSNota()
                {
                    FechaRegistro=System.DateTime.Now, 
                    Ticket=ticketId, 
                    UsuarioId=usuarioId,
                    OSNotaId=notaId,
                    Nota=comentario
                };

                ctx.NotasOrdenServicio.Add(nuevaNota);
                ctx.SaveChanges();
            }

            return exito;
        }

        public static IQueryable<OrdenServicioHistoriaViewModel> ObtenerHistoria(long ticketId)
        {
            List<OrdenServicioHistoriaViewModel> historia = new List<OrdenServicioHistoriaViewModel>();
            using (OSContext ctx = new OSContext())
            {

                var qHis = ctx.BitacoraStatusOrdenServicio.Include("Usuario").Include("StatusAnterior").Include("StatusCambio").Where(p => p.Ticket == ticketId).OrderBy(o => o.Fecha).ToList();
                foreach (var item in qHis)
                {
                    historia.Add(new OrdenServicioHistoriaViewModel()
                    {
                        Fecha = item.Fecha,
                        Obs = item.StatusAnteriorId == (short)OrdenServicioStatus.Creada && item.StatusCambioId == (short)OrdenServicioStatus.Creada ? "" : item.Observacion != null ? item.Observacion : "",
                        CambioStatus = item.StatusAnteriorId == (short)OrdenServicioStatus.Creada && item.StatusCambioId == (short)OrdenServicioStatus.Creada ? "Creo La Orden de Servicio" : string.Format("Cambio de status {0} a status {1}", item.StatusAnterior.Descr, item.StatusCambio.Descr),
                        Responsable = item.Usuario.AsesorId.HasValue ? item.Usuario.Asesor.NombreCompleto : item.Usuario.Contacto.NombreCompleto
                    }
                    );
                }
                if (qHis.Count <= 0)
                {
                    historia.Add(new OrdenServicioHistoriaViewModel() { });
                }
                return historia.AsQueryable();
            }
        }

        //public static IQueryable<OrdenServicioHistoriaViewModel> ObtenerHistoria(long ticketId)
        //{
        //    List<OrdenServicioHistoriaViewModel> historia = new List<OrdenServicioHistoriaViewModel>();
        //    using (OSContext ctx = new OSContext())
        //    {

        //        //var qHis = from h in ctx.BitacoraStatusOrdenServicio
        //        //           from sa in ctx.OSStatus
        //        //           from sc in ctx.OSStatus
        //        //           where h.Ticket == ticketId && sa.OSStatusId == h.StatusAnteriorId && sc.OSStatusId == h.StatusCambioId
        //        //           select new OrdenServicioHistoriaViewModel { Fecha = h.Fecha, Obs = h.Observacion, CambioStatus = h.StatusAnteriorId == 1 && h.StatusCambioId == 1 ? "Creación de la orden de servicio" : "Cambio status de " + sa.Descr + " a " + sc.Descr, UsuarioResponsable = h.Usuario };

        //        ctx.Configuration.LazyLoadingEnabled = false;
        //        ctx.Configuration.ProxyCreationEnabled = false;

        //        var qHis = from h in ctx.BitacoraStatusOrdenServicio.Include("Usuario") 
        //                   join sa in ctx.OSStatus on h.StatusAnteriorId equals sa.OSStatusId
        //                   join sc in ctx.OSStatus on h.StatusCambioId equals sc.OSStatusId
        //                   orderby h.Fecha
        //                   where h.Ticket == ticketId 
        //                   select new OrdenServicioHistoriaViewModel { Fecha = h.Fecha, Obs = h.Observacion, CambioStatus = h.StatusAnteriorId == 1 && h.StatusCambioId == 1 ? "Creación de la orden de servicio" : "Cambio status de " + sa.Descr + " a " + sc.Descr, UsuarioResponsable = h.Usuario };


        //        historia = qHis.ToList();

        //        foreach (var item in historia)
        //        {
        //            item.Responsable = item.UsuarioResponsable.AsesorId.HasValue ? item.UsuarioResponsable.Asesor.NombreCompleto : item.UsuarioResponsable.Contacto.NombreCompleto;
        //        }

        //        return historia.AsQueryable();
        //    }
        //}



        internal static OrdenServicioValidacionInfo AsignarAsesor(long ticketId, int asesorId, int proyectoId, int usuarioId)
        {
            OrdenServicioValidacionInfo validacionRetorno = new OrdenServicioValidacionInfo();

            using (OSContext ctx = new OSContext())
            {
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticketId);
                if (ordenServicio != null)
                {
                    Usuario usuario = ctx.Usuarios.Find(usuarioId);
                    ordenServicio.AsesorId = asesorId;
                    ordenServicio.ProyectoId = proyectoId;

                    Asesor asesor = ctx.Asesores.Find(asesorId);
                    if (asesor != null)
                    {
                        ordenServicio.AreaRespId = asesor.AreaId;
                    }

                    BitOSStatus bitacoraStatus = new BitOSStatus()
                    {
                        Fecha = System.DateTime.Now,
                        Ticket = ticketId,
                        StatusAnterior = ctx.OSStatus.Find(ordenServicio.OSStatusId),
                        StatusCambio = ctx.OSStatus.Find((short)OrdenServicioStatus.Asignado),
                        StatusAnteriorId = ordenServicio.OSStatusId,
                        StatusCambioId = (short)OrdenServicioStatus.Asignado,
                        Usuario = usuario,
                        Observacion = String.Format("Asesor {0} Asignado", asesor.NombreCompleto),
                    };
                    
                    ctx.BitacoraStatusOrdenServicio.Add(bitacoraStatus);

                    ctx.Entry(ordenServicio).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    validacionRetorno.Valido = true;
                }
            }

            return validacionRetorno;
        }

        internal static bool Reasignar(long ticketId, int nuevoAsesorId, string observaciones, int usuarioId)
        {
            bool cambioValido = true;

            using (OSContext ctx = new OSContext())
            {
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticketId);
                if (ordenServicio != null)
                {
                    int asesorActual = ordenServicio.AsesorId.Value;

                    Asesor asesorNuevo = ctx.Asesores.Find(nuevoAsesorId);

                    BitOSStatus bitacoraStatus = new BitOSStatus()
                    {
                        Fecha = System.DateTime.Now,
                        Ticket = ticketId,
                        StatusAnterior = ctx.OSStatus.Find(ordenServicio.OSStatusId),
                        StatusCambio = ctx.OSStatus.Find((short)OrdenServicioStatus.Reasignado),
                        StatusAnteriorId = ordenServicio.OSStatusId,
                        StatusCambioId = (short)OrdenServicioStatus.Reasignado,
                        Observacion = String.Format("Reasignacion de Asesor, Anterior:{0}, Nuevo:{1}", ordenServicio.Asesor.NombreCompleto, asesorNuevo.NombreCompleto),
                        Usuario = ctx.Usuarios.Find(usuarioId)
                    };
                    ctx.BitacoraStatusOrdenServicio.Add(bitacoraStatus);

                    BitOSReasignacion bitacoraReasignacion = new BitOSReasignacion()
                    {
                        Fecha = System.DateTime.Now,
                        Ticket = ticketId,
                        AsesorAnterior = ordenServicio.Asesor,
                        AsesorCambio = asesorNuevo,
                        AsesorAnteriorId = asesorActual,
                        AsesorCambioId = nuevoAsesorId,
                        StatusId = ordenServicio.OSStatusId,
                        Status = ctx.OSStatus.Find(ordenServicio.OSStatusId),
                        Observacion = observaciones,
                        Usuario = ctx.Usuarios.Find(usuarioId)
                    };
                    ctx.BitacoraReasignacion.Add(bitacoraReasignacion);

                    ordenServicio.AsesorId = nuevoAsesorId;
                    ordenServicio.AreaRespId = asesorNuevo.AreaId;

                    ctx.Entry(ordenServicio).State = System.Data.Entity.EntityState.Modified;

                    ctx.SaveChanges();                    
                }
            }

            return cambioValido;
        }

        internal static void EnviarCorreoPorCreacionTicket(long ticket, int usuarioId, CrearOrdenServicioViewModel modelo, string userState)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;
                    Contacto contacto;

                    try
                    {
                        if (modelo.AsesorCreando)
                        {
                            if (modelo.NotificarContactoEmail)
                            {
                                client = new SmtpClient();
                                client.Host = paramCorreo.Host;

                                message = new MailMessage();
                                message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);

                                contacto = ctx.Contactos.Find(modelo.ContacoIdSeleccionado);
                                Asesor asesor = ctx.Asesores.Find(modelo.OrdenServicio.AsesorId);

                                if (contacto != null && contacto.Email != null && contacto.Email.Length > 0)
                                {
                                    message.To.Add(new MailAddress(contacto.Email, contacto.NombreCompleto));
                                    //message.To.Add(new MailAddress("gluque@hotmail.com", "Gera"));

                                    if (asesor != null && asesor.Email != null && asesor.Email.Length > 0)
                                        message.Bcc.Add(new MailAddress(asesor.Email, asesor.NombreCompleto));

                                    message.Subject = string.Format(Resources.SOEmailResource.CrearOSSubject, ticket);

                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine(String.Format("{0}/{1}:", contacto.Cliente.Nombre, contacto.NombreCompleto));
                                    sb.AppendLine("");
                                    sb.AppendLine(Resources.SOEmailResource.OSMsg1);
                                    sb.AppendLine(modelo.OrdenServicio.Obs);
                                    sb.AppendLine(string.Format(Resources.SOEmailResource.OSCreada, ticket));
                                    sb.AppendLine("");
                                    sb.AppendLine(Resources.SOEmailResource.Seguimiento);
                                    sb.AppendLine("");
                                    sb.AppendLine(Resources.SOEmailResource.Informativo);

                                    message.Body = sb.ToString();

                                    client.SendAsync(message, userState);
                                }
                            }
                        }
                        else
                        {
                            client = new SmtpClient();
                            client.Host = paramCorreo.Host;

                            message = new MailMessage();
                            message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);

                            contacto = ctx.Contactos.Find(modelo.OrdenServicio.ContactoId);

                            if (contacto != null && contacto.Email != null && contacto.Email.Length > 0)
                            {
                                message.To.Add(new MailAddress(contacto.Email, contacto.NombreCompleto));
                                //message.To.Add(new MailAddress("gluque@hotmail.com", contacto.NombreCompleto));

                                //Agregar la copia ciega para el grupo de asesores que reciben correo de creacion de ticket por parte del cliente
                                var asesores = ctx.GrupoAsesores.Include("Asesor").Where(g => g.GrupoId == 1);
                                foreach (var asesor in asesores)
                                {
                                    if (asesor.Asesor.Email != null && asesor.Asesor.Email.Length > 0)
                                        message.Bcc.Add(asesor.Asesor.Email);
                                }

                                message.Subject = string.Format(Resources.SOEmailResource.CrearOSSubject, ticket);

                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine(String.Format("{0}/{1}:", contacto.Cliente.Nombre, contacto.NombreCompleto));
                                sb.AppendLine("");
                                sb.AppendLine(Resources.SOEmailResource.OSMsg1);
                                sb.AppendLine(modelo.OrdenServicio.Obs);
                                sb.AppendLine("");
                                sb.AppendLine(string.Format(Resources.SOEmailResource.OSCreada, ticket));

                                if (DateTime.Now.Day == 1 && DateTime.Now.Month == 5)
                                {
                                    sb.AppendLine(Resources.SOEmailResource.DiaInhabil);
                                }

                                sb.AppendLine("");
                                sb.AppendLine(Resources.SOEmailResource.Informativo);

                                message.Body = sb.ToString();

                                client.SendAsync(message, userState);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        internal static void EnviarCorreoContactoCerroTicket(long ticketId, int contactoId)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;

                    client = new SmtpClient();
                    client.Host = paramCorreo.Host;

                    message = new MailMessage();
                    message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);

                    OrdenServicio ticket = ctx.OrdenesServicio.Find(ticketId);

                    if (ticket != null)
                    {
                        if (ticket.Asesor != null && ticket.Asesor.Email != null && ticket.Asesor.Email.Length > 0)
                        {
                            message.To.Add(new MailAddress(ticket.Asesor.Email, ticket.Asesor.NombreCompleto));

                            message.Subject = string.Format(Resources.SOEmailResource.CerradoOSSubject, ticketId);

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(String.Format("{0}/{1}:", ticket.Contacto.Cliente.Nombre, ticket.Contacto.NombreCompleto));
                            sb.AppendLine("");
                            sb.AppendLine(Resources.SOEmailResource.OSCerrado);
                            sb.AppendLine(ticket.Obs);
                            sb.AppendLine("");
                            sb.AppendLine(string.Format(Resources.SOEmailResource.TicketNo, ticketId));
                            sb.AppendLine("");
                            sb.AppendLine("");
                            sb.AppendLine(Resources.SOEmailResource.Informativo);

                            message.Body = sb.ToString();

                            client.SendAsync(message, "enviar");
                        }
                    }
                }
            }
        }

        internal static void EnviarCorreoAsesorTermino(long ticketId)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;

                    OrdenServicio ticket = ctx.OrdenesServicio.Find(ticketId);

                    if (ticket != null && ticket.Contacto.Email != null && ticket.Contacto.Email.Length > 0)
                    {
                        client = new SmtpClient();
                        client.Host = paramCorreo.Host;

                        message = new MailMessage();
                        message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);
                        message.To.Add(new MailAddress(ticket.Contacto.Email, ticket.Contacto.NombreCompleto));

                        if (ticket.Asesor != null && ticket.Asesor.Email != null && ticket.Asesor.Email.Length > 0)
                        {
                            message.Bcc.Add(new MailAddress(ticket.Asesor.Email, ticket.Asesor.NombreCompleto));
                        }

                        message.Subject = string.Format(Resources.SOEmailResource.TerminadoOSSubject, ticketId);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("{0}/{1}:", ticket.Contacto.Cliente.Nombre, ticket.Contacto.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.OSMsg1);
                        sb.AppendLine(ticket.Obs);
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.TicketNo, ticketId));
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.TerminadoAviso, ticket.Asesor.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.CambioStatusSeguimiento);
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.Informativo);

                        message.Body = sb.ToString();

                        client.SendAsync(message, "enviar");

                    }
                }
            }
        }

        internal static void EnviarCorreoAsignacionAsesor(long ticketId, int asesorIdAsignado)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;

                    OrdenServicio ticket = ctx.OrdenesServicio.Find(ticketId);

                    if (ticket != null && ticket.Contacto.Email != null && ticket.Contacto.Email.Length > 0)
                    {
                        client = new SmtpClient();
                        client.Host = paramCorreo.Host;

                        message = new MailMessage();
                        message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);
                        message.To.Add(new MailAddress(ticket.Contacto.Email, ticket.Contacto.NombreCompleto));

                        if (ticket.Asesor != null && ticket.Asesor.Email != null && ticket.Asesor.Email.Length > 0)
                        {
                            message.Bcc.Add(new MailAddress(ticket.Asesor.Email, ticket.Asesor.NombreCompleto));
                        }

                        message.Subject = string.Format(Resources.SOEmailResource.AsignadoOSSubject, ticketId);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("{0}/{1}:", ticket.Contacto.Cliente.Nombre, ticket.Contacto.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.OSMsg1);
                        sb.AppendLine(ticket.Obs);
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.TicketNo, ticketId));
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.AsignadoAviso, ticket.Asesor.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.CambioStatusSeguimiento);
                        sb.AppendLine("");
                        sb.AppendLine(String.Format(Resources.SOEmailResource.ContactoDatos, ticket.Contacto.Email, ticket.Contacto.Telefono));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.Informativo);

                        message.Body = sb.ToString();

                        client.SendAsync(message, "enviar");

                    }
                }
            }
        }

        internal static void EnviarCorreoReAsignacionAsesor(long ticketId)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;

                    OrdenServicio ticket = ctx.OrdenesServicio.Find(ticketId);

                    if ((ticket != null && ticket.Contacto.Email != null && ticket.Contacto.Email.Length > 0) || 
                        (ticket.Asesor.Email != null))
                    {
                        client = new SmtpClient();
                        client.Host = paramCorreo.Host;

                        message = new MailMessage();
                        message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);

                        if (ticket.Contacto.Email != null && ticket.Contacto.Email.Length > 0)
                            message.To.Add(new MailAddress(ticket.Contacto.Email, ticket.Contacto.NombreCompleto));

                        if (ticket.Asesor != null && ticket.Asesor.Email != null && ticket.Asesor.Email.Length > 0)
                            message.To.Add(new MailAddress(ticket.Asesor.Email, ticket.Asesor.NombreCompleto));

                        message.Subject = string.Format(Resources.SOEmailResource.ReAsignadoOSSubject, ticketId);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("{0}/{1}:", ticket.Contacto.Cliente.Nombre, ticket.Contacto.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.OSMsg1);
                        sb.AppendLine(ticket.Obs);
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.TicketNo, ticketId));
                        sb.AppendLine("");
                        sb.AppendLine(string.Format(Resources.SOEmailResource.Reasignacion, ticket.Asesor.NombreCompleto));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.CambioStatusSeguimiento);
                        sb.AppendLine("");
                        sb.AppendLine(String.Format(Resources.SOEmailResource.ContactoDatos, ticket.Contacto.Email, ticket.Contacto.Telefono));
                        sb.AppendLine("");
                        sb.AppendLine(Resources.SOEmailResource.Informativo);

                        message.Body = sb.ToString();

                        client.SendAsync(message, "enviar");

                    }
                }
            }
        }

        internal static void EnviarCorreoRegistroEvento(string emailDestino, string nombreDestino, string subject)
        {
            using (OSContext ctx = new OSContext())
            {
                ParamCorreo paramCorreo = ctx.ParametrosCorreo.FirstOrDefault();

                if (paramCorreo != null)
                {
                    SmtpClient client;
                    MailMessage message;

                    client = new SmtpClient();
                    client.Host = paramCorreo.Host;

                    message = new MailMessage();
                    //message.IsBodyHtml = true;
                    message.From = new MailAddress(paramCorreo.From, Resources.SOEmailResource.From);
                    message.To.Add(new MailAddress(emailDestino, nombreDestino));
                    message.Bcc.Add(new MailAddress("gluque@zoeitcustoms.com", "Gerardo Luque"));
                    message.Bcc.Add(new MailAddress("egonzalez@zoeitcustoms.com", "Emilio Gonzalez"));
                    message.Bcc.Add(new MailAddress("ereyes@zoeitcustoms.com", "Eric Reyes"));

                    message.Subject = subject;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Gracias por registrarse a {0}",subject));
                    sb.AppendLine("");
                    sb.AppendLine("Fecha del Taller: Lunes 27 de Enero 2014, 09:00am hora del pacífico, 11:00am hora del centro");
                    sb.AppendLine("");
                    sb.AppendLine("Acceso a taller https://www.webconference.com/joinmeeting.htm");
                    sb.AppendLine("");
                    sb.AppendLine("Usuario    : USER");
                    sb.AppendLine("Contraseña : PASS");
                    sb.AppendLine("");
                    sb.AppendLine("Para mayor información favor de contactarce a los teléfonos US 408-2394487, MEX 664-6862900");
                     
                    message.Body = sb.ToString();

                    client.SendAsync(message, "enviar");
                }
            }
        }

        internal static bool CambiarMinutos(long ticketId, int minutos)
        {
            bool cambioValido = true;

            using (OSContext ctx = new OSContext())
            {
                OrdenServicio ordenServicio = ctx.OrdenesServicio.Find(ticketId);
                if (ordenServicio != null)
                {
                    ordenServicio.Minutos = minutos;
                    ctx.Entry(ordenServicio).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
            }

            return cambioValido;
        }

        internal static List<Proyecto> ObtenerProyectosPorCliente(int clienteId, bool tipoPoliza = true)
        {
            List<Proyecto> proyectos = new List<Proyecto>();

            using (OSContext ctx = new OSContext())
            {
                proyectos = ctx
                    .Proyectos
                    .Where(x => x.ClienteId == clienteId && 
                           (x.TipoProyecto.TipoPoliza.Equals("S", StringComparison.InvariantCultureIgnoreCase) || tipoPoliza == false))
                    .ToList();
            }

            return proyectos;
        }

        internal static Cliente ObtenerClientePorId(int clienteId)
        {
            using (OSContext ctx = new OSContext())
            {
                Cliente cliente = ctx.Clientes.Find(clienteId);
   
                return cliente;
            }
        }
    }

    public class OrdenServicioValidacionInfo
    {
        public bool Valido { get; set; }
        public string Mensaje { get; set; }
    }

}