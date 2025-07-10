using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioCambioProyectoViewModel
    {
        public long TicketId { get; set; }

        [Required(ErrorMessage = "El proyecto es requerido")]
        public short ProyectoId { get; set; }
    }
}