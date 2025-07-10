using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class TipoServicio
    {
        [Key]
        public virtual short TipoServicioId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string TipoServicioDescr { get; set; }

        [Required]
        [MaxLength(1)]
        public virtual string AfectaPoliza { get; set; }
    }
}
