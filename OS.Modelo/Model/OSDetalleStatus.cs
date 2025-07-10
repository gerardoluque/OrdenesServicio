using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class OSDetalleStatus
    {
        [Key]
        public virtual short OSDetalleSTId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string OSDetalleSTDescr { get; set; }
    }
}
