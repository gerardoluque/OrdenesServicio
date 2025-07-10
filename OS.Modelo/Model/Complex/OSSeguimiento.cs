using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo.Model.Complex
{
    [ComplexType]
    public class OSSeguimiento
    {
        public long Ticket { get; set; }
        public string Descripcion { get; set; }
        public int? AsesorId { get; set; }
        public string Asesor { get; set; }
        public DateTime Fecha { get; set; }
        public string Obs { get; set; }
        public string Cliente { get; set; }
        public string Contacto { get; set; }
        public DateTime? FechaUlitmoMovimiento { get; set; }
        public string Status { get; set; }
        public string Proyecto { get; set; }
        public int SaldoPoliza { get; set; }
    }
}
