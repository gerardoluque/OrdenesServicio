using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ZOE.OrdenesServicio.Models;
using ZOE.OrdenesServicio.Negocio;
using ZOE.OS.Modelo;
using ZOE.OrdenesServicio.Controllers;
using OrdenesServicio.Filters;

namespace OrdenesServicio.Controllers
{    
    public class AccountController : BaseController
    {

        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            //throw new Exception("Error en el logon");
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.UserName.Trim();
                model.Password = model.Password.Trim();

                MembershipUser user = Membership.GetUser(model.UserName);

                if (user != null)
                {
                    if (!user.IsLockedOut)
                    {
                        if (Membership.ValidateUser(model.UserName, model.Password))
                        {
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                            List<Usuario> users = Seguridad.ObtenerUsuarios(model.UserName);

                            if (users.Count > 0)
                            {
                                switch (users.FirstOrDefault().TipoUsuario)
                                {
                                    case (short)TiposUsuario.MesaAsignacion:
                                    case (short)TiposUsuario.Asesor:
                                        Session["Conexion"] = "OSContext";
                                        return RedirectToAction("ListaServiciosPropias", "AsesorSeguimiento");
                                    case (short)TiposUsuario.Contacto:
                                        if (users.Count > 1)
                                            Session["ContactoId"] = model.ContactoId;
                                        else
                                            Session["ContactoId"] = users.First().ContactoId;

                                        return RedirectToAction("ListaServiciosPropias", "ContactoSeguimiento");

                                    default:
                                        return RedirectToAction("ListaServiciosPropias", "AsesorSeguimiento");
                                }
                            }
                            else
                                ModelState.AddModelError("AccesoDenegado", ZOE.OrdenesServicio.Views.Account.AccountResource.LogonUsuarioNoExiste);
                        }
                        else
                        {
                            ModelState.AddModelError("AccesoDenegado", ZOE.OrdenesServicio.Views.Account.AccountResource.LogonSummaryError);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LoginBloqueado", ZOE.OrdenesServicio.Views.Account.AccountResource.LogonBloqueado);
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginNoExiste", ZOE.OrdenesServicio.Views.Account.AccountResource.LogonUsuarioNoExiste);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ObtenerDatosUsuario()
        {
            Usuario usuario;

            if (Session["ContactoId"] != null)
            {
                usuario = Seguridad.ObtenerUsuarioPorContactoId(Convert.ToInt32(Session["ContactoId"]));
                var datosUsuario = new { Nombre = usuario.Contacto.NombreCompleto, Empresa = usuario.Contacto.Cliente.Nombre };
                return Json(new { datosUsuario, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                usuario = Seguridad.ObtenerUsuario(User.Identity.Name);
                var datosUsuario = new { Nombre = usuario.Asesor.NombreCompleto, Empresa = "ZOE IT Customs" };
                return Json(new { datosUsuario, success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult VerificarUsuarioInstancias(string nombreUsuario)
        {
            List<Usuario> users = Seguridad.ObtenerUsuarios(nombreUsuario);
            return Json( new { users.Count, success = true }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ObtenerEmpresasPorUserName(string nombreUsuario)
        {
            var empresas = Seguridad.ObtenerEmpresasPorUserName(nombreUsuario);

            return Json(new { empresas, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return View("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "La contraseña actual es incorrecta, o la nueva contraseña es invalida.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
