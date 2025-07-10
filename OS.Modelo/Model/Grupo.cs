using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class Grupo
    {
        public Grupo()
        {
            GrupoTipo = (short)TiposGrupo.Zoe;
        }

        [Key]
        public virtual short GrupoId { get; set; }

        [Required(ErrorMessage = "La descripción del Grupo es requerido")]
        [MaxLength(50)]
        [DisplayName("Descripción")]
        public virtual string GrupoDescr { get; set; }

        /// 1=Zoe, 2=Cliente
        [Required(ErrorMessage = "El tipo del Grupo es requerido")]
        [DisplayName("Tipo de Grupo")]
        public virtual short GrupoTipo{ get; set; }

        public virtual ICollection<Asesor> Asesores { get; set; }
    }

    public enum TiposGrupo
    {
        Zoe = 1,
        Cliente
    }
}
