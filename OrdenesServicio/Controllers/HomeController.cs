using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;
using Infragistics.Web.Mvc;
using ZOE.OrdenesServicio.Negocio;
using System.Web.UI;
using System.Net.Mail;
using System.Text;
using ZOE.OrdenesServicio.Controllers;

namespace OrdenesServicio.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenidos a ZOE IT Customs / Solicitudes de Ordenes de Servicio";

            
            //var items = ZOE.OrdenesServicio.Negocio.OrdenServicioBC.ObtenerAreas();

            return View();
        }

        //[OutputCache(Duration = 7200, VaryByParam = "none")]
        public ActionResult MostrarOpciones()
        {
            var acceso = Seguridad.ObtenerPivilegiosAcceso(User.Identity.Name) ;
            return View("PanelControl", acceso);
        }

        public ActionResult About()
        {
            return View();
        }


        [HttpPost]
        public ActionResult About(string id)
        {
            SmtpClient client;
            //System.Net.NetworkCredential credenciales;
            MailMessage message;

            //credenciales = new System.Net.NetworkCredential("servicio@zoeitcustoms.com", "MzCK89%%");

            client = new SmtpClient();
            client.Host = "maila46.webcontrolcenter.com";
            //client.Port = 3535;
            //client.Credentials = credenciales;

            message = new MailMessage();
            //message.From = new MailAddress("servicio@zoeitcustoms.com", "Zoe IT Customs/Sistema de Ordenes de Servicio");
            message.From = new MailAddress("sozoeitcustoms@so.zoeitcustoms.com", "Zoe IT Customs/Sistema de Ordenes de Servicio");
            message.To.Add(new MailAddress("gluque@zoeitcustoms.com", "Gerardo Luque"));

            message.Subject = string.Format("Zoe Sistema de Ordenes de Servicio, Ticket No. {0}", 5);

            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("Este correo es informativo, Favor de no responder a esta cuenta de correo !");

            message.Body = sb.ToString();

            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

            //client.SendAsync(message, "Enviando");
            try
            {
                client.SendAsync(message,"Enviando");
            }
            catch (Exception ex)
            {
                string error = string.Empty;

                error = ex.Message + ", " + ex.ToString();

                if (ex.InnerException != null)
                    error += ", " + ex.InnerException.Message;
                                
                ViewBag.Error = error;
               
            }
            
            return View();

        }

        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ViewBag.Error = e.Error.Message;
                RedirectToAction("About");
            }
            else
                ViewBag.Error = "Exito";
            
        }
    }
}
