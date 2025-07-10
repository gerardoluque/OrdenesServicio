namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ZOE.OS.Modelo.OSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ZOE.OS.Modelo.OSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            
            //Grupo grupo = new Grupo() { GrupoDescr = "Asesores Reciben Correo al crear ticket" };
            //context.Grupos.Add(grupo);
            
            //context.GrupoAsesores.Add(new GrupoAsesor() { Grupo = grupo, GrupoId = 1, AsesorId = 1 });
            //context.GrupoAsesores.Add(new GrupoAsesor() { Grupo = grupo, GrupoId = 1, AsesorId = 2 });
            //context.GrupoAsesores.Add(new GrupoAsesor() { Grupo = grupo, GrupoId = 1, AsesorId = 3 });
            //context.GrupoAsesores.Add(new GrupoAsesor() { Grupo = grupo, GrupoId = 1, AsesorId = 6 });
            //context.GrupoAsesores.Add(new GrupoAsesor() { Grupo = grupo, GrupoId = 1, AsesorId = 13 });
            
            //context.ParametrosCorreo.Add(new ParamCorreo() { Host = "mail.zoeitcustoms.com", PuertoSalida = 3535, From = "servicio@zoeitcustoms.com", CuentaAcceso = "servicio@zoeitcustoms.com", ContrasenaAcceso = "MzCK89%%" });

            //context.SaveChanges();
        }
    }
}
