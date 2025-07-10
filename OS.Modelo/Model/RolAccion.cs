using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class RolAccion
    {
        [Key]
        public virtual short RolAccionId { get; set; }

        [MaxLength(256)]
        public virtual string RoleName { get; set; }

        [ForeignKey("Accion")]
        public virtual short AccionId { get; set; }

        public virtual Accion Accion { get; set; }
    }
}
