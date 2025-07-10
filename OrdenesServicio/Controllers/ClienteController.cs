using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZOE.OS.Modelo;

namespace ZOE.OrdenesServicio.Controllers
{ 
    public class ClienteController : BaseController
    {
        private OSContext db = new OSContext();

        //
        // GET: /Cliente/

        public ViewResult Index()
        {
            return View(db.Clientes.OrderBy(o=>o.Nombre).ToList());
        }

        //
        // GET: /Cliente/Details/5

        public ViewResult Details(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            return View(cliente);
        }

        //
        // GET: /Cliente/Create

        public ActionResult Create()
        {
            Cliente cliente = new Cliente();
            return View(cliente);
        } 

        //
        // POST: /Cliente/Create

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);

                cliente.TicketPrioridades.Add(new 
                    ClientePrioridad 
                    { Prioridad = (int)Prioridades.MuyAlto,
                        Horas = cliente.ClientePrioridadesHoras.MuyAlto,
                    });

                cliente.TicketPrioridades.Add(new
                    ClientePrioridad
                {
                    Prioridad = (int)Prioridades.Alto,
                    Horas = cliente.ClientePrioridadesHoras.Alto,
                });

                cliente.TicketPrioridades.Add(new
                    ClientePrioridad
                {
                    Prioridad = (int)Prioridades.Medio,
                    Horas = cliente.ClientePrioridadesHoras.Medio,
                });

                cliente.TicketPrioridades.Add(new
                    ClientePrioridad
                {
                    Prioridad = (int)Prioridades.Bajo,
                    Horas = cliente.ClientePrioridadesHoras.Bajo,
                });

                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(cliente);
        }
        
        //
        // GET: /Cliente/Edit/5
 
        public ActionResult Edit(int id)
        {
            Cliente cliente =  db
            .Clientes
            .Include("TicketPrioridades")
            .FirstOrDefault(x => x.ClienteId == id);

            if (cliente.TicketPrioridades.Any())
            {
                cliente.ClientePrioridadesHoras.MuyAlto = cliente
                    .TicketPrioridades
                    .Where(t => t.Prioridad == (int)Prioridades.MuyAlto)
                    .FirstOrDefault().Horas;
                cliente.ClientePrioridadesHoras.Alto = cliente
                    .TicketPrioridades
                    .Where(t => t.Prioridad == (int)Prioridades.Alto)
                    .FirstOrDefault().Horas;
                cliente.ClientePrioridadesHoras.Medio = cliente
                    .TicketPrioridades
                    .Where(t => t.Prioridad == (int)Prioridades.Medio)
                    .FirstOrDefault().Horas;
                cliente.ClientePrioridadesHoras.Bajo = cliente
                    .TicketPrioridades
                    .Where(t => t.Prioridad == (int)Prioridades.Bajo)
                    .FirstOrDefault().Horas;
            }

            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            var clienteMod = cliente;

            if (ModelState.IsValid)
            {
                //db.Entry(clienteMod).State = System.Data.Entity.EntityState.Modified;
                clienteMod = db
                    .Clientes
                    .Include("TicketPrioridades")
                    .FirstOrDefault(x => x.ClienteId == cliente.ClienteId);

                clienteMod.Tipo = cliente.Tipo;
                clienteMod.Nombre = cliente.Nombre;
                clienteMod.Email = cliente.Email;
                clienteMod.Prioritario = cliente.Prioritario;
                clienteMod.Direccion = cliente.Direccion;
                clienteMod.Rfc = cliente.Rfc;
                clienteMod.Telefono = cliente.Telefono;
                clienteMod.TipoCliente = cliente.TipoCliente;
                clienteMod.SLAPorcentaje = cliente.SLAPorcentaje;

                if (clienteMod.TicketPrioridades.Exists(x => x.Prioridad == (int)Prioridades.MuyAlto))
                {
                    clienteMod.TicketPrioridades
                        .Where(x => x.Prioridad == (int)Prioridades.MuyAlto)
                        .FirstOrDefault().Horas = cliente.ClientePrioridadesHoras.MuyAlto;
                }
                else
                {
                    clienteMod.TicketPrioridades.Add(new ClientePrioridad
                    {
                        Prioridad = (int)Prioridades.MuyAlto,
                        Horas = cliente.ClientePrioridadesHoras.MuyAlto,
                    });
                }


                if (clienteMod.TicketPrioridades.Exists(x => x.Prioridad == (int)Prioridades.Alto))
                {
                    clienteMod.TicketPrioridades
                        .Where(x => x.Prioridad == (int)Prioridades.Alto)
                        .FirstOrDefault().Horas = cliente.ClientePrioridadesHoras.Alto;
                }
                else
                {
                    clienteMod.TicketPrioridades.Add(new ClientePrioridad
                    {
                        Prioridad = (int)Prioridades.Alto,
                        Horas = cliente.ClientePrioridadesHoras.Alto,
                    });
                }

                if (clienteMod.TicketPrioridades.Exists(x => x.Prioridad == (int)Prioridades.Medio))
                {
                    clienteMod.TicketPrioridades
                        .Where(x => x.Prioridad == (int)Prioridades.Medio)
                        .FirstOrDefault().Horas = cliente.ClientePrioridadesHoras.Medio;
                }
                else
                {
                    clienteMod.TicketPrioridades.Add(new ClientePrioridad
                    {
                        Prioridad = (int)Prioridades.Medio,
                        Horas = cliente.ClientePrioridadesHoras.Medio,
                    });
                }

                if (clienteMod.TicketPrioridades.Exists(x => x.Prioridad == (int)Prioridades.Bajo))
                {
                    clienteMod.TicketPrioridades
                        .Where(x => x.Prioridad == (int)Prioridades.Bajo)
                        .FirstOrDefault().Horas = cliente.ClientePrioridadesHoras.Bajo;
                }
                else
                {
                    clienteMod.TicketPrioridades.Add(new ClientePrioridad
                    {
                        Prioridad = (int)Prioridades.Bajo,
                        Horas = cliente.ClientePrioridadesHoras.Bajo,
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clienteMod);
        }

        //
        // GET: /Cliente/Delete/5
 
        public ActionResult Delete(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            return View(cliente);
        }

        //
        // POST: /Cliente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}