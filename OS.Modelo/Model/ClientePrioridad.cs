using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ZOE.OS.Modelo
{
    [Table("ClientePrioridades")]
    public class ClientePrioridad
    {
        [Key]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int Prioridad { get; set; }
        public int Horas { get; set; }
    }
}
