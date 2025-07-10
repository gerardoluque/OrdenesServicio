using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class Usuario
    {
        public Usuario()
        {
        }

        [Key]
        public virtual int UsuarioId { get; set; }

        public virtual Guid? UserId { get; set; }

        [MaxLength(256)]
        public virtual string UserName { get; set; }

        /// 1.- Asesor
        /// 2.- Contacto
        [Required]
        public virtual short TipoUsuario { get; set; }

        [ForeignKey("Contacto")]
        public virtual int? ContactoId { get; set; }
        public virtual Contacto Contacto { get; set; }

        [ForeignKey("Asesor")]
        public virtual int? AsesorId { get; set; }
        public virtual Asesor Asesor { get; set; }
    }

    public enum TiposUsuario
    {
        Asesor = 1,
        Contacto,
        Administrador,
        MesaAsignacion
    }
}
