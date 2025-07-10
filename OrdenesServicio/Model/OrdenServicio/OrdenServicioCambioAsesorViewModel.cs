using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioCambioAsesorViewModel
    {
        public long TicketId { get; set; }

        [Required(ErrorMessage = "El Asesor es requerido")]
        public short AsesorId { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }

    }
}