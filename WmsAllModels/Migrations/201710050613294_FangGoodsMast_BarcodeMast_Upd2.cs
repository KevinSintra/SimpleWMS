namespace WmsAllModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FangGoodsMast_BarcodeMast_Upd2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FangGoodsMast", "UpdUser", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FangGoodsMast", "UpdUser", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
