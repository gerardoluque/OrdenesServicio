using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class GrupoAsesor
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Grupo")]
        public virtual short GrupoId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Asesor")]
        public virtual int AsesorId { get; set; }

        public virtual Asesor Asesor { get; set; }

        public virtual Grupo Grupo { get; set; }
    }
}
