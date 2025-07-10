using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ZOE.OS.Modelo;
using System.Web.Security;
using System.Web.Management;
using ZOE.OrdenesServicio.Models;


namespace ZOE.OS.Modelo
{
    public class DataContextInitializer : CreateDatabaseIfNotExists<OSContext>
    {
        protected override void Seed(OSContext context)
        {
            #region Status de las ordenes de servicio
            OSStatus OSStatus1 = new Modelo.OSStatus()
            {
                OSStatusId=1,
                Descr="Creada"
            };

            OSStatus OSStatus2 = new Modelo.OSStatus()
            {
                OSStatusId = 2,
                Descr = "En Seguimiento"
            };

            OSStatus OSStatus3 = new Modelo.OSStatus()
            {
                OSStatusId = 3,
                Descr = "Terminada"
            };

            OSStatus OSStatus4 = new Modelo.OSStatus()
            {
                OSStatusId = 4,
                Descr = "Cerrada"
            };
            #endregion

            #region Status de las actividades
            OSDetalleStatus detStatus1 = new OSDetalleStatus()
            {
                OSDetalleSTId = 1,
                OSDetalleSTDescr = "Registrada"
            };
            OSDetalleStatus detStatus2 = new OSDetalleStatus()
            {
                OSDetalleSTId = 2,
                OSDetalleSTDescr = "Activa"
            };

            OSDetalleStatus detStatus3 = new OSDetalleStatus()
            {
                OSDetalleSTId = 3,
                OSDetalleSTDescr = "Pendiente"
            };
            OSDetalleStatus detStatus4 = new OSDetalleStatus()
            {
                OSDetalleSTId = 4,
                OSDetalleSTDescr = "Terminada"
            };
            OSDetalleStatus detStatus5 = new OSDetalleStatus()
            {
                OSDetalleSTId = 5,
                OSDetalleSTDescr = "Cancelada"
            };
            #endregion

            #region Tipos de proyectos
            TipoProyecto tipoProy1 = new TipoProyecto()
            {
                TipoProyectoId = 1,
                TipoProyectoDescr = "Poliza",
                TipoPoliza = "S"
            };

            TipoProyecto tipoProy2 = new TipoProyecto()
            {
                TipoProyectoId = 2,
                TipoProyectoDescr = "Reconstrucciones",
                TipoPoliza = "N"
            };

            TipoProyecto tipoProy3 = new TipoProyecto()
            {
                TipoProyectoId = 3,
                TipoProyectoDescr = "Desarrollo",
                TipoPoliza = "N"
            };

            TipoProyecto tipoProy4 = new TipoProyecto()
            {
                TipoProyectoId = 4,
                TipoProyectoDescr = "Capacitacion",
                TipoPoliza = "N"
            };
            #endregion

            #region Servicios
            Servicio serv1 = new Servicio()
            {
                ServicioId = 1,
                ServicioDescr = "Instalación"
            };
            Servicio serv2 = new Servicio()
            {
                ServicioId = 2,
                ServicioDescr = "Capacitación"
            };
            Servicio serv3 = new Servicio()
            {
                ServicioId = 3,
                ServicioDescr = "Demostración"
            };
            Servicio serv4 = new Servicio()
            {
                ServicioId = 4,
                ServicioDescr = "Reconstrucción"
            };
            #endregion

            #region Tipos de Servicio
            TipoServicio tip1 = new TipoServicio()
            {
                TipoServicioId=1,
                TipoServicioDescr = "Tiempo Cargable",
                AfectaPoliza="S"
            };
            TipoServicio tip2 = new TipoServicio()
            {
                TipoServicioId = 2,
                TipoServicioDescr = "Garantia",
                AfectaPoliza = "N"
            };
            TipoServicio tip3 = new TipoServicio()
            {
                TipoServicioId = 3,
                TipoServicioDescr = "Desarrollo",
                AfectaPoliza = "S"
            };
            TipoServicio tip4 = new TipoServicio()
            {
                TipoServicioId = 4,
                TipoServicioDescr = "Interno",
                AfectaPoliza = "S"
            };
            TipoServicio tip5 = new TipoServicio()
            {
                TipoServicioId = 5,
                TipoServicioDescr = "Contra Proyecto",
                AfectaPoliza = "S"
            };
            TipoServicio tip6 = new TipoServicio()
            {
                TipoServicioId = 6,
                TipoServicioDescr = "Seguimeinto Proyecto",
                AfectaPoliza = "S"
            };
            TipoServicio tip7 = new TipoServicio()
            {
                TipoServicioId = 7,
                TipoServicioDescr = "Reconstruccion",
                AfectaPoliza = "S"
            };
            TipoServicio tip8 = new TipoServicio()
            {
                TipoServicioId = 8,
                TipoServicioDescr = "Administrativo",
                AfectaPoliza = "N"
            };
            TipoServicio tip9 = new TipoServicio()
            {
                TipoServicioId = 9,
                TipoServicioDescr = "Externo",
                AfectaPoliza = "N"
            };
            #endregion

            #region Vias de comunicacion
            ViaComunicacion via1 = new ViaComunicacion()
            {
                ViaComId=1,
                ViaComDescr = "Telefono"
            };
            ViaComunicacion via2 = new ViaComunicacion()
            {
                ViaComId = 2,
                ViaComDescr = "Personal"
            };
            ViaComunicacion via3 = new ViaComunicacion()
            {
                ViaComId = 3,
                ViaComDescr = "Local"
            };
            ViaComunicacion via4 = new ViaComunicacion()
            {
                ViaComId = 4,
                ViaComDescr = "Email"
            };
            ViaComunicacion via5 = new ViaComunicacion()
            {
                ViaComId = 5,
                ViaComDescr = "Skype"
            };
            #endregion

            #region Areas
            Area area1 = new Area()
            {
                AreaDescr = "Finanzas-Contabilidad",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area2 = new Area()
            {
                AreaDescr = "IT-Informática",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area4 = new Area()
            {
                AreaDescr = "Aduanas-Mex",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area5 = new Area()
            {
                AreaDescr = "Aduanas-USA",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area6 = new Area()
            {
                AreaDescr = "Exportaciones",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area7 = new Area()
            {
                AreaDescr = "Administrativo",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area8 = new Area()
            {
                AreaDescr = "Finanzas USA",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area9 = new Area()
            {
                AreaDescr = "Gerente Administrativo",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area10 = new Area()
            {
                AreaDescr = "Trafico",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area11 = new Area()
            {
                AreaDescr = "Importaciones/Exportaciones",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area12 = new Area()
            {
                AreaDescr = "Importaciones",
                TipoArea = (short)TiposArea.Cliente
            };
            Area area13 = new Area()
            {
                AreaDescr = "IT",
                TipoArea = (short)TiposArea.Zoe
            };
            Area area14 = new Area()
            {
                AreaDescr = "Import/Export",
                TipoArea = (short)TiposArea.Zoe
            };
            #endregion

            #region cliente zoe e integra
            Cliente cliente = new Cliente()
            {
                Nombre = "Integra",
                Rfc = "",
                Direccion = "",
                Telefono = "",
                Email = "integra@integra.COM",
                Tipo = (short)EmpresaOrigen.Nacional,
                TipoCliente = (short)TiposCliente.Cliente
            };
            Cliente cliente2 = new Cliente()
            {
                Nombre = "Zoe",
                Rfc = "",
                Direccion = "",
                Telefono = "",
                Email = "zoe@zoeitcustoms.COM",
                Tipo = (short)EmpresaOrigen.Nacional,
                TipoCliente = (short)TiposCliente.Cliente
            };
            Contacto contacto1 = new Contacto()
            {
                Nombre = "Adriana",
                Paterno = "Layna",
                Email = "alayna@zoeitcustoms.com",
                Telefono = "",
                UserName = "alayna",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto2 = new Contacto()
            {
                Nombre = "Josue",
                Paterno = "Hernandez",
                Email = "jhernandez@zoeitcustoms.com",
                Telefono = "",
                UserName = "jhernandez",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto3 = new Contacto()
            {
                Nombre = "Eric",
                Paterno = "Reyes",
                Email = "ereyes@zoeitcustoms.com",
                Telefono = "",
                UserName = "ereyes",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto4 = new Contacto()
            {
                Nombre = "Rafael",
                Paterno = "Villegas",
                Email = "rvillegas@zoeitcustoms.com",
                Telefono = "",
                UserName = "rvillegas",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };

            Contacto contacto5 = new Contacto()
            {
                Nombre = "Nataliza",
                Paterno = "Bezanilla",
                Email = "nbezanilla@zoeitcustoms.com",
                Telefono = "",
                UserName = "nbezanilla",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto6 = new Contacto()
            {
                Nombre = "Roman",
                Paterno = "Gomez",
                Email = "rgomez@zoeitcustoms.com",
                Telefono = "",
                UserName = "rgomez",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto7 = new Contacto()
            {
                Nombre = "Paola",
                Paterno = "Navarro",
                Email = "pnavarro@zoeitcustoms.com",
                Telefono = "",
                UserName = "pnavarro",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto8 = new Contacto()
            {
                Nombre = "Marco",
                Paterno = "Melendez",
                Email = "mmelendez@zoeitcustoms.com",
                Telefono = "",
                UserName = "mmelendez",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto9 = new Contacto()
            {
                Nombre = "Francisco",
                Paterno = "Paniagua",
                Email = "fpaniagua@zoeitcustoms.com",
                Telefono = "",
                UserName = "fpaniagua",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto10 = new Contacto()
            {
                Nombre = "Gerardo",
                Paterno = "Luque",
                Email = "gluque@zoeitcustoms.com",
                Telefono = "",
                UserName = "gluque",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente2
            };
            Contacto contacto11 = new Contacto()
            {
                Nombre = "Rogelio",
                Paterno = "Garcia",
                Email = "rgarcia@zoeitcustoms.com",
                Telefono = "",
                UserName = "rgarcia",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente2
            };
            Contacto contacto12 = new Contacto()
            {
                Nombre = "Emilio",
                Paterno = "Gonzalez",
                Email = "egonzalez@zoeitcustoms.com",
                Telefono = "",
                UserName = "egonzalez",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            Contacto contacto13 = new Contacto()
            {
                Nombre = "Analia",
                Paterno = "Padilla",
                Email = "apadilla@zoeitcustoms.com",
                Telefono = "",
                UserName = "apadilla",
                Password = "zoe123",
                ConfirmPassword = "zoe123",
                Cliente = cliente
            };
            #endregion

            #region Instalar ASP.NET Membership y agregar usuarios iniciales
            ApplicationServices.InstallServices(SqlFeatures.Membership | SqlFeatures.RoleManager);

            Membership.CreateUser("gluque", "zoe123");
            Membership.CreateUser("egonzalez", "zoe123");
            Membership.CreateUser("rgarcia", "zoe123");
            Membership.CreateUser("alayna", "zoe123");
            Membership.CreateUser("ereyes", "zoe123");
            Membership.CreateUser("jhernandez", "zoe123");
            Membership.CreateUser("rvillegas", "zoe123");

            Membership.CreateUser("nbezanilla", "zoe123");
            Membership.CreateUser("rgomez", "zoe123");
            Membership.CreateUser("pnavarro", "zoe123");
            Membership.CreateUser("mmelendez", "zoe123");
            Membership.CreateUser("fpaniagua", "zoe123");

            Roles.CreateRole("Admin");
            Roles.CreateRole("Asesor");
            Roles.CreateRole("Contacto");
            
            Roles.AddUsersToRole(new[] { "gluque" }, "Admin");
            Roles.AddUsersToRole(new[] { "egonzalez" }, "Admin");
            Roles.AddUsersToRole(new[] { "rgarcia" }, "Admin");
            Roles.AddUsersToRole(new[] { "alayna" }, "Asesor");
            Roles.AddUsersToRole(new[] { "ereyes" }, "Asesor");
            Roles.AddUsersToRole(new[] { "jhernandez" }, "Asesor");
            Roles.AddUsersToRole(new[] { "rvillegas" }, "Asesor");

            Roles.AddUsersToRole(new[] { "nbezanilla" }, "Asesor");
            Roles.AddUsersToRole(new[] { "rgomez" }, "Asesor");
            Roles.AddUsersToRole(new[] { "pnavarro" }, "Asesor");
            Roles.AddUsersToRole(new[] { "mmelendez" }, "Asesor");
            Roles.AddUsersToRole(new[] { "fpaniagua" }, "Asesor");

            Asesor asesor1 = new Asesor()
            {
                Nombre = "Adriana",
                Paterno = "Layna",
                Email = "alayna@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "alayna",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            }; 
            Asesor asesor2 = new Asesor()
            {
                Nombre = "Josue",
                Paterno = "Hernandez",
                Email = "jhernandez@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "jhernandez",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor3 = new Asesor()
            {
                Nombre = "Eric",
                Paterno = "Reyes",
                Email = "ereyes@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "ereyes",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor4 = new Asesor()
            {
                Nombre = "Rafael",
                Paterno = "Villegas",
                Email = "rvillegas@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "rvillegas",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };

            Asesor asesor5 = new Asesor()
            {
                Nombre = "Nataliza",
                Paterno = "Bezanilla",
                Email = "nbezanilla@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "nbezanilla",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor6 = new Asesor()
            {
                Nombre = "Roman",
                Paterno = "Gomez",
                Email = "rgomez@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "rgomez",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor7 = new Asesor()
            {
                Nombre = "Paola",
                Paterno = "Navarro",
                Email = "pnavarro@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "pnavarro",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor8 = new Asesor()
            {
                Nombre = "Marco",
                Paterno = "Melendez",
                Email = "mmelendez@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "mmelendez",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor9 = new Asesor()
            {
                Nombre = "Francisco",
                Paterno = "Paniagua",
                Email = "fpaniagua@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "fpaniagua",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor10 = new Asesor()
            {
                Nombre = "Gerardo",
                Paterno = "Luque",
                Email = "gluque@zoeitcustoms.com",
                Telefono = "",
                Area = area13,
                UserName = "gluque",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor11 = new Asesor()
            {
                Nombre = "Rogelio",
                Paterno = "Garcia",
                Email = "rgarcia@zoeitcustoms.com",
                Telefono = "",
                Area = area13,
                UserName = "rgarcia",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };
            Asesor asesor12 = new Asesor()
            {
                Nombre = "Emilio",
                Paterno = "Gonzalez",
                Email = "egonzalez@zoeitcustoms.com",
                Telefono = "",
                Area = area14,
                UserName = "egonzalez",
                Password = "zoe123",
                ConfirmPassword = "zoe123"
            };

            Usuario usuarioAdmin1 = new Usuario()
            {
                UserName = "gluque",
                TipoUsuario = (short)TiposUsuario.Administrador,
                Asesor = asesor10
            };
            Usuario usuarioAdmin2 = new Usuario()
            {
                UserName = "egonzalez",
                TipoUsuario = (short)TiposUsuario.Administrador,
                Asesor = asesor12
            };
            Usuario usuarioAdmin3 = new Usuario()
            {
                UserName = "rgarcia",
                TipoUsuario = (short)TiposUsuario.Administrador,
                Asesor = asesor11
            };

            Usuario usuario1 = new Usuario()
            {
                UserName = "alayna",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor1
            };
            Usuario usuario2 = new Usuario()
            {
                UserName = "jhernandez",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor2
            }; 
            Usuario usuario3 = new Usuario()
            {
                UserName = "ereyes",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor3
            };
            Usuario usuario4 = new Usuario()
            {
                UserName = "rvillegas",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor4
            };
            Usuario usuario5 = new Usuario()
            {
                UserName = "nbezanilla",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor5
            };
            Usuario usuario6 = new Usuario()
            {
                UserName = "rgomez",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor6
            };
            Usuario usuario7 = new Usuario()
            {
                UserName = "pnavarro",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor7
            };
            Usuario usuario8 = new Usuario()
            {
                UserName = "mmelendez",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor8
            };
            Usuario usuario9 = new Usuario()
            {
                UserName = "fpaniagua",
                TipoUsuario = (short)TiposUsuario.Asesor,
                Asesor = asesor9
            };
            #endregion

            #region Agregar al contexto y actualizar en la BD
            context.Asesores.Add(asesor1);
            context.Asesores.Add(asesor2);
            context.Asesores.Add(asesor3);
            context.Asesores.Add(asesor4);
            context.Asesores.Add(asesor5);
            context.Asesores.Add(asesor6);
            context.Asesores.Add(asesor7);
            context.Asesores.Add(asesor8);
            context.Asesores.Add(asesor9);
            context.Asesores.Add(asesor10);
            context.Asesores.Add(asesor11);
            context.Asesores.Add(asesor12);

            context.Usuarios.Add(usuarioAdmin1);
            context.Usuarios.Add(usuarioAdmin2);
            context.Usuarios.Add(usuarioAdmin3);

            context.Usuarios.Add(usuario1);
            context.Usuarios.Add(usuario2);
            context.Usuarios.Add(usuario3);
            context.Usuarios.Add(usuario4);
            context.Usuarios.Add(usuario5);
            context.Usuarios.Add(usuario6);
            context.Usuarios.Add(usuario7);
            context.Usuarios.Add(usuario8);
            context.Usuarios.Add(usuario9);

            context.OSDetalleStatus.Add(detStatus1);
            context.OSDetalleStatus.Add(detStatus2);
            context.OSDetalleStatus.Add(detStatus3);
            context.OSDetalleStatus.Add(detStatus4);
            context.OSDetalleStatus.Add(detStatus5);

            context.OSStatus.Add(OSStatus1);
            context.OSStatus.Add(OSStatus2);
            context.OSStatus.Add(OSStatus3);
            context.OSStatus.Add(OSStatus4);

            context.TiposProyecto.Add(tipoProy1);
            context.TiposProyecto.Add(tipoProy2);
            context.TiposProyecto.Add(tipoProy3);
            context.TiposProyecto.Add(tipoProy4);

            context.Servicios.Add(serv1);
            context.Servicios.Add(serv2);
            context.Servicios.Add(serv3);
            context.Servicios.Add(serv4);

            context.TiposServicio.Add(tip1);
            context.TiposServicio.Add(tip2);
            context.TiposServicio.Add(tip3);
            context.TiposServicio.Add(tip4);
            context.TiposServicio.Add(tip5);
            context.TiposServicio.Add(tip6);
            context.TiposServicio.Add(tip7);
            context.TiposServicio.Add(tip8);
            context.TiposServicio.Add(tip9);

            context.ViasComunicacion.Add(via1);
            context.ViasComunicacion.Add(via2);
            context.ViasComunicacion.Add(via3);
            context.ViasComunicacion.Add(via4);
            context.ViasComunicacion.Add(via5);

            context.Areas.Add(area1);
            context.Areas.Add(area2);
            context.Areas.Add(area4);
            context.Areas.Add(area5);
            context.Areas.Add(area6);
            context.Areas.Add(area7);
            context.Areas.Add(area8);
            context.Areas.Add(area9);
            context.Areas.Add(area10);
            context.Areas.Add(area11);
            context.Areas.Add(area12);
            context.Areas.Add(area13);
            context.Areas.Add(area14);

            context.Clientes.Add(cliente);
            context.Clientes.Add(cliente2);

            context.Contactos.Add(contacto1);
            context.Contactos.Add(contacto2);
            context.Contactos.Add(contacto3);
            context.Contactos.Add(contacto4);
            context.Contactos.Add(contacto5);
            context.Contactos.Add(contacto6);
            context.Contactos.Add(contacto7);
            context.Contactos.Add(contacto8);
            context.Contactos.Add(contacto9);
            context.Contactos.Add(contacto10);
            context.Contactos.Add(contacto11);
            context.Contactos.Add(contacto12);
            context.Contactos.Add(contacto13);

            context.SaveChanges();
            #endregion

        }
    }
}