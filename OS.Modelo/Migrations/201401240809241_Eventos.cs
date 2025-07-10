namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eventos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Eventoes",
                c => new
                    {
                        EventoId = c.Short(nullable: false, identity: true),
                        EventoDescr = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.EventoId);
            
            CreateTable(
                "dbo.EventoAsistentes",
                c => new
                    {
                        EventoAsistenteId = c.Short(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 100),
                        EventoId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.EventoAsistenteId)
                .ForeignKey("dbo.Eventoes", t => t.EventoId, cascadeDelete: true)
                .Index(t => t.EventoId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.EventoAsistentes", new[] { "EventoId" });
            DropForeignKey("dbo.EventoAsistentes", "EventoId", "dbo.Eventoes");
            DropTable("dbo.EventoAsistentes");
            DropTable("dbo.Eventoes");
        }
    }
}
