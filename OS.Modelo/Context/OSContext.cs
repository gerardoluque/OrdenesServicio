using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ZOE.OS.Modelo.Model;


namespace ZOE.OS.Modelo
{
    public class OSContext : DbContext
    {
        public OSContext() : base()
        {
            base.Database.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OSContext"].ConnectionString;
            Database.SetInitializer<OSContext>(null);
        }
        public OSContext(string connectionStringName) : base(connectionStringName)
        {
            base.Database.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<TipoProyecto> TiposProyecto { get; set; }
        public DbSet<ProyectoAbono> ProyectoAbonos { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Asesor> Asesores { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<TipoServicio> TiposServicio { get; set; }
        public DbSet<ViaComunicacion> ViasComunicacion { get; set; }
        public DbSet<OSStatus> OSStatus { get; set; }
        public DbSet<OSDetalleStatus> OSDetalleStatus { get; set; }
        public DbSet<OrdenServicio> OrdenesServicio { get; set; }
        public DbSet<OSDetalle> DetallesOrdenServicio { get; set; }
        public DbSet<OSNota> NotasOrdenServicio { get; set; }
        public DbSet<BitOSStatus> BitacoraStatusOrdenServicio { get; set; }
        public DbSet<BitOSDetalleStatus> BitacoraStatusDetalleOrdenServicio { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<RolAccion> RolAcciones { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<GrupoAsesor> GrupoAsesores { get; set; }
        public DbSet<ParamCorreo> ParametrosCorreo { get; set; }
        public DbSet<BitOSReasignacion> BitacoraReasignacion { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<EventoAsistente> EventoAsistente { get; set; }
        public DbSet<ClientePrioridad> ClientePrioridades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOptional(p => p.Asesor)
                .WithMany()
                .HasForeignKey(c => c.AsesorId);

            modelBuilder.Entity<Usuario>()
                .HasOptional(p => p.Contacto)
                .WithMany()
                .HasForeignKey(c => c.ContactoId);

            modelBuilder.Entity<BitOSDetalleStatus>()
                .HasRequired(p => p.StatusAnterior)
                .WithMany()
                .HasForeignKey(c => c.StatusAnteriorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSDetalleStatus>()
                        .HasRequired(p => p.StatusCambio)
                        .WithMany()
                        .HasForeignKey(c => c.StatusCambioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSStatus>()
                .HasRequired(p => p.StatusAnterior)
                .WithMany()
                .HasForeignKey(c => c.StatusAnteriorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSStatus>()
                        .HasRequired(p => p.StatusCambio)
                        .WithMany()
                        .HasForeignKey(c => c.StatusCambioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSStatus>()
                        .HasRequired(p => p.OrdenServicio)
                        .WithMany()
                        .HasForeignKey(c => c.Ticket).WillCascadeOnDelete(false);

            //modelBuilder.Entity<OrdenServicio>()
            //            .HasRequired(p => p.Asesor)
            //            .WithMany()
            //            .HasForeignKey(c => c.AsesorId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<OrdenServicio>()
            //            .HasRequired(p => p.AreaResponsable)
            //            .WithMany()
            //            .HasForeignKey(c => c.AreaRespId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<OrdenServicio>()
            //            .HasRequired(p => p.Proyecto)
            //            .WithMany()
            //            .HasForeignKey(c => c.ProyectoId).WillCascadeOnDelete(false);

            modelBuilder.Entity<OSDetalle>()
                        .HasRequired(p => p.Contacto)
                        .WithMany()
                        .HasForeignKey(c => c.ContactoId).WillCascadeOnDelete(false);

            modelBuilder.Entity<OSDetalle>()
                        .HasRequired(p => p.Asesor)
                        .WithMany()
                        .HasForeignKey(c => c.AsesorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<OSDetalle>()
                        .HasRequired(p => p.AreaResponsable)
                        .WithMany()
                        .HasForeignKey(c => c.AreaRespId).WillCascadeOnDelete(false);

            modelBuilder.Entity<ProyectoAbono>()
                        .HasRequired(p => p.Usuario)
                        .WithMany()
                        .HasForeignKey(c => c.UsuarioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<GrupoAsesor>()
                        .HasRequired(p => p.Asesor)
                        .WithMany()
                        .HasForeignKey(c => c.AsesorId).WillCascadeOnDelete(false);


            modelBuilder.Entity<BitOSReasignacion>()
                .HasRequired(p => p.AsesorAnterior)
                .WithMany()
                .HasForeignKey(c => c.AsesorAnteriorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSReasignacion>()
                .HasRequired(p => p.AsesorCambio)
                .WithMany()
                .HasForeignKey(c => c.AsesorCambioId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSReasignacion>()
                        .HasRequired(p => p.OrdenServicio)
                        .WithMany()
                        .HasForeignKey(c => c.Ticket).WillCascadeOnDelete(false);

            modelBuilder.Entity<BitOSReasignacion>()
                .HasRequired(p => p.Status)
                .WithMany()
                .HasForeignKey(c => c.StatusId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
