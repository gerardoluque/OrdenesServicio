using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Modelo
{
    public class OrdenServicioHistoriaViewModel
    {
        public DateTime Fecha { get; set; }
        public String Obs { get; set; }
        public String CambioStatus { get; set; }
        public String Responsable { get; set; }
        public Usuario UsuarioResponsable { get; set; }
    }
}