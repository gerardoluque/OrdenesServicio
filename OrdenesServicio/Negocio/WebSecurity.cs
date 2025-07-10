using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ZOE.OrdenesServicio.Seguridad
{
    public sealed class UsuarioWeb
    {
        public static HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public static HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        public static HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        public static System.Security.Principal.IPrincipal User
        {
            get { return Context.User; }
        }

        public static bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        public static MembershipCreateStatus Register(string Username, string Password, string Email, bool IsApproved)
        {
            MembershipCreateStatus CreateStatus;
            Membership.CreateUser(Username, Password, Email, null, null, IsApproved, null, out CreateStatus);

            //if (CreateStatus == MembershipCreateStatus.Success)
            //{
            //    if (IsApproved)
            //    {
            //        FormsAuthentication.SetAuthCookie(Username, false);
            //    }
            //}

            return CreateStatus;
        }

        public enum MembershipLoginStatus
        {
            Success, Failure
        }

        public static MembershipLoginStatus Login(string Username, string Password, bool RememberMe)
        {
            if (Membership.ValidateUser(Username, Password))
            {
                FormsAuthentication.SetAuthCookie(Username, RememberMe);
                return MembershipLoginStatus.Success;
            }
            else
            {
                return MembershipLoginStatus.Failure;
            }
        }

        public static void ResetPassword(string userName, string newPass)
        {
            MembershipUser usuario = Membership.GetUser(userName);
            if (usuario.IsLockedOut)
                usuario.UnlockUser();
            string passReset = usuario.ResetPassword();
            usuario.ChangePassword(passReset, newPass);
        }

        public static void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public static MembershipUser GetUser(string Username)
        {
            return Membership.GetUser(Username);
        }

        public static bool ChangePassword(string OldPassword, string NewPassword)
        {
            MembershipUser CurrentUser = Membership.GetUser(User.Identity.Name);
            return CurrentUser.ChangePassword(OldPassword, NewPassword);
        }

        public static bool DeleteUser(string Username)
        {
            return Membership.DeleteUser(Username);
        }

        public static List<MembershipUser> FindUsersByEmail(string Email, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByEmail(Email, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> FindUsersByName(string Username, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByName(Username, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> GetAllUsers(int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.GetAllUsers(PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static void AgregarUsuarioRol(string userName, string rolName)
        {
            if (!Roles.IsUserInRole(userName, rolName))
                Roles.AddUserToRole(userName, rolName);
        }
    }
}