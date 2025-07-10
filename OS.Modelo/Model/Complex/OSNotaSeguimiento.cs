using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ZOE.OS.Modelo
{
    public class OSNotaSeguimiento
    {
        public long? Ticket { get; set; }
        public short? OSNotaId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Nota { get; set; }
        public int? UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
    }
}
