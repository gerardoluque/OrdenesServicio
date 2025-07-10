using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public partial class Asesor
    {
        [Key]
        public virtual int AsesorId { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(50)]
        public virtual string Nombre { get; set; }

        [MaxLength(30)]
        public virtual string Paterno { get; set; }

        [MaxLength(30)]
        public virtual string Materno { get; set; }

        [MaxLength(15)]
        public virtual string Telefono { get; set; }

        [MaxLength(50)]
        public virtual string Movil { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "El {0} es de {1} caracteres")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage="Corre electronico no es valido, ejemplo (zoeit@zoeitcustoms.com)")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [ForeignKey("Area")]
        [Required(ErrorMessage="El {0} es requerido")]
        public short AreaId { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioAcceso { get; set; }

        public virtual Area Area { get; set; }

        //public virtual ICollection<OrdenServicio> OrdenesServicio { get; set; }

        //public virtual ICollection<OSDetalle> OSDetalles { get; set; }
    }
}
