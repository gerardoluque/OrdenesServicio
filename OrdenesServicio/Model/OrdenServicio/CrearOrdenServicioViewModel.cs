using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OrdenesServicio.Modelo
{
    public class CrearOrdenServicioViewModel
    {
        public bool AsesorCreando { get; set; }

        [Required(ErrorMessage = "El cliente o empresa es requerido")]
        public int ClienteIdSeleccionado { get; set; }

        [Required(ErrorMessage = "El contacto es requerido")]
        public int ContacoIdSeleccionado { get; set; }

        [Required(ErrorMessage = "El proyecto es requerido")]
        public int ProyectoIdSeleccionado { get; set; }

        [DisplayName("Notificar por correo electrónico al contacto")]
        [Display(Prompt="Notificar por correo electrónico al contacto")]
        public bool NotificarContactoEmail { get; set; }

        public SelectList Clientes { get; set; }
        public SelectList Contactos { get; set; }
        public SelectList Asesores { get; set; }
        public SelectList Prioridades { get; set; }
        public OrdenServicio OrdenServicio { get; set; }
    }
}