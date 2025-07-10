using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZOE.OS.Modelo;
using System.Data;

namespace ZOE.OrdenesServicio.Negocio
{
    public class ProyectoBC
    {
        public static void Abonar(ProyectoAbono abono)
        {
            using (OSContext ctx = new OSContext())
            {
                Proyecto proyecto = ctx.Proyectos.Find(abono.ProyectoId);
                
                abono.MinutosAntesAbono = proyecto.Saldo;
                proyecto.Saldo = proyecto.Saldo + abono.Minutos;

                ctx.ProyectoAbonos.Add(abono);
                ctx.SaveChanges();
            }
        }

        public static List<ProyectoAbono> ObtenerAbonos(int id)
        {
            List<ProyectoAbono> abonos = new List<ProyectoAbono>();
            
            using (OSContext ctx = new OSContext())
            {
                abonos = ctx.ProyectoAbonos.Where(p => p.ProyectoId == id).OrderBy(o => o.Fecha).ToList();
            }

            return abonos;
        }

        public static List<ProyectoAbono> ObtenerAbonosPorCliente(int clienteId)
        {
            List<ProyectoAbono> abonos = new List<ProyectoAbono>();

            using (OSContext ctx = new OSContext())
            {
                abonos = ctx.ProyectoAbonos.Where(p => p.Proyecto.ClienteId == clienteId).OrderBy(o => o.Fecha).ToList();
            }

            return abonos;
        }

        internal static void ActualizarAbono(ProyectoAbono abono)
        {
            using (OSContext ctx = new OSContext())
            {
                Proyecto proyecto = ctx.Proyectos.Find(abono.ProyectoId);

                proyecto.Saldo = abono.MinutosAntesAbono + abono.Minutos;

                ctx.Entry(proyecto).State = System.Data.Entity.EntityState.Modified;
                ctx.Entry(abono).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static Int32 ObtenerSaldo(int proyectoId)
        {
            Int32 saldo = 0;

            using (OSContext ctx = new OSContext())
            {
                Int32 cargos = ctx.DetallesOrdenServicio.Any(p => p.OrdenServicio.ProyectoId == proyectoId && p.AfectaPoliza == "S") ? ctx.DetallesOrdenServicio.Where(p => p.OrdenServicio.ProyectoId == proyectoId && p.AfectaPoliza == "S" && p.StatusId != 5).Sum(p => p.Minutos) : 0;
                Int32 abonos = ctx.ProyectoAbonos.Any(p => p.ProyectoId == proyectoId) ? ctx.ProyectoAbonos.Where(p => p.ProyectoId == proyectoId).Sum(p => p.Minutos) : 0;

                saldo = abonos - cargos;
            }

            return saldo;
        }
    }
}