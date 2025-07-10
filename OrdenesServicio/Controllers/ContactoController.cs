using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Seguridad;
using System.Web.Security;

namespace ZOE.OrdenesServicio.Controllers
{ 
    public class ContactoController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /Contacto/

        public ViewResult Index()
        {
            var contactos = db.Contactos.Include(c => c.Cliente).OrderBy(o => o.Nombre).ThenBy(o => o.Paterno).ThenBy(o => o.Materno).Include(c => c.Area);
            return View(contactos.ToList());
        }

        //
        // GET: /Contacto/Details/5

        public ViewResult Details(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            return View(contacto);
        }

        //
        // GET: /Contacto/Create

        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(o=>o.Nombre) , "ClienteId", "Nombre");
            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Cliente).ToList(), "AreaId", "AreaDescr");
            
            return View();
        } 

        //
        // POST: /Contacto/Create

        [HttpPost]
        public ActionResult Create(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                //db.Contactos.Add(contacto);
                //db.SaveChanges();

                ZOE.OrdenesServicio.Negocio.Seguridad.CrearContacto(contacto);

                return RedirectToAction("Index");  
            }

            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ClienteId", "Nombre", contacto.ClienteId);
            ViewBag.AreaId = new SelectList(db.Areas.Where(a=>a.TipoArea == (short)TiposArea.Cliente).ToList(), "AreaId", "AreaDescr", contacto.AreaId);
            
            return View(contacto);
        }
        
        //
        // GET: /Contacto/Edit/5
 
        public ActionResult Edit(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            contacto.UserName = "temporal";
            contacto.Password = "temporal";
            contacto.ConfirmPassword = "temporal";
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ClienteId", "Nombre", contacto.ClienteId);
            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Cliente).ToList(), "AreaId", "AreaDescr", contacto.AreaId);
            
            return View(contacto);
        }

        //
        // POST: /Contacto/Edit/5

        [HttpPost]
        public ActionResult Edit(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes.OrderBy(o => o.Nombre), "ClienteId", "Nombre", contacto.ClienteId);
            ViewBag.AreaId = new SelectList(db.Areas.Where(a => a.TipoArea == (short)TiposArea.Cliente).ToList(), "AreaId", "AreaDescr", contacto.AreaId);
            
            return View(contacto);
        }

        //
        // GET: /Contacto/Delete/5
 
        public ActionResult Delete(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            return View(contacto);
        }

        //
        // POST: /Contacto/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Contacto contacto = db.Contactos.Find(id);
            db.Contactos.Remove(contacto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CambiarContrasena(int id)
        {
            Contacto contacto = db.Contactos.Find(id);

            if (db.Usuarios.Where(u => u.ContactoId == id).Any())
            {
                contacto.UserName = db.Usuarios.Where(u => u.ContactoId == id).FirstOrDefault().UserName;
            }
            return View(contacto);
        }

        [HttpPost]
        public ActionResult CambiarContrasena(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                if (UsuarioWeb.GetUser(contacto.UserName) != null)
                    UsuarioWeb.ResetPassword(contacto.UserName, contacto.Password);
                else
                {
                    ZOE.OrdenesServicio.Negocio.Seguridad.CrearUsuarioParaContacto(contacto);
                }
                return RedirectToAction("Index");
            }

            Contacto contactoRegresar = db.Contactos.Find(contacto.ContactoId);
            return View(contactoRegresar);
        }

        [AllowAnonymous]
        public ActionResult CrearCuentas(int id)
        {
            ViewBag.ProcesoMensaje = "Presione el boton 'Procesar' para iniciar";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CrearCuentas()
        {
            //var contactos = db.Contactos.Where(c => c.Email != null && c.ClienteId != 1 && c.ClienteId != 2).OrderBy(o => o.Email).ToList();
            var contactos = db.Contactos.Where(c => c.Email != null && c.ClienteId != 1 && c.ClienteId != 2).OrderBy(o => o.Email).ToList();
            MembershipCreateStatus CreateStatus;
            int contactoId = 0;
      
            try
            {
                foreach (var item in contactos)
                {
                    contactoId = item.ContactoId;
                    Usuario usuario = db.Usuarios.FirstOrDefault(c => c.ContactoId == item.ContactoId);

                    if (usuario != null)
                    {
                        UsuarioWeb.DeleteUser(usuario.UserName);
                        usuario.UserName = item.Email;
                        db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Usuario usuarioNuevo = new Usuario() { ContactoId = item.ContactoId, TipoUsuario = (short)TiposUsuario.Contacto, UserName = item.Email };
                        db.Usuarios.Add(usuarioNuevo);
                    }
                    
                    MembershipUser usuarioMembership = Membership.CreateUser(item.Email, "so2013$" + item.ClienteId, item.Email, null, null, true, null, out CreateStatus);
                    
                    switch (CreateStatus)
                    {
                        case MembershipCreateStatus.DuplicateEmail:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership,emial duplicado");
                        case MembershipCreateStatus.DuplicateProviderUserKey:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, proveedor duplicado");
                        case MembershipCreateStatus.InvalidAnswer:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, respuesta invalida");
                        case MembershipCreateStatus.InvalidEmail:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, email invalido");
                        case MembershipCreateStatus.InvalidPassword:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, contraseña invalida");
                        case MembershipCreateStatus.InvalidProviderUserKey:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, proveedor invalido");
                        case MembershipCreateStatus.InvalidQuestion:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, pregunta invalida");
                        case MembershipCreateStatus.InvalidUserName:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, username invalido");
                        case MembershipCreateStatus.ProviderError:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, error de proveedor");
                        case MembershipCreateStatus.UserRejected:
                            throw new Exception("Ocurrio un error al tratar de crear el usuario membership, usuario rechazado");
                        default:
                            break;
                    }                    

                    UsuarioWeb.AgregarUsuarioRol(item.Email, "Contacto");

                    db.SaveChanges();
                }

                ViewBag.ProcesoMensaje = "Exitoso";
            }
            catch (Exception ex)
            {
                ViewBag.ProcesoMensaje = string.Format("Contactoid {0}, error {1}",  contactoId, ex.Message);
 
            }
            finally
            {                
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}