using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ZOE.OrdenesServicio.Model.Reportes;
using ZOE.OS.Modelo;
using System.Data.Objects;
using System.Security.Cryptography;

namespace ZOE.OrdenesServicio.Negocio
{
    public class ReporteBC
    {

        public List<ActividadesAsesor> ObtenerTotalHorasAsesoresPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            #region ObtenerTotalHorasAsesoresPorFecha
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<ActividadesAsesor> actividades = new List<ActividadesAsesor>();

            using (OSContext ctx = new OSContext())
            {
                var qAsesores = (from act in ctx.DetallesOrdenServicio.Include("Asesor")
                                 orderby act.Asesor.Nombre, act.Asesor.Paterno, act.Asesor.Materno
                                 where act.FechaAbierto >= fechaInicial && act.FechaAbierto <= fechaFinal && act.StatusId != 5
                                 group act by act.AsesorId into g
                                 select new
                                 {
                                     AsesorId = g.Key,
                                     Data = g,
                                     Minutos = g.Sum(p => p.Minutos)
                                 });

                foreach (var asesor in qAsesores)
                {
                    var actividad = asesor.Data.FirstOrDefault(p => p.AsesorId == asesor.AsesorId);

                    actividades.Add(new ActividadesAsesor
                    {
                        AsesorId = asesor.AsesorId,
                        Nombre = actividad.Asesor.Nombre,
                        Paterno = actividad.Asesor.Paterno,
                        Materno = actividad.Asesor.Materno,
                        Minutos = asesor.Minutos,
                        Horas = TimeSpan.FromSeconds(asesor.Minutos).ToString()
                    });
                }
            }

            return actividades;
            #endregion
        }

        public List<ActividadesAsesor> ObtenerTotalHorasPorDiaAsesoresPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            #region ObtenerTotalHorasPorDiaAsesoresPorFecha
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<ActividadesAsesor> actividades = new List<ActividadesAsesor>();

            using (OSContext ctx = new OSContext())
            {
                var qAsesores = (from act in ctx.DetallesOrdenServicio.Include("Asesor")
                                 orderby act.Asesor.Nombre, act.Asesor.Paterno, act.Asesor.Materno, act.FechaAbierto
                                 where act.FechaAbierto >= fechaInicial && act.FechaAbierto <= fechaFinal && act.StatusId != 5
                                 group act by new { act.AsesorId, FechaAbierto = EntityFunctions.TruncateTime(act.FechaAbierto) } into g
                                 select new
                                 {
                                     AsesorId = g.Key.AsesorId,
                                     Fecha = g.Key.FechaAbierto,
                                     Data = g,
                                     NoActividades = g.Count(),
                                     Minutos = g.Sum(p => p.Minutos)
                                 });

                foreach (var asesor in qAsesores)
                {
                    var actividad = asesor.Data.FirstOrDefault(p => p.AsesorId == asesor.AsesorId);

                    actividades.Add(new ActividadesAsesor
                    {
                        AsesorId = asesor.AsesorId,
                        Nombre = actividad.Asesor.Nombre,
                        Paterno = actividad.Asesor.Paterno,
                        Materno = actividad.Asesor.Materno,
                        Fecha = asesor.Fecha.Value,
                        NoActividades = asesor.NoActividades,
                        Minutos = asesor.Minutos,
                        Horas = TimeSpan.FromSeconds(asesor.Minutos).ToString()
                    });
                }
            }

            return actividades;
            #endregion
        }

        public List<ActividadesAsesor> ObtenerActividadesAsesoresPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            #region ObtenerActividadesAsesoresPorFecha
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<ActividadesAsesor> actividades = new List<ActividadesAsesor>();

            using (OSContext ctx = new OSContext())
            {
                var qAsesores = (from act in ctx.DetallesOrdenServicio.Include("Asesor")
                                 orderby act.Asesor.Nombre, act.Asesor.Paterno, act.Asesor.Materno, act.FechaAbierto
                                 where act.FechaAbierto >= fechaInicial && act.FechaAbierto <= fechaFinal && act.StatusId != 5
                                 select new ActividadesAsesor
                                 {
                                     AsesorId = act.AsesorId,
                                     Ticket = act.Ticket,
                                     Nombre = act.Asesor.Nombre,
                                     Paterno = act.Asesor.Paterno,
                                     Materno = act.Asesor.Materno,
                                     ClienteNombre = act.Contacto.Cliente.Nombre,
                                     Fecha = act.FechaAbierto,
                                     ProyectoDescr = act.OrdenServicio.Proyecto.Descr,
                                     ServicioDescr = act.Servicio.ServicioDescr,
                                     TipoServicioDescr = act.TipoServicio.TipoServicioDescr,
                                     ActividadDescr = act.DetalleDescr,
                                     Minutos = act.Minutos
                                 });

                actividades = qAsesores.ToList();

                //foreach (var actividad in actividades)
                //{
                //    actividad.Horas = TimeSpan.FromSeconds(actividad.Minutos).ToString();
                //}

            }

            return actividades;
            #endregion
        }

        public List<OrdenServicioAsesor> ObtenerOrdenesServicioPorAsesor(short asesorId)
        {
            #region ObtenerOrdenesServicioPorAsesor
            List<OrdenServicioAsesor> ordenesServicio = new List<OrdenServicioAsesor>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<OrdenServicioAsesor> qOS = (from os in ctx.DetallesOrdenServicio.Include("OrdenServicio")
                                                       where os.OrdenServicio.AsesorId == asesorId && os.AsesorId == asesorId
                                                       orderby os.OrdenServicio.Fecha
                                                       select new OrdenServicioAsesor
                                                       {
                                                           Ticket = os.Ticket,
                                                           Fecha = os.OrdenServicio.Fecha,
                                                           Nombre = os.Asesor.Nombre,
                                                           Paterno = os.Asesor.Paterno,
                                                           Materno = os.Asesor.Materno,
                                                           TicketStatusId = os.OrdenServicio.OSStatusId,
                                                           TicketStatusDescr = os.OrdenServicio.Status.Descr,
                                                           ClienteNombre = os.Contacto.Cliente.Nombre,
                                                           ProyectoId = os.OrdenServicio.ProyectoId.Value,
                                                           ProyectoDescr = os.OrdenServicio.Proyecto.Descr,
                                                           ProyectoTipo = os.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
                                                           ActividadDescr = os.DetalleDescr,
                                                           FechaActividad = os.FechaAbierto,
                                                           ActividadStastusId = os.StatusId,
                                                           ActividadStatusDescr = os.StatusDescripcion,
                                                           ServicioId = os.ServicioId,
                                                           ServicioDescr = os.ServicioDescripcion,
                                                           TipoServicioId = os.TipoServicioId,
                                                           TipoServicioDescr = os.TipoServicioDescripcion,
                                                           Minutos = os.Minutos
                                                       });

                ordenesServicio = qOS.ToList();
            }

            return ordenesServicio;
            #endregion
        }

        //public List<OrdenServicioAsesor> ObtenerOrdenesServicioPorAsesor(short asesorId, short statusId, DateTime fechaInicial, DateTime fechaFinal, bool sinRangoFecha)
        //{
        //    System.Linq.Expressions.Expression<Func<OrdenServicio, bool>> expresion = p => p.AsesorId.HasValue;
        //    List<OrdenServicioAsesor> ordenesServicio = new List<OrdenServicioAsesor>();

        //    if (asesorId != 0)
        //    {
        //        expresion = p => p.AsesorId == asesorId;
        //        if (!sinRangoFecha)
        //        {
        //            if (statusId == 0)
        //                expresion = p => p.AsesorId == asesorId && (p.Fecha >= fechaInicial && p.Fecha <= fechaFinal);
        //            else
        //                expresion = p => p.AsesorId == asesorId && p.OSStatusId == statusId && (p.Fecha >= fechaInicial && p.Fecha <= fechaFinal);
        //        }
        //        else
        //            if (statusId != 0)
        //                expresion = p => p.AsesorId == asesorId && p.OSStatusId == statusId;
        //    }
        //    else
        //    {
        //        if (!sinRangoFecha)
        //        {
        //            if (statusId == 0)
        //                expresion = p => p.Fecha >= fechaInicial && p.Fecha <= fechaFinal;
        //            else
        //                expresion = p => p.OSStatusId == statusId && (p.Fecha >= fechaInicial && p.Fecha <= fechaFinal);
        //        }
        //        else
        //            if (statusId != 0)
        //                expresion = p => p.OSStatusId == statusId;
        //    }

        //    using (OSContext ctx = new OSContext())
        //    {
        //        IQueryable<OrdenServicioAsesor> qOS = (ctx.OrdenesServicio
        //            .Include("OrdenesServicio")
        //            .Include("Contacto")
        //            .Include("Proyecto")
        //            .Where(expresion)
        //            .OrderBy(o=>o.Asesor.Nombre).ThenBy(o=>o.Asesor.Paterno).ThenBy(o=>o.Asesor.Materno).ThenBy(o=>o.Fecha)
        //            .Select(p => new OrdenServicioAsesor
        //                                               {
        //                                                   Ticket = p.Ticket,
        //                                                   TicketDescr = p.Obs,
        //                                                   Fecha = p.Fecha,
        //                                                   AsesorId = p.AsesorId.Value,
        //                                                   Nombre = p.Asesor.Nombre,
        //                                                   Paterno = p.Asesor.Paterno,
        //                                                   Materno = p.Asesor.Materno,
        //                                                   TicketStatusId = p.OSStatusId,
        //                                                   TicketStatusDescr = p.Status.Descr,
        //                                                   ClienteId = p.Contacto.ClienteId,
        //                                                   ClienteNombre = p.Contacto.Cliente.Nombre,
        //                                                   ProyectoId = p.ProyectoId,
        //                                                   ProyectoDescr = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.Descr,
        //                                                   ProyectoTipo = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.TipoProyecto.TipoProyectoDescr
        //                                               }));

        //        ordenesServicio = qOS.ToList();
        //    }

        //    return ordenesServicio;
        //}

        public List<OrdenServicioAsesor> ObtenerOrdenesServicioPorDia(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<OrdenServicioAsesor> ordenesServicio = new List<OrdenServicioAsesor>();

            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (OSContext ctx = new OSContext())
            {
                var qOS = (ctx.OrdenesServicio
                            .Include(i => i.OSDetalles)
                            .Where(p => p.Fecha >= fechaInicial && p.Fecha <= fechaFinal)
                            .OrderBy(o => o.Asesor.Nombre).ThenBy(o => o.Asesor.Paterno).ThenBy(o => o.Asesor.Materno).ThenBy(o => o.Fecha)).ToList();


                foreach (var p in qOS)
                {
                    var lastBit = ctx.BitacoraStatusOrdenServicio
                        .Where(b => b.Ticket == p.Ticket && (b.Fecha >= fechaInicial && b.Fecha <= fechaFinal))
                        .OrderByDescending(g => g.Fecha)
                        .Select(s => new { FechaMax = s.Fecha, StatusIdMax = s.StatusCambioId }).FirstOrDefault();

                    DateTime? fechaActMax = null;

                    if (p.OSDetalles.Any())
                    {
                        fechaActMax = p.OSDetalles.Where(b => b.Ticket == p.Ticket).Max(m => m.FechaRegistro);

                        foreach (var detalle in p.OSDetalles)
                        {
                            var nuevaOS = new OrdenServicioAsesor
                            {
                                Ticket = p.Ticket,
                                TicketDescr = p.Obs,
                                Fecha = p.Fecha,
                                FechaStatus = lastBit.FechaMax,
                                TicketMaxStatusDescr = ctx.OSStatus.Find(lastBit.StatusIdMax).Descr,
                                AsesorId = p.AsesorId.Value,
                                Nombre = p.Asesor.Nombre,
                                Paterno = p.Asesor.Paterno,
                                Materno = p.Asesor.Materno,
                                TicketStatusId = p.OSStatusId,
                                TicketStatusDescr = p.Status.Descr,
                                ClienteId = p.Contacto.ClienteId,
                                ClienteNombre = p.Contacto.Cliente.Nombre,
                                ProyectoId = p.ProyectoId,
                                ProyectoDescr = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.Descr,
                                ProyectoTipo = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.TipoProyecto.TipoProyectoDescr,
                                ActividadDescr = detalle.DetalleDescr,
                                FechaActividad = detalle.FechaAbierto,
                                FechaMaxActividad = fechaActMax,
                                ActividadStastusId = detalle.StatusId,
                                ActividadStatusDescr = detalle.Status.OSDetalleSTDescr,
                                ServicioId = detalle.ServicioId,
                                ServicioDescr = detalle.Servicio.ServicioDescr,
                                TipoServicioId = detalle.TipoServicioId,
                                TipoServicioDescr = detalle.TipoServicio.TipoServicioDescr,
                                Minutos = detalle.Minutos,
                                ActividadNombre = detalle.Asesor.Nombre,
                                ActividadPaterno = detalle.Asesor.Paterno,
                                ActividadMaterno = detalle.Asesor.Materno
                            };
                            ordenesServicio.Add(nuevaOS);
                        }
                    }
                    else
                    {
                        var nuevaOS = new OrdenServicioAsesor
                        {
                            Ticket = p.Ticket,
                            TicketDescr = p.Obs,
                            Fecha = p.Fecha,
                            FechaStatus = lastBit.FechaMax,
                            TicketMaxStatusDescr = ctx.OSStatus.Find(lastBit.StatusIdMax).Descr,
                            AsesorId = p.Asesor == null ? 0 : p.AsesorId.Value,
                            Nombre = p.Asesor == null ? "Sin Asesor Asignado" : p.Asesor.Nombre,
                            Paterno = p.Asesor == null ? "" : p.Asesor.Paterno,
                            Materno = p.Asesor == null ? "" : p.Asesor.Materno,
                            TicketStatusId = p.OSStatusId,
                            TicketStatusDescr = p.Status.Descr,
                            ClienteId = p.Contacto.ClienteId,
                            ClienteNombre = p.Contacto.Cliente.Nombre,
                            ProyectoId = p.ProyectoId,
                            ProyectoDescr = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.Descr,
                            ProyectoTipo = p.Proyecto == null ? "Sin Proyecto" : p.Proyecto.TipoProyecto.TipoProyectoDescr,
                            FechaMaxActividad = fechaActMax
                        };
                        ordenesServicio.Add(nuevaOS);
                    }
                }
            }

            return ordenesServicio;
        }


        public List<OrdenServicioAsesor> ObtenerOrdenesServicioPorAsesor(short asesorId, short statusId, DateTime fechaInicial, DateTime fechaFinal, bool sinRangoFecha)
        {
            System.Linq.Expressions.Expression<Func<OSDetalle, bool>> expresion = p => p.OrdenServicio.AsesorId.HasValue;
            List<OrdenServicioAsesor> ordenesServicio = new List<OrdenServicioAsesor>();

            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (asesorId != 0)
            {
                expresion = p => p.OrdenServicio.AsesorId == asesorId;
                if (!sinRangoFecha)
                {
                    if (statusId == 0)
                        expresion = p => p.OrdenServicio.AsesorId == asesorId && (p.OrdenServicio.Fecha >= fechaInicial && p.OrdenServicio.Fecha <= fechaFinal) && p.OrdenServicio.AsesorId.HasValue;
                    else
                        expresion = p => p.OrdenServicio.AsesorId == asesorId && p.OrdenServicio.OSStatusId == statusId && (p.OrdenServicio.Fecha >= fechaInicial && p.OrdenServicio.Fecha <= fechaFinal) && p.OrdenServicio.AsesorId.HasValue;
                }
                else
                    if (statusId != 0)
                    expresion = p => p.OrdenServicio.AsesorId == asesorId && p.OrdenServicio.OSStatusId == statusId && p.OrdenServicio.AsesorId.HasValue;
            }
            else
            {
                if (!sinRangoFecha)
                {
                    if (statusId == 0)
                        expresion = p => p.OrdenServicio.Fecha >= fechaInicial && p.OrdenServicio.Fecha <= fechaFinal && p.OrdenServicio.AsesorId.HasValue;
                    else
                        expresion = p => p.OrdenServicio.OSStatusId == statusId && (p.OrdenServicio.Fecha >= fechaInicial && p.OrdenServicio.Fecha <= fechaFinal) && p.OrdenServicio.AsesorId.HasValue;
                }
                else
                    if (statusId != 0)
                    expresion = p => p.OrdenServicio.OSStatusId == statusId && p.OrdenServicio.AsesorId.HasValue;
            }

            using (OSContext ctx = new OSContext())
            {
                IQueryable<OrdenServicioAsesor> qOS = (ctx.DetallesOrdenServicio
                    .Include("OrdenesServicio")
                    .Where(expresion)
                    .OrderBy(o => o.OrdenServicio.Asesor.Nombre).ThenBy(o => o.OrdenServicio.Asesor.Paterno).ThenBy(o => o.OrdenServicio.Asesor.Materno).ThenBy(o => o.OrdenServicio.Fecha).ThenBy(o => o.FechaAbierto)
                    .Select(p => new OrdenServicioAsesor
                    {
                        Ticket = p.Ticket,
                        TicketDescr = p.OrdenServicio.Obs,
                        Fecha = p.OrdenServicio.Fecha,
                        AsesorId = p.OrdenServicio.AsesorId.Value,
                        Nombre = p.OrdenServicio.Asesor.Nombre,
                        Paterno = p.OrdenServicio.Asesor.Paterno,
                        Materno = p.OrdenServicio.Asesor.Materno,
                        TicketStatusId = p.OrdenServicio.OSStatusId,
                        TicketStatusDescr = p.OrdenServicio.Status.Descr,
                        ClienteId = p.OrdenServicio.Contacto.ClienteId,
                        ClienteNombre = p.OrdenServicio.Contacto.Cliente.Nombre,
                        ProyectoId = p.OrdenServicio.ProyectoId,
                        ProyectoDescr = p.OrdenServicio.Proyecto == null ? "Sin Proyecto" : p.OrdenServicio.Proyecto.Descr,
                        ProyectoTipo = p.OrdenServicio.Proyecto == null ? "Sin Proyecto" : p.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
                        ActividadDescr = p.DetalleDescr,
                        FechaActividad = p.FechaAbierto,
                        ActividadStastusId = p.StatusId,
                        ActividadStatusDescr = p.Status.OSDetalleSTDescr,
                        ServicioId = p.ServicioId,
                        ServicioDescr = p.Servicio.ServicioDescr,
                        TipoServicioId = p.TipoServicioId,
                        TipoServicioDescr = p.TipoServicio.TipoServicioDescr,
                        Minutos = p.Minutos,
                        ActividadNombre = p.Asesor.Nombre,
                        ActividadPaterno = p.Asesor.Paterno,
                        ActividadMaterno = p.Asesor.Materno
                    }));

                ordenesServicio = qOS.ToList();
            }

            return ordenesServicio;
        }


        public List<ActividadesAsesor> ObtenerActividadesPorAsesor()
        {
            List<ActividadesAsesor> tiposServicio = new List<ActividadesAsesor>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<ActividadesAsesor> qTipos = (from act in ctx.DetallesOrdenServicio.Include("Asesor").Include("TipoServicio").Include("Servicio")
                                                        orderby act.Asesor.Nombre, act.Asesor.Paterno, act.Asesor.Materno, act.Contacto.Cliente.Nombre, act.FechaAbierto
                                                        select new ActividadesAsesor
                                                        {
                                                            AsesorId = act.AsesorId,
                                                            Nombre = act.Asesor.Nombre,
                                                            Paterno = act.Asesor.Paterno,
                                                            Materno = act.Asesor.Materno,
                                                            ClienteId = act.Contacto.ClienteId,
                                                            ClienteNombre = act.Contacto.Cliente.Nombre,
                                                            TipoServicioId = act.TipoServicioId,
                                                            TipoServicioDescr = act.TipoServicio.TipoServicioDescr,
                                                            ServicioId = act.ServicioId,
                                                            ServicioDescr = act.Servicio.ServicioDescr,
                                                            Ticket = act.Ticket,
                                                            Fecha = act.FechaAbierto,
                                                            Minutos = act.Minutos
                                                        });

                tiposServicio = qTipos.ToList();
            }

            return tiposServicio;
        }

        public List<TiposServicioAsesor> ObtenerTiposServicioPorAsesor()
        {
            List<TiposServicioAsesor> tiposServicio = new List<TiposServicioAsesor>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<TiposServicioAsesor> qTipos = (from act in ctx.DetallesOrdenServicio.Include("Asesor").Include("TipoServicio")
                                                          select new TiposServicioAsesor { AsesorId = act.AsesorId, Nombre = act.Asesor.Nombre, Paterno = act.Asesor.Paterno, Materno = act.Asesor.Materno, TipoServicioId = act.TipoServicioId, TipoServicioDescr = act.TipoServicio.TipoServicioDescr });

                tiposServicio = qTipos.ToList();
            }

            return tiposServicio;
        }

        public List<TiposServicioCliente> ObtenerTiposServicioPorCliente()
        {
            List<TiposServicioCliente> tiposServicio = new List<TiposServicioCliente>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<TiposServicioCliente> qTipos = (from act in ctx.DetallesOrdenServicio.Include("Cliente").Include("TipoServicio")
                                                           select new TiposServicioCliente { ClienteId = act.Contacto.ClienteId, Nombre = act.Contacto.Cliente.Nombre, TipoServicioId = act.TipoServicioId, TipoServicioDescr = act.TipoServicio.TipoServicioDescr });

                tiposServicio = qTipos.ToList();
            }

            return tiposServicio;
        }

        public List<PolizaDatos> ObtenerPolizas()
        {
            List<PolizaDatos> polizas = new List<PolizaDatos>();

            // using (OSContext ctx = new OSContext())
            //{
            //    var qPol = from detOS in ctx.DetallesOrdenServicio
            //               where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo
            //               group detOS by new { PolizaId = detOS.OrdenServicio.Proyecto.ProyectoId }  into g
            //               select new
            //               {
            //                   PolizaID = g.Key,
            //                   Data = g,
            //                   TotalMinutos = g.Sum(p => p.Minutos)
            //               };

            //    foreach (var poliza in qPol.ToList())
            //    {
            //        var detalle = poliza.Data.FirstOrDefault(p => p.OrdenServicio.Proyecto.ProyectoId == poliza.PolizaID.PolizaId);
            //        polizas.Add(new PolizaDatos
            //        {
            //            PolizaId = poliza.PolizaID.PolizaId,
            //            ClienteId = detalle.Contacto.ClienteId,
            //            ClieneNombre = detalle.Contacto.Cliente.Nombre,
            //            Fecha = detalle.OrdenServicio.Proyecto.Fecha,
            //            Minutos = detalle.OrdenServicio.Proyecto.Minutos,
            //            MinutosConsumidos = poliza.TotalMinutos
            //        });
            //    }

            // }

            return polizas;
        }

        public List<PolizaDatos> ObtenerPolizasSaldoPorAgotar(int saldoMin, int saldoMax, bool negativos)
        {
            #region ObtenerPolizasSaldoPorAgotar
            List<PolizaDatos> polizas = new List<PolizaDatos>();

            using (OSContext ctx = new OSContext())
            {
                var qPol = from detOS in ctx.DetallesOrdenServicio
                           where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                 detOS.OrdenServicio.Proyecto.TipoProyecto.TipoPoliza == "S" &&
                                 detOS.StatusId != 5 &&
                                 detOS.TipoServicio.AfectaPoliza == "S"
                           group detOS by new { PolizaId = detOS.OrdenServicio.Proyecto.ProyectoId } into g
                           select new
                           {
                               PolizaID = g.Key,
                               Data = g,
                               TotalMinutos = g.Sum(p => p.Minutos)
                           };

                foreach (var poliza in qPol.ToList())
                {
                    var detalle = poliza.Data.FirstOrDefault(p => p.OrdenServicio.Proyecto.ProyectoId == poliza.PolizaID.PolizaId);
                    int saldo;

                    saldo = (detalle.OrdenServicio.Proyecto.Abonos.Any() ? detalle.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0) - poliza.TotalMinutos;

                    if ((saldo >= saldoMin && saldo <= saldoMax) || (negativos && saldo < 0))
                    {
                        polizas.Add(new PolizaDatos
                        {
                            PolizaId = poliza.PolizaID.PolizaId,
                            ClienteId = detalle.Contacto.ClienteId,
                            ClieneNombre = detalle.Contacto.Cliente.Nombre,
                            Fecha = detalle.OrdenServicio.Proyecto.Fecha,
                            Minutos = detalle.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos),
                            MinutosConsumidos = poliza.TotalMinutos
                        });
                    }
                }

            }

            return polizas;
            #endregion
        }

        public List<PolizaDatos> ObtenerPolizasSaldo(int clienteId)
        {
            #region ObtenerPolizasSaldo
            List<PolizaDatos> polizas = new List<PolizaDatos>();

            using (OSContext ctx = new OSContext())
            {
                var qPol = from detOS in ctx.DetallesOrdenServicio
                           orderby detOS.Contacto.Cliente.Nombre
                           where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                 detOS.OrdenServicio.Proyecto.TipoProyecto.TipoPoliza == "S" &&
                                 detOS.TipoServicio.AfectaPoliza == "S" &&
                                 detOS.StatusId != 5 &&
                                 (detOS.OrdenServicio.Contacto.ClienteId == clienteId || clienteId == 0)
                           group detOS by new { PolizaId = detOS.OrdenServicio.Proyecto.ProyectoId } into g
                           select new
                           {
                               PolizaID = g.Key,
                               Data = g,
                               TotalMinutos = g.Sum(p => p.Minutos)
                           };

                foreach (var poliza in qPol.ToList())
                {
                    var detalle = poliza.Data.FirstOrDefault(p => p.OrdenServicio.Proyecto.ProyectoId == poliza.PolizaID.PolizaId);
                    int saldo;

                    saldo = (detalle.OrdenServicio.Proyecto.Abonos.Any() ? detalle.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0) - poliza.TotalMinutos;

                    polizas.Add(new PolizaDatos
                    {
                        PolizaId = poliza.PolizaID.PolizaId,
                        ClienteId = detalle.Contacto.ClienteId,
                        ClieneNombre = detalle.Contacto.Cliente.Nombre,
                        Fecha = detalle.OrdenServicio.Proyecto.Fecha,
                        Minutos = detalle.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos),
                        MinutosConsumidos = poliza.TotalMinutos
                    });
                }

            }

            return polizas;
            #endregion
        }

        public List<PolizaSaldo> ObtenerTicketsDetalleMinutos(int clienteId, int proyectoId)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.StatusId != 5 &&
                                                      ((detOS.OrdenServicio.Contacto.ClienteId == clienteId || clienteId == 0) &&
                                                       (detOS.OrdenServicio.ProyectoId == proyectoId || proyectoId == 0))
                                                orderby detOS.Contacto.ClienteId, detOS.OrdenServicio.ProyectoId
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    TicketFecha = detOS.OrdenServicio.Fecha,
                                                    MinutosPropuestos = detOS.OrdenServicio.Minutos.HasValue ? detOS.OrdenServicio.Minutos.Value : 0,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    TicketDescr = detOS.OrdenServicio.Obs,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    ActividadStatus = detOS.Status.OSDetalleSTDescr,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerPolizaDetalleServicios(int clienteId, DateTime fechaInicial, DateTime fechaFinal, bool sinRangoFecha)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.OrdenServicio.Proyecto.TipoProyecto.TipoPoliza == "S" &&
                                                      detOS.TipoServicio.AfectaPoliza == "S" &&
                                                      detOS.StatusId != 5 &&
                                                      ((detOS.OrdenServicio.Contacto.ClienteId == clienteId || clienteId == 0) &&
                                                       ((detOS.FechaAbierto >= fechaInicial && detOS.FechaAbierto <= fechaFinal) || sinRangoFecha))
                                                orderby detOS.Contacto.ClienteId
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    TicketFecha = detOS.OrdenServicio.Fecha,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    TicketDescr = detOS.OrdenServicio.Obs,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerPolizaDetalleServiciosPorCliente(int clienteId)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.OrdenServicio.Proyecto.TipoProyecto.TipoPoliza == "S" &&
                                                      detOS.TipoServicio.AfectaPoliza == "S" &&
                                                      detOS.StatusId != 5 &&
                                                      detOS.OrdenServicio.Contacto.ClienteId == clienteId
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    TicketDescr = detOS.OrdenServicio.Obs,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerProyectoDetalleServicios()
        {
            return ObtenerProyectoDetalleServicios(0);
            //List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            //using (OSContext ctx = new OSContext())
            //{
            //    IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
            //                                    where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo  
            //                                    orderby detOS.Contacto.Cliente.Nombre, detOS.OrdenServicio.Proyecto.Fecha
            //                                    select new PolizaSaldo
            //                                    {
            //                                        PolizaId = detOS.OrdenServicio.ProyectoId.Value,
            //                                        PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
            //                                        ProyectoTipo = detOS.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
            //                                        Fecha = detOS.OrdenServicio.Proyecto.Fecha,
            //                                        Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
            //                                        ClienteId = detOS.Contacto.ClienteId,
            //                                        ClieneNombre = detOS.Contacto.Cliente.Nombre,
            //                                        AsesorId = detOS.AsesorId,
            //                                        Nombre = detOS.Asesor.Nombre,
            //                                        Paterno = detOS.Asesor.Paterno,
            //                                        Materno = detOS.Asesor.Materno,
            //                                        Ticket = detOS.Ticket,
            //                                        FechaServicio = detOS.FechaAbierto,
            //                                        MinutosConsumidos = detOS.Minutos,
            //                                        Actividad = detOS.DetalleDescr,
            //                                        ServicioId = detOS.ServicioId,
            //                                        ServicioDescr = detOS.Servicio.ServicioDescr,
            //                                        TipoServicioId = detOS.TipoServicioId,
            //                                        TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
            //                                        AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
            //                                    });

            //    polizas = qPol.ToList();
            //}

            //return polizas;
        }

        public List<PolizaSaldo> ObtenerProyectoDetalleServicios(int proyectoId)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.StatusId != 5 &&
                                                      (detOS.OrdenServicio.ProyectoId == proyectoId || proyectoId == 0)
                                                orderby detOS.Contacto.Cliente.Nombre, detOS.OrdenServicio.Proyecto.Fecha
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
                                                    ProyectoTipo = detOS.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerProyectosDetalleServiciosPorCliente(int clienteId, DateTime fechaInicial, DateTime fechaFinal, bool sinRangoFecha)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.StatusId != 5 &&
                                                      ((detOS.OrdenServicio.Contacto.ClienteId == clienteId || clienteId == 0) &&
                                                       ((detOS.FechaAbierto >= fechaInicial && detOS.FechaAbierto <= fechaFinal) || sinRangoFecha))
                                                orderby detOS.Contacto.Cliente.Nombre, detOS.OrdenServicio.Proyecto.Fecha
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
                                                    ProyectoTipo = detOS.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerProyectoDetalleServiciosPorCliente(int clienteId)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            using (OSContext ctx = new OSContext())
            {
                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.StatusId != 5 && (detOS.OrdenServicio.Contacto.ClienteId == clienteId || clienteId == 0)
                                                orderby detOS.Contacto.Cliente.Nombre, detOS.OrdenServicio.Proyecto.Fecha
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId.Value,
                                                    PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
                                                    ProyectoTipo = detOS.OrdenServicio.Proyecto.TipoProyecto.TipoProyectoDescr,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    Minutos = detOS.OrdenServicio.Proyecto.Abonos.Any() ? detOS.OrdenServicio.Proyecto.Abonos.Sum(p => p.Minutos) : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S")
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<PolizaSaldo> ObtenerTicketsDetallePrioridad(int clienteId, int proyectoId, int asesorId,
            DateTime fechaInicial, DateTime fechaFinal, int ticket, bool sinRangoFecha)
        {
            List<PolizaSaldo> polizas = new List<PolizaSaldo>();

            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (OSContext ctx = new OSContext())
            {
                var t = ((from prio in ctx.ClientePrioridades
                          where prio.ClienteId == 2 &&
                                prio.Prioridad == 3
                          select prio.Horas).FirstOrDefault() * 60);

                IQueryable<PolizaSaldo> qPol = (from detOS in ctx.DetallesOrdenServicio.Include("OrdenServicio").Include("Asesor")
                                                where detOS.OrdenServicio.Proyecto.Status == (short)Proyectotatus.Activo &&
                                                      detOS.StatusId != (short)OrdenServicioStatus.Cancelada &&
                                                      detOS.OrdenServicio.Contacto.ClienteId == clienteId &&
                                                      ((detOS.OrdenServicio.AsesorId == asesorId && detOS.AsesorId == asesorId) || asesorId == 0) &&
                                                      (detOS.OrdenServicio.ProyectoId == proyectoId || 
                                                       (proyectoId == 0 && detOS.OrdenServicio.Proyecto.TipoProyecto.TipoPoliza.Equals("S", StringComparison.InvariantCultureIgnoreCase))) &&
                                                      (detOS.OrdenServicio.Ticket == ticket || ticket == 0) &&
                                                      ((detOS.OrdenServicio.Fecha >= fechaInicial &&
                                                        detOS.OrdenServicio.Fecha <= fechaFinal) || sinRangoFecha)
                                                orderby detOS.OrdenServicio.ProyectoId,
                                                        detOS.OrdenServicio.Fecha,
                                                        detOS.OrdenServicio.Ticket
                                                select new PolizaSaldo
                                                {
                                                    PolizaId = detOS.OrdenServicio.ProyectoId ?? 0,
                                                    PolizaDescr = detOS.OrdenServicio.Proyecto.Descr,
                                                    Fecha = detOS.OrdenServicio.Proyecto.Fecha,
                                                    TicketFecha = detOS.OrdenServicio.Fecha,
                                                    MinutosPropuestos = detOS.OrdenServicio.Minutos.HasValue ? detOS.OrdenServicio.Minutos.Value : 0,
                                                    ClienteId = detOS.Contacto.ClienteId,
                                                    ClieneNombre = detOS.Contacto.Cliente.Nombre,
                                                    AsesorId = detOS.AsesorId,
                                                    Nombre = detOS.Asesor.Nombre,
                                                    Paterno = detOS.Asesor.Paterno,
                                                    Materno = detOS.Asesor.Materno,
                                                    Ticket = detOS.Ticket,
                                                    TicketDescr = detOS.OrdenServicio.Obs,
                                                    TicketStatusDescr = detOS.OrdenServicio.Status.Descr,
                                                    FechaServicio = detOS.FechaAbierto,
                                                    MinutosConsumidos = detOS.Minutos,
                                                    Minutos = detOS.OrdenServicio.Minutos ?? 0,
                                                    ActividadStatus = detOS.Status.OSDetalleSTDescr,
                                                    Actividad = detOS.DetalleDescr,
                                                    ServicioId = detOS.ServicioId,
                                                    ServicioDescr = detOS.Servicio.ServicioDescr,
                                                    TipoServicioId = detOS.TipoServicioId,
                                                    TipoServicioDescr = detOS.TipoServicio.TipoServicioDescr,
                                                    AfectaPoliza = (detOS.TipoServicio.AfectaPoliza == "S"),
                                                    Prioridad = detOS.OrdenServicio.Prioridad,
                                                    FechaTerminada = (from bit in ctx.BitacoraStatusOrdenServicio
                                                                      where bit.Ticket == detOS.Ticket &&
                                                                      (bit.StatusCambioId == (short)OrdenServicioStatus.Terminada ||
                                                                       bit.StatusCambioId == (short)OrdenServicioStatus.Cerrada)
                                                                      select bit.Fecha).Max(),
                                                    PrioridadMinutos = ((from prio in ctx.ClientePrioridades
                                                                         where prio.ClienteId == detOS.Contacto.ClienteId &&
                                                                               prio.Prioridad == detOS.OrdenServicio.Prioridad
                                                                         select prio.Horas)
                                                                              .FirstOrDefault() * 60)
                                                });

                polizas = qPol.ToList();
            }

            return polizas;
        }

        public List<ClientePrioridades> ObtenerTicketPrioridades(int clienteId)
        {
            var ticketPrioridades = new List<ClientePrioridades>();

            using (OSContext ctx = new OSContext())
            {
                var ticktPrio = ctx
                    .ClientePrioridades
                    .Where(t => t.ClienteId == clienteId)
                    .OrderBy(o => o.Prioridad)
                    .ToList();

                foreach (var tp in ticktPrio)
                {
                    ticketPrioridades.Add(new ClientePrioridades 
                                              { 
                                                PrioridadId = tp.Prioridad,
                                                Prioridad = Enum.GetName(typeof(Prioridades), tp.Prioridad), 
                                                Horas = tp.Horas 
                                              });
                }

                return ticketPrioridades;
            }
        }
    }
}