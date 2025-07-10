using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class BitOSDetalleStatus
    {
        [Key]
        public virtual long Id { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }

        /// Fecha de registro
        [Required]
        public virtual DateTime Fecha { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        //[ForeignKey("OSDetalle")]
        //[Required]
        //public long OSDetalleId { get; set; }

        [Required]
        public virtual OSDetalle OSDetalle { get; set; }

        [ForeignKey("StatusAnterior")]
        public short StatusAnteriorId { get; set; }

        //[Required]
        public virtual OSDetalleStatus StatusAnterior { get; set; }

        [ForeignKey("StatusCambio")]
        public short StatusCambioId { get; set; }

        //[Required]
        public virtual OSDetalleStatus StatusCambio { get; set; }
    }
}
