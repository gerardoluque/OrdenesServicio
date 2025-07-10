using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class Area
    {
        [Key]
        public virtual short AreaId { get; set; }

        [Required(ErrorMessage = "La descripción del Area es requerido")]
        [MaxLength(50)]
        [DisplayName("Descripción")]
        public virtual string AreaDescr { get; set; }

        /// 1=Zoe, 2=Cliente
        [Required(ErrorMessage = "El tipo del Area es requerido")]
        [DisplayName("Tipo de Area")]
        public virtual short TipoArea { get; set; }

        public virtual ICollection<Contacto> Contactos { get; set; }

        public virtual ICollection<Asesor> Asesores { get; set; }
    }

    public enum TiposArea
    {
        Zoe = 1,
        Cliente
    }
}
