using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioCambioMinutosViewModel
    {
        public long TicketId { get; set; }
        public int Minutos { get; set; }
    }
}
