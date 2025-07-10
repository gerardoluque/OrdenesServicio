using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            FechaRegistro = DateTime.UtcNow;
            Saldo = 0;
            Status = (short)Proyectotatus.Activo;
        }

        [Key]
        public virtual int ProyectoId { get; set; }
        
        [MaxLength(300)]
        [Required(ErrorMessage = "La descripción del proyecto es requerido")]
        [StringLength(300, ErrorMessage = "La descripción del proyecto debe ser maximo de {2} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public virtual string Descr { get; set; }
        
        /// Fecha de inicio
        [DataType(DataType.Date,ErrorMessage="Capture una fecha valida")]
        [DisplayFormat(DataFormatString="{0:d}", ApplyFormatInEditMode=true, NullDisplayText="Fecha de inicio")]
        public virtual DateTime Fecha { get; set; }

        /// Minutos Iniciales cuando el tipo de proyecto no es una poliza
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true, NullDisplayText = "Minutos del proyecto")]
        //public virtual int? Minutos { get; set; }

        //Saldo en minutos
        public virtual int? Saldo { get; set; }

        /// 1.- Activo
        /// 2.- Cerrado        
        public virtual short Status { get; set; }

        [ForeignKey("Cliente")]
        [Required(ErrorMessage = "El cliente o empresa del proyecto es requerido")]
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual DateTime FechaRegistro { get; set; }

        [ForeignKey("TipoProyecto")]
        [Required(ErrorMessage = "El tipo de proyecto es requerido")]
        public short TipoProyectoId { get; set; }

        public virtual TipoProyecto TipoProyecto { get; set; }

        [ForeignKey("UsuarioRegistro")]
        public int UsuarioIdRegistro { get; set; }

        public virtual Usuario UsuarioRegistro { get; set; }

        public virtual ICollection<ProyectoAbono> Abonos { get; set; }

        public virtual short? TipoCliente { get; set; }
    }

    public enum Proyectotatus
    {
        Activo = 1,
        Cerrado
    }
}
