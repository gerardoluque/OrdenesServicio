using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class BitOSReasignacion
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

        [ForeignKey("Status")]
        public short StatusId { get; set; }

        [Required]
        public virtual OSStatus Status { get; set; }

        [ForeignKey("AsesorAnterior")]
        public int AsesorAnteriorId { get; set; }

        [Required]
        public virtual Asesor AsesorAnterior { get; set; }

        [ForeignKey("AsesorCambio")]
        public int AsesorCambioId { get; set; }

        [Required]
        public virtual Asesor AsesorCambio { get; set; }
    }
}
