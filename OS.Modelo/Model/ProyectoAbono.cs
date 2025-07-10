using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class ProyectoAbono
    {
        public ProyectoAbono()
        {
            Fecha = System.DateTime.UtcNow;
        }

        [Key]
        public virtual long AbonoId { get; set; }

        [ForeignKey("Proyecto")]
        [Required]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "La fecha de cargo es requerida")]
        [DisplayName("Fecha Abono")]
        public virtual DateTime FechaAbono { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage="La referencia es requerida")]
        public string Referencia { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        /// Minutos Disponibles antes del abono
        public virtual int? MinutosAntesAbono { get; set; }

        /// Minutos Abonados
        [Required(ErrorMessage = "Los minutos abonados al proyecto es requerido")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true, NullDisplayText = "Minutos Abnonados")]
        public virtual int Minutos { get; set; }

        /// Monto Abonado
        //[Required(ErrorMessage = "El monto abonado al proyecto es requerido")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true, NullDisplayText = "Monto")]
        public virtual long Monto { get; set; }

        /// Fecha de registro
        public virtual DateTime Fecha { get; set; }

        [ForeignKey("Usuario")]
        [Required]
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }        

    }
}
