namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenServicios", "UsuarioIdRegistro", c => c.Int());
            AddForeignKey("dbo.OrdenServicios", "UsuarioIdRegistro", "dbo.Usuarios", "UsuarioId");
            CreateIndex("dbo.OrdenServicios", "UsuarioIdRegistro");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrdenServicios", new[] { "UsuarioIdRegistro" });
            DropForeignKey("dbo.OrdenServicios", "UsuarioIdRegistro", "dbo.Usuarios");
            DropColumn("dbo.OrdenServicios", "UsuarioIdRegistro");
        }
    }
}
