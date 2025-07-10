using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public partial class Contacto
    {
        [Key]
        public virtual int ContactoId { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(80)]
        public virtual string Nombre { get; set; }

        [MaxLength(30)]
        public virtual string Paterno { get; set; }

        [MaxLength(30)]
        public virtual string Materno { get; set; }

        [MaxLength(100)]
        public virtual string Telefono { get; set; }

        [MaxLength(100)]
        public virtual string Movil { get; set; }

        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "El {0} es de {1} caracteres")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Corre electronico no es valido, ejemplo (zoeit@zoeitcustoms.com)")]
        public virtual string Email { get; set; }

        [MaxLength(50)]
        public virtual string Puesto { get; set; }

        [ForeignKey("Cliente")]
        [Required(ErrorMessage = "El cliente o empresa es requerido")]
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }

        [ForeignKey("Area")]
        public short? AreaId { get; set; }

        public virtual Area Area { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioAcceso { get; set; }

        //public virtual ICollection<OrdenServicio> OrdenesServicio { get; set; }

        //public virtual ICollection<OSDetalle> OSDetalles { get; set; }
    }
}
