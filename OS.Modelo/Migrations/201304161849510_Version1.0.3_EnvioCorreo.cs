namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version103_EnvioCorreo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grupoes",
                c => new
                    {
                        GrupoId = c.Short(nullable: false, identity: true),
                        GrupoDescr = c.String(nullable: false, maxLength: 50),
                        GrupoTipo = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.GrupoId);
            
            CreateTable(
                "dbo.GrupoAsesors",
                c => new
                    {
                        GrupoId = c.Short(nullable: false),
                        AsesorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GrupoId, t.AsesorId })
                .ForeignKey("dbo.Asesors", t => t.AsesorId, cascadeDelete: false)
                .ForeignKey("dbo.Grupoes", t => t.GrupoId, cascadeDelete: false)
                .Index(t => t.AsesorId)
                .Index(t => t.GrupoId);
            
            CreateTable(
                "dbo.ParamCorreos",
                c => new
                    {
                        ParamId = c.Short(nullable: false, identity: true),
                        Host = c.String(nullable: false, maxLength: 100),
                        PuertoSalida = c.Short(nullable: false),
                        CuentaAcceso = c.String(nullable: false, maxLength: 100),
                        ContrasenaAcceso = c.String(nullable: false, maxLength: 100),
                        From = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ParamId);
            
            AddColumn("dbo.Asesors", "Grupo_GrupoId", c => c.Short());
            AddForeignKey("dbo.Asesors", "Grupo_GrupoId", "dbo.Grupoes", "GrupoId");
            CreateIndex("dbo.Asesors", "Grupo_GrupoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GrupoAsesors", new[] { "GrupoId" });
            DropIndex("dbo.GrupoAsesors", new[] { "AsesorId" });
            DropIndex("dbo.Asesors", new[] { "Grupo_GrupoId" });
            DropForeignKey("dbo.GrupoAsesors", "GrupoId", "dbo.Grupoes");
            DropForeignKey("dbo.GrupoAsesors", "AsesorId", "dbo.Asesors");
            DropForeignKey("dbo.Asesors", "Grupo_GrupoId", "dbo.Grupoes");
            DropColumn("dbo.Asesors", "Grupo_GrupoId");
            DropTable("dbo.ParamCorreos");
            DropTable("dbo.GrupoAsesors");
            DropTable("dbo.Grupoes");
        }
    }
}
