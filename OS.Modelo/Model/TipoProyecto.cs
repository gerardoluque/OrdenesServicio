using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class TipoProyecto
    {
        [Key]
        public virtual short TipoProyectoId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string TipoProyectoDescr { get; set; }

        [Required]
        [MaxLength(1)]
        public virtual string TipoPoliza { get; set; }
    }
}
