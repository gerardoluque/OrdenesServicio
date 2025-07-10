namespace ZOE.OS.Modelo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarMinutosTickets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenServicios", "Minutos", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdenServicios", "Minutos");
        }
    }
}
