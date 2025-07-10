using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOE.OS.Modelo
{
    public class Cliente
    {
        public Cliente()
        {
            TipoCliente = (short)TiposCliente.Cliente;
            Tipo = (short)EmpresaOrigen.Nacional;
            Prioritario = "N";
            TicketPrioridades = new List<ClientePrioridad>();
            ClientePrioridadesHoras = new ClientePrioridadHorasList();
        }

        [Key]
        public virtual int ClienteId { get; set; }

        [Required(ErrorMessage="El nombre del cliente es requerido")]
        [MaxLength(100)]
        public virtual string Nombre { get; set; }

        //[Required(ErrorMessage = "El RFC del cliente es requerido")]
        [MaxLength(20)]
        [DisplayName("RFC")]
        public virtual string Rfc { get; set; }

        /// Tipo de cliente
        /// 1.- Nacional
        /// 2.- Extranjero
        [Required]
        [DisplayName("Origen")]
        public virtual short Tipo { get; set; }
        
        /// 1.- Cliente
        /// 2.- Prospecto
        [Required]
        [DisplayName("Tipo de Cliente")]
        public virtual short TipoCliente { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Dirección")]
        public virtual string Direccion { get; set; }

        [MaxLength(100)]
        public virtual string Telefono { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Corre electronico no es valido, ejemplo (zoeit@zoeitcustoms.com)")]
        public virtual string Email { get; set; }
        
        /// Si
        /// No
        [MaxLength(1)]
        public virtual string Prioritario { get; set; }

        public virtual ICollection<Proyecto> Proyectos { get; set; }

        public virtual ICollection<Contacto> Contactos { get; set; }

        public virtual List<ClientePrioridad> TicketPrioridades { get; set; }

        [NotMapped]
        public ClientePrioridadHorasList ClientePrioridadesHoras { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números enteros positivos")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public int? SLAPorcentaje { get; set; }
    }

    public class ClientePrioridadHorasList
    {
        public int MuyAlto { get; set; }
        public int Alto { get; set; }
        public int Medio { get; set; }
        public int Bajo { get; set; }
    }

    public enum TiposCliente
    {
        Cliente = 1,
        Prospecto
    }

    public enum EmpresaOrigen
    {
        Nacional = 1,
        Extranjero
    }
}
