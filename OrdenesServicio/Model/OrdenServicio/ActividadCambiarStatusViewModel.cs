using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZOE.OrdenesServicio.Modelo
{
    public class CambiarStatusViewModel
    {
        public long Ticket { get; set; }

        public long DetalleId { get; set; }

        public short StatusId { get; set; }

        public string Observaciones { get; set; }

        public string StatusActualDescr { get; set; }

        public string StatusCambioDescr { get; set; }

        public string Mensaje { get; set; }

        public bool success { get; set; }

        public bool NotificarPorCorreoContacto { get; set; }
    }
}