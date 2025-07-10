using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;
using System.Web.Mvc;
using System.ComponentModel;

namespace ZOE.OrdenesServicio.Model.OrdenServicio
{
    public class OrdenServicioBuscarViewModel
    {
        public int Tipo { get; set; }
        public long? Ticket { get; set; }
        public string OrdenServicioDescr { get; set; }
        public int? ClienteId { get; set; }
        public int? AsesorId { get; set; }
        public int? StatusId { get; set; }

        [DisplayName("Fecha Inicial:")]
        public DateTime? FechaInicial { get; set; }
        
        [DisplayName("Fecha Final:")]
        public DateTime? FechaFinal { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Asesores { get; set; }
        public SelectList Status { get; set; }
    }
}