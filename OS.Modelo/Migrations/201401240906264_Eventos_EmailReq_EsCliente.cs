namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eventos_EmailReq_EsCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventoAsistentes", "EsCliente", c => c.Boolean(nullable: false));
            AlterColumn("dbo.EventoAsistentes", "Email", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventoAsistentes", "Email", c => c.String(maxLength: 100));
            DropColumn("dbo.EventoAsistentes", "EsCliente");
        }
    }
}
