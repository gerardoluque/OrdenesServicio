using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioComentarioViewModel
    {
        public long TicketId { get; set; }

        [Required(ErrorMessage = "El texto del comentario es requerido")]
        public string Comentario { get; set; }
    }
}