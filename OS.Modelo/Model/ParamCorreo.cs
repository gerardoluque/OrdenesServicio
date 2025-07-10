using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class ParamCorreo
    {
        [Key]
        public virtual short ParamId { get; set; }

        [Required(ErrorMessage = "La descripción del Area es requerido")]
        [MaxLength(100)]
        [DisplayName("Servidor Host")]
        public virtual string Host { get; set; }

        [Required(ErrorMessage = "El numero de puerto de salida es requerido")]
        [DisplayName("Puerto de Salida")]
        public virtual short PuertoSalida { get; set; }

        [Required(ErrorMessage = "La cuenta de acceso es requerido")]
        [MaxLength(100)]
        [DisplayName("Cuenta de acceso")]
        public virtual string CuentaAcceso { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(100)]
        [DisplayName("Contraseña de acceso")]
        [Required(ErrorMessage = "La contraseña de acceso es requerido")]
        public virtual string ContrasenaAcceso { get; set; }

        [Required(ErrorMessage = "La cuenta de envio es requerido")]
        [MaxLength(100)]
        [DisplayName("Cuenta de Envio")]
        public virtual string From { get; set; }
    }
}
