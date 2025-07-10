namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version101 : DbMigration
    {
        public override void Up()
        {

            Sql("UPDATE dbo.OSDetalles SET ViaComunicacionId=3 WHERE ViaComunicacionId IS NULL");

            DropForeignKey("dbo.OSDetalles", "ViaComunicacionId", "dbo.ViaComunicacions");
            DropIndex("dbo.OSDetalles", new[] { "ViaComunicacionId" });
            AddColumn("dbo.Proyectoes", "TipoCliente", c => c.Short());
            AddColumn("dbo.OrdenServicios", "TipoCliente", c => c.Short());
            AddColumn("dbo.OSNotas", "PermitirVerCliente", c => c.Boolean(nullable: false));
            AlterColumn("dbo.OrdenServicios", "AsesorId", c => c.Int());
            AlterColumn("dbo.OrdenServicios", "AreaRespId", c => c.Short());
            AlterColumn("dbo.OSDetalles", "DetalleDescr", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.OSDetalles", "ViaComunicacionId", c => c.Short(nullable: false));
            AddForeignKey("dbo.OSDetalles", "ViaComunicacionId", "dbo.ViaComunicacions", "ViaComId", cascadeDelete: false);
            CreateIndex("dbo.OSDetalles", "ViaComunicacionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OSDetalles", new[] { "ViaComunicacionId" });
            DropForeignKey("dbo.OSDetalles", "ViaComunicacionId", "dbo.ViaComunicacions");
            AlterColumn("dbo.OSDetalles", "ViaComunicacionId", c => c.Short());
            AlterColumn("dbo.OSDetalles", "DetalleDescr", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.OrdenServicios", "AreaRespId", c => c.Short(nullable: false));
            AlterColumn("dbo.OrdenServicios", "AsesorId", c => c.Int(nullable: false));
            DropColumn("dbo.OSNotas", "PermitirVerCliente");
            DropColumn("dbo.OrdenServicios", "TipoCliente");
            DropColumn("dbo.Proyectoes", "TipoCliente");
            CreateIndex("dbo.OSDetalles", "ViaComunicacionId");
            AddForeignKey("dbo.OSDetalles", "ViaComunicacionId", "dbo.ViaComunicacions", "ViaComId");
        }
    }
}
