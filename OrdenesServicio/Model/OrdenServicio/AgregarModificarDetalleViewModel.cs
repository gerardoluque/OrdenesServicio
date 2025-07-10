using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OrdenesServicio.Model.OrdenServicio
{
    public class AgregarModificarDetalleViewModel
    {
        public ZOE.OS.Modelo.OrdenServicio OrdenServicio { get; set; }
        public ZOE.OS.Modelo.OSDetalle Actividad { get; set; }
        
        [Required(ErrorMessage = "El proyecto es requerido")]
        public int ProyectoIdSeleccionado { get; set; }

        [Required(ErrorMessage = "El asesor es requerido")]
        public int AsesorIdSeleccionado { get; set; }
    }
}