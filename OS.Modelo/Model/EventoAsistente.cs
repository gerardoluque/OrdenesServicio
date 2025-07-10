using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class EventoAsistente
    {
        public EventoAsistente()
        {
            Fecha = System.DateTime.Now;
            EsCliente = true;
        }

        [Key]
        public virtual short EventoAsistenteId { get; set; }

        [Required(ErrorMessage = "La fecha en que se registro es requerida")]
        [DisplayName("Fecha y hora de Registro")]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Empresa es requerido")]
        [MaxLength(100)]
        [DisplayName("Empresa")]
        public virtual string Nombre { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "El correo es requerido")]
        [StringLength(100, ErrorMessage = "El {0} es de {1} caracteres")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Corre electronico no es valido, ejemplo (zoeit@zoeitcustoms.com)")]
        public virtual string Email { get; set; }

        [MaxLength(100)]
        public virtual string Telefono { get; set; }

        [Required(ErrorMessage = "Favor de especificar tipo de contacto")]
        [DisplayName("Tipo de Contacto")]
        public virtual bool EsCliente { get; set; }

        [ForeignKey("Evento")]
        [Required(ErrorMessage = "El Evento es requerido")]
        public short EventoId { get; set; }

        public virtual Evento Evento { get; set; }

    }
}
