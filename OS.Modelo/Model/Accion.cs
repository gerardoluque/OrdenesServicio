using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class Accion
    {
        [Key]
        public virtual short AccionId { get; set; }

        [MaxLength(50)]
        public virtual string AccionDescr { get; set; }

        [MaxLength(50)]
        public virtual string Action { get; set; }

        [MaxLength(50)]
        public virtual string Controller { get; set; }

        public virtual short Orden { get; set; }

        public virtual short Tipo { get; set; }

        [ForeignKey("AccionPadre")]
        public virtual short?    AccionIdPadre { get; set; }

        public virtual Accion AccionPadre { get; set; }

        public virtual ICollection<RolAccion> RolAcciones { get; set; }
    }

    public enum TiposAccion
    {
        Accion = 1,
        Agrupacion
    }
}
