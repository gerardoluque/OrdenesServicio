using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class OSNota
    {
        public OSNota()
        {
            FechaRegistro = DateTime.UtcNow;
        }

        [Key, Column(Order = 0)]
        [ForeignKey("OrdenServicio")]
        public long Ticket { get; set; }

        [Key, Column(Order = 1)]
        public virtual short OSNotaId { get; set; }

        public virtual DateTime FechaRegistro { get; set; }

        [MaxLength(1000)]
        [Required(ErrorMessage = "El comentario o nota es requerido")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Comentario")]
        public virtual string Nota { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual OrdenServicio OrdenServicio { get; set; }

        [DisplayName("Permitir que el contacto vea este comentario")]
        public virtual bool PermitirVerCliente { get; set; }

        [NotMapped]
        public string NombreUsuario { get; set; }
    }
}
