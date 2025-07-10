using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ZOE.OS.Modelo
{
    public class OrdenServicio
    {
        public OrdenServicio()
        {
            Fecha = DateTime.UtcNow;
            OSStatusId = (short)OrdenServicioStatus.Creada;
        }

        [Key]
        public virtual long Ticket { get; set; }

        /// Fecha Abierto
        [Required]
        public virtual DateTime Fecha { get; set; }

        /// Fecha de compromiso
        public virtual DateTime? FechaComp { get; set; }

        /// Observaciones del servicio
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(ZOE.OS.Modelo.ModeloResource), ErrorMessageResourceName = "CampoDetalleOrdenServicioReq")]
        //[Display(Prompt="Detalle",Description="Observaciones")]
        [Display(ResourceType = typeof(ZOE.OS.Modelo.ModeloResource), Name = "lblCampoDetalleOrdenServicio")]
        //[DisplayName(ZOE.OS.Modelo.ModeloResource.lblCampoDetalleOrdenServicio)]
        public virtual string Obs { get; set; }

        [ForeignKey("Status")]
        public short OSStatusId { get; set; }

        public virtual OSStatus Status { get; set; }

        [ForeignKey("Contacto")]
        [Required(ErrorMessage = "El contacto de la empresa es requerido")]
        public int ContactoId { get; set; }

        public virtual Contacto Contacto { get; set; }

        [ForeignKey("Asesor")]
        //[Required(ErrorMessage = "El Asesor asignado es requerido")]
        public int? AsesorId { get; set; }

        public virtual Asesor Asesor { get; set; }

        [ForeignKey("AreaResponsable")]
        public short? AreaRespId { get; set; }

        public virtual Area AreaResponsable { get; set; }

        [ForeignKey("Proyecto")]
        public int? ProyectoId { get; set; }

        public virtual Proyecto Proyecto { get; set; }

        public virtual ICollection<OSDetalle> OSDetalles { get; set; }

        public virtual short? TipoCliente { get; set; }

        [ForeignKey("Usuario")]
        public virtual int? UsuarioIdRegistro { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual int? Minutos { get; set; }

        public virtual int? Prioridad { get; set; }

        [NotMapped]
        public string PrioridadTexto
        {
            get
            {
                if (Prioridad.HasValue)
                {
                    return ((Prioridades)Prioridad.Value).ToString();
                }
                return "Sin Prioridad";
            }
        }

        public static SelectList GetPrioridadesSelectList(object selectedValue = null)
        {
            var values = Enum.GetValues(typeof(Prioridades))
                .Cast<Prioridades>()
                .Select(p => new
                {
                    Value = (int)p,
                    Text = p.ToString()
                });

            return new SelectList(values, "Value", "Text", selectedValue);
        }
    }

    public enum OrdenServicioStatus
    {
        Creada = 1,
        EnSeguimiento,
        Terminada,
        Cerrada,
        Cancelada,
        Asignado,
        Reasignado
    }

    public enum Prioridades
    {
        MuyAlto = 1,
        Alto,
        Medio,
        Bajo,
    }
}