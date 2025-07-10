using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioAsignarAsesorViewModel
    {
        public long TicketId { get; set; }

        [Required(ErrorMessage = "El Proyecto es requerido")]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El Asesor es requerido")]
        public int AsesorId { get; set; }
    }
}