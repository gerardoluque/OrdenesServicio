using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Model.Reportes
{
    public class OrdenServicioAsesor
    {
        public int? AsesorId { get; set; }
        public string NombreCompleto { get { return Nombre + ' ' + Paterno + ' ' + (string.IsNullOrEmpty(Materno) ? "" : Materno); } }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaStatus { get; set; }
        public long Ticket { get; set; }
        public string TicketDescr { get; set; }
        public short TicketStatusId { get; set; }
        public string TicketStatusDescr { get; set; }
        public string TicketMaxStatusDescr { get; set; }
        public int? ProyectoId { get; set; }
        public string ProyectoDescr { get; set; }
        public string ProyectoTipo { get; set; }
        public string ActividadDescr { get; set; }
        public DateTime FechaActividad { get; set; }
        public DateTime? FechaMaxActividad { get; set; }
        public short TipoServicioId { get; set; }
        public string TipoServicioDescr { get; set; }
        public short ServicioId { get; set; }
        public string ServicioDescr { get; set; }
        public short ActividadStastusId { get; set; }
        public string ActividadStatusDescr { get; set; }
        public int Minutos { get; set; }

        public string ActividadNombreCompleto { get { return ActividadNombre + ' ' + ActividadPaterno + ' ' + (string.IsNullOrEmpty(ActividadMaterno) ? "" : ActividadMaterno); } }
        public string ActividadNombre { get; set; }
        public string ActividadPaterno { get; set; }
        public string ActividadMaterno { get; set; }

    }

    public class ActividadesAsesor
    {
        public int AsesorId { get; set; }
        public string NombreCompleto { get { return Nombre + ' ' + Paterno + ' ' + (string.IsNullOrEmpty(Materno) ? "" : Materno); } }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public string ActividadDescr { get; set; }
        public string ProyectoDescr { get; set; }
        public DateTime Fecha { get; set; }
        public long Ticket { get; set; }
        public short TipoServicioId { get; set; }
        public string TipoServicioDescr { get; set; }
        public short ServicioId { get; set; }
        public string ServicioDescr { get; set; }
        public int Minutos { get; set; }
        public string Horas { get; set; }
        public int HorasNumerico { get; set; }
        public int NoActividades { get; set; }
    }

    public class TiposServicioAsesor
    {
        public int AsesorId { get; set; }
        public string NombreCompleto { get { return Nombre + ' ' + Paterno + ' ' + (string.IsNullOrEmpty(Materno) ? "" : Materno); } }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public short TipoServicioId { get; set; }
        public string TipoServicioDescr { get; set; }
    }

    public class TiposServicioCliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public short TipoServicioId { get; set; }
        public string TipoServicioDescr { get; set; }
    }

    public class PolizaDatos
    {
        public int PolizaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Minutos { get; set; }
        public int ClienteId { get; set; }
        public string ClieneNombre { get; set; }
        public int MinutosConsumidos { get; set; }
        public int Saldo { get { return (Minutos - MinutosConsumidos); } }
    }

    public class PolizaSaldo
    {
        public int PolizaId { get; set; }
        public string PolizaDescr { get; set; }
        public string ProyectoTipo { get; set; }
        public DateTime Fecha { get; set; }
        public int Minutos { get; set; }
        public int ClienteId { get; set; }
        public string ClieneNombre { get; set; }

        public long Ticket { get; set; }
        public string TicketDescr { get; set; }
        public int MinutosPropuestos { get; set; }
        public DateTime TicketFecha { get; set; }
        public DateTime FechaServicio { get; set; }
        public string Actividad { get; set; }
        public short TipoServicioId { get; set; }
        public string TipoServicioDescr { get; set; }
        public short ServicioId { get; set; }
        public string ServicioDescr { get; set; }
        public int MinutosConsumidos { get; set; }
        public string ActividadStatus { get; set; }

        public int AsesorId { get; set; }
        public string AsesorNombreCompleto { get { return Nombre + ' ' + Paterno + ' ' + (string.IsNullOrEmpty(Materno) ? "" : Materno); } }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }

        public bool AfectaPoliza { get; set; }

        public int? Prioridad { get; set; }
        public int PrioridadMinutos { get; set; }
        public string PrioridadTexto
        {
            get
            {
                if (Prioridad.HasValue)
                {
                    return ((Prioridades)Prioridad.Value).ToString();
                }
                return "Sin Prioridad";
            }
        }
        public int Saldo { get { return (Minutos - MinutosConsumidos); } }
        public DateTime? FechaTerminada { get; set; }
        public bool SLACumplido
        {
            get
            {
                if (FechaTerminada == DateTime.MinValue)
                    return false;
                return FechaTerminada <= TicketFecha.AddMinutes(PrioridadMinutos);
            }
        }
        public string TicketStatusDescr { get; set; }
    }

    public class ClientePrioridades
    {
        public int PrioridadId { get; set; }
        public string Prioridad { get; set; }
        public int Horas { get; set; }
    }

    public class ClienteSLAResumen
    {
        public string SLA { get; set; } = "SLA";
        public string SLAContrato { get; set; } = "SLA en contrato";
        public string SLAPorcentaje { get; set; }
        public string SLAContratoPorcentaje { get; set; }
        public int SLACumplio { get; set; }
    }
}