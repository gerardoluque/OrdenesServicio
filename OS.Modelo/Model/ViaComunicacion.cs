using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public class ViaComunicacion
    {
        [Key]
        public virtual short ViaComId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string ViaComDescr { get; set; }
    }
}
