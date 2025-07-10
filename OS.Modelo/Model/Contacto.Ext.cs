using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OS.Modelo
{
    public partial class Contacto
    {
        [NotMapped]
        [Required]
        [Display(Name = "Nombre Usuario")]
        [StringLength(100, ErrorMessage="El {0} debe ser maximo de {2} caracteres", MinimumLength=5)]
        public string UserName { get; set; }

        [NotMapped]
        [Required(ErrorMessage="La contraseña es requerida")]
        [StringLength(20, ErrorMessage = "El {0} debe ser al menos de {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no concuerdan")]
        public string ConfirmPassword { get; set; }

        public string NombreCompleto
        {
            get { return this == null ? "" : string.Format("{0} {1} {2}", this.Nombre, this.Paterno, this.Materno); }
        }
    }
}
