using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class Evento
    {
        [Key]
        public virtual short EventoId { get; set; }

        [Required(ErrorMessage = "La descripción del Evento es requerido")]
        [MaxLength(100)]
        [DisplayName("Descripción")]
        public virtual string EventoDescr { get; set; }
    }
}
