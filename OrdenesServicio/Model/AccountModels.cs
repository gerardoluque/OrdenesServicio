using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ZOE.OrdenesServicio.Models
{

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), ErrorMessageResourceName = "CPCampoPassActReq")]
        [DataType(DataType.Password)]
//        [Display(Name = "Contraseña actual")]
        [Display(ResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), Name = "CPCampoPassAct")]
        public string OldPassword { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "La {0} debe ser de al menos {2} caracteres", MinimumLength = 6)]
        [StringLength(100, ErrorMessageResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), ErrorMessageResourceName = "CPCampoPassNuevoValLen", MinimumLength = 6)]
        [DataType(DataType.Password)]
//        [Display(Name = "Nueva contraseña")]
        [Display(ResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), Name = "CPCampoPassNuevo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = "Confirmar contraseña")]
        [Display(ResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), Name = "CPCampoConfirmarNuevo")]
        //[Compare("NewPassword", ErrorMessage = "La contraseña nueva y la confirmación de la contraseña no son iguales.")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), ErrorMessageResourceName = "CPCampoConfirmValidacion")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessageResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), ErrorMessageResourceName = "LogonCampoLoginReq")]
        [Display(ResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), Name = "LogonCampoLogin")] 
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), ErrorMessageResourceName = "LogonCampoPassReq")]
        [DataType(DataType.Password)]
        //[Display(Name = "Contraseña")]
        [Display(ResourceType = typeof(ZOE.OrdenesServicio.Views.Account.AccountResource), Name = "LogonCampoPass")] 
        public string Password { get; set; }

        [Display(Name = "Recordarme en este equipo ?")]
        public bool RememberMe { get; set; }

        public int? ContactoId { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
