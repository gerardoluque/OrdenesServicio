using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public partial class OSDetalle
    {
        public OSDetalle()
        {
            FechaAbierto = DateTime.Now;
            FechaRegistro = DateTime.Now;
            Prioridad = (short)PrioridadDetalle.Baja;
            StatusId = (short)ActividadStatus.Activa;
        }

        [Key, Column(Order=0)]
        [ForeignKey("OrdenServicio")]
        [Required]
        public long Ticket { get; set; }

        [Key, Column(Order=1)]
        public virtual long DetalleId { get; set; }

        public virtual OrdenServicio OrdenServicio { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage="La descripción de la actividad es requerida")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        [StringLength(500, ErrorMessage = "El {0} debe ser maximo de {2} caracteres")]
        public virtual string DetalleDescr { get; set; }

        public virtual DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage="La fecha en que se registro es requerida")]
        [DisplayName("Fecha y hora de Inicio")]
        public virtual DateTime FechaAbierto { get; set; }

        [DisplayName("Fecha y hora de Termino")]
        public virtual DateTime? FechaCerrado { get; set; }
        
        /// Fecha de compromiso
        [DisplayName("Fecha de Compromiso")]
        public virtual DateTime? FechaComp { get; set; }
        
        /// Minutos consumidos
        [Required(ErrorMessage="Los minutos de consumo es requerido")]
        public virtual int Minutos { get; set; }

        /// La actividad afecta a proyecto de tipo poliza
        [MaxLength(1)]
        public virtual string AfectaPoliza { get; set; }

        /// 1.- Alta
        /// 2.- Media
        /// 3.- Baja
        [Required(ErrorMessage = "La prioridad de la actividad es requerida")]
        public virtual short Prioridad { get; set; }

        [ForeignKey("Contacto")]
        [DisplayName("Contacto")]
        [Required(ErrorMessage = "El contacto de la empresa/cliente es requerido")]
        public int ContactoId { get; set; }

        public virtual Contacto Contacto { get; set; }

        [ForeignKey("Asesor")]
        public int AsesorId { get; set; }

        public virtual Asesor Asesor { get; set; }

        [ForeignKey("AreaResponsable")]
        public short AreaRespId { get; set; }

        public virtual Area AreaResponsable { get; set; }

        [ForeignKey("Status")]
        [DisplayName("Status Inicial")]
        public short StatusId { get; set; }

        public virtual OSDetalleStatus Status { get; set; }

        [ForeignKey("Servicio")]
        [Required(ErrorMessage = "El tipo de servicio es requerido")]
        [DisplayName("Tipo de Servicio")]
        public short ServicioId { get; set; }

        public virtual Servicio Servicio { get; set; }

        [ForeignKey("TipoServicio")]
        [Required(ErrorMessage = "La clasificación es requerida")]
        [DisplayName("Clasificación")]
        public short TipoServicioId { get; set; }

        public virtual TipoServicio TipoServicio { get; set; }

        [ForeignKey("ViaComunicacion")]
        [DisplayName("Via de comunicación")]
        [Required(ErrorMessage = "La via de comunicación es requerida")]
        public short? ViaComunicacionId { get; set; }

        public virtual ViaComunicacion ViaComunicacion { get; set; }


        public virtual DateTime? FechaTerminada { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string ObsTerminada { get; set; }

        public virtual Usuario UsuarioTermino { get; set; }


        public virtual DateTime? FechaCancelada { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string ObsCancelada { get; set; }

        public virtual Usuario UsuarioCancelo { get; set; }


        public virtual DateTime? FechaPendiente { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observación")]
        public virtual string ObsPendiente { get; set; }

        public virtual Usuario UsuarioPendiente { get; set; }

    }

    public enum PrioridadDetalle
	{
        Alta = 1,
        Media,
        Baja
	}

    public enum ActividadStatus
    {
        Registrada = 1,
        Activa,
        Pendiente,
        Terminada,
        Cancelada
    }
}
