using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class Servicio
    {
        [Key]
        public virtual short ServicioId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string ServicioDescr { get; set; }
    }
}
