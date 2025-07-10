using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Modelo
{
    public class RegistrarContactoViewModel
    {
        public Contacto Contacto { get; set; }

        [Required]
        [Display(Name = "Nombre Usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe ser al menos de {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la confirmación no concuerdan")]
        public string ConfirmPassword { get; set; }
    }
}