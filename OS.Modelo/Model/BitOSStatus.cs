using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class BitOSStatus
    {
        [Key]
        public virtual long Id { get; set; }

        /// Fecha de registro
        [Required]
        public virtual DateTime Fecha { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }

        [ForeignKey("OrdenServicio")]
        [Required]
        public long Ticket { get; set; }

        [Required]
        public virtual OrdenServicio OrdenServicio { get; set; }

        [ForeignKey("StatusAnterior")]
        public short StatusAnteriorId { get; set; }

        [Required]
        public virtual OSStatus StatusAnterior { get; set; }

        [ForeignKey("StatusCambio")]
        public short StatusCambioId { get; set; }

        [Required]
        public virtual OSStatus StatusCambio { get; set; }
    }
}
