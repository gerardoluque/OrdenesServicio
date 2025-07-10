using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Seguridad;
using System.Web.Security;

namespace ZOE.OrdenesServicio.Negocio
{
    public static class Seguridad
    {
        public static void CrearUsuarioParaContacto(Contacto contacto)
        {
            using (OSContext ctx = new OSContext())
            {
                System.Web.Security.MembershipCreateStatus status = UsuarioWeb.Register(contacto.UserName, contacto.Password, contacto.Email, true);

                if (status == System.Web.Security.MembershipCreateStatus.Success)
                {
                    Usuario usuarioNuevo = new Usuario()
                    {
                        TipoUsuario = (short)TiposUsuario.Contacto,
                        ContactoId = contacto.ContactoId,
                        UserName = contacto.UserName
                    };

                    ctx.Usuarios.Add(usuarioNuevo);

                    ctx.SaveChanges();
                }
            }
        }

        public static void CrearContacto(Contacto contacto)
        {
            using (OSContext ctx = new OSContext())
            {
                System.Web.Security.MembershipCreateStatus status = UsuarioWeb.Register(contacto.UserName, contacto.Password, contacto.Email, true);

                if (status == System.Web.Security.MembershipCreateStatus.Success)
                {
                    UsuarioWeb.AgregarUsuarioRol(contacto.UserName, "contacto");

                    Usuario usuarioNuevo = new Usuario()
                    {
                        TipoUsuario = (short)TiposUsuario.Contacto,
                        ContactoId = contacto.ContactoId,
                        UserName = contacto.UserName
                    };

                    usuarioNuevo.Contacto = contacto;
                    ctx.Usuarios.Add(usuarioNuevo);

                    ctx.SaveChanges();
                }
            }
        }

        public static void CrearAsesor(Asesor asesor)
        {
            using (OSContext ctx = new OSContext())
            {
                System.Web.Security.MembershipCreateStatus status = UsuarioWeb.Register(asesor.UserName, asesor.Password, asesor.Email, true);

                if (status == System.Web.Security.MembershipCreateStatus.Success)
                {
                    UsuarioWeb.AgregarUsuarioRol(asesor.UserName, "asesor");
                    Usuario usuarioNuevo = new Usuario()
                    {
                        TipoUsuario = (short)TiposUsuario.Asesor,
                        AsesorId = asesor.AsesorId,
                        UserName = asesor.UserName
                    };

                    usuarioNuevo.Asesor = asesor;
                    ctx.Usuarios.Add(usuarioNuevo);

                    ctx.SaveChanges();
                }                
            }
        }

        public static Usuario ObtenerUsuario(string userName)
        {
            Usuario user;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                ctx.Configuration.ProxyCreationEnabled = false;
                user = ctx.Usuarios.Include("Asesor").FirstOrDefault(p => p.UserName == userName);
            }

            return user;
        }

        public static List<Usuario> ObtenerUsuarios(string userName)
        {
            List<Usuario> users;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                users = ctx.Usuarios.Where(p => p.UserName == userName).ToList();
            }

            return users;
        }

        public static List<Accion> ObtenerPivilegiosAcceso(string userName)
        {
            List<Accion> acciones = new List<Accion>();

            string[] userroles = Roles.GetRolesForUser(userName);
            string roleName = string.Empty;

            if (userroles.Length > 0)
            {
                roleName = userroles[0];
                using (OSContext ctx = new OSContext())
                {
                   acciones = ctx.Acciones.Where(p => p.RolAcciones.Any(r => r.RoleName == roleName)).ToList();
                }
            }

            return acciones;
            
        }


        public static IQueryable ObtenerEmpresasPorUserName(string nombreUsuario)
        {
            using (OSContext ctx = new OSContext())
            {
                //var empresas = ctx.Clientes.Where(c => c.Contactos.Any(p => p.UserName == nombreUsuario));
                //return empresas;
                ctx.Configuration.ProxyCreationEnabled = false;

                var empresas = from cont in ctx.Contactos
                               join usr in ctx.Usuarios 
                               on cont.ContactoId equals usr.ContactoId 
                               where usr.UserName == nombreUsuario
                               select new { ContactoId = cont.ContactoId, Empresa = cont.Cliente.Nombre };

                return empresas.ToList().AsQueryable();
            }

        }

        internal static Usuario ObtenerUsuarioPorContactoId(int contactoId)
        {
            Usuario user;

            using (OSContext ctx = new OSContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                ctx.Configuration.ProxyCreationEnabled = false;
                user = ctx.Usuarios.Include("Contacto").FirstOrDefault(p => p.ContactoId == contactoId);
            }

            return user;
        }
    }

    //public class ResultadoCrearUsuario
    //{
    //    public int UsuarioId { get; set; }
    //    public Guid UserId { get; set; }
    //}
}