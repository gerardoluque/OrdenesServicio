using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
	public partial class OSDetalle
	{
        public string NombreContacto
        {
            get { return this.Contacto == null ? "" : string.Format("{0} {1} {2}", this.Contacto.Nombre, this.Contacto.Paterno, this.Contacto.Materno); }
        }
        
        public string NombreAsesor
        {
            get { return this.Asesor == null ? "" : string.Format("{0} {1} {2}", this.Asesor.Nombre, this.Asesor.Paterno, this.Asesor.Materno); }
        }

        public string ServicioDescripcion
        {
            get { return this.Servicio == null ? "" : this.Servicio.ServicioDescr; }
        }

        public string TipoServicioDescripcion
        {
            get { return this.TipoServicio == null ? "" : this.TipoServicio.TipoServicioDescr; }
        }

        public string StatusDescripcion
        {
            get { return this.Status == null ? "" : this.Status.OSDetalleSTDescr; }
        }

	}
}
