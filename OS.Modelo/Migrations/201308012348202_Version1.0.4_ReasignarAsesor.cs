namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version104_ReasignarAsesor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BitOSReasignacions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Observacion = c.String(maxLength: 300),
                        Ticket = c.Long(nullable: false),
                        StatusId = c.Short(nullable: false),
                        AsesorAnteriorId = c.Int(nullable: false),
                        AsesorCambioId = c.Int(nullable: false),
                        Usuario_UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId, cascadeDelete: false)
                .ForeignKey("dbo.OrdenServicios", t => t.Ticket, cascadeDelete: false)
                .ForeignKey("dbo.OSStatus", t => t.StatusId, cascadeDelete: false)
                .ForeignKey("dbo.Asesors", t => t.AsesorAnteriorId, cascadeDelete: false)
                .ForeignKey("dbo.Asesors", t => t.AsesorCambioId, cascadeDelete: false)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Ticket)
                .Index(t => t.StatusId)
                .Index(t => t.AsesorAnteriorId)
                .Index(t => t.AsesorCambioId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BitOSReasignacions", new[] { "AsesorCambioId" });
            DropIndex("dbo.BitOSReasignacions", new[] { "AsesorAnteriorId" });
            DropIndex("dbo.BitOSReasignacions", new[] { "StatusId" });
            DropIndex("dbo.BitOSReasignacions", new[] { "Ticket" });
            DropIndex("dbo.BitOSReasignacions", new[] { "Usuario_UsuarioId" });
            DropForeignKey("dbo.BitOSReasignacions", "AsesorCambioId", "dbo.Asesors");
            DropForeignKey("dbo.BitOSReasignacions", "AsesorAnteriorId", "dbo.Asesors");
            DropForeignKey("dbo.BitOSReasignacions", "StatusId", "dbo.OSStatus");
            DropForeignKey("dbo.BitOSReasignacions", "Ticket", "dbo.OrdenServicios");
            DropForeignKey("dbo.BitOSReasignacions", "Usuario_UsuarioId", "dbo.Usuarios");
            DropTable("dbo.BitOSReasignacions");
        }
    }
}
