namespace WmsAllModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FangGoodsMast_BarcodeMast_Upd1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FangGoodsMast",
                c => new
                    {
                        Pd_Id = c.Int(nullable: false, identity: true),
                        PdCode = c.String(nullable: false, maxLength: 20),
                        PdName = c.String(nullable: false, maxLength: 50),
                        DeleteFlag = c.Boolean(),
                        AllowDay = c.Short(nullable: false),
                        CaseQty = c.Short(nullable: false),
                        BoxQty = c.Short(nullable: false),
                        CaseQty_Level = c.Short(nullable: false),
                        HightLevel = c.Short(nullable: false),
                        PickStock = c.String(nullable: false, maxLength: 7),
                        PdLength = c.Short(nullable: false),
                        PdWidth = c.Short(nullable: false),
                        PdHeight = c.Short(nullable: false),
                        PdVolume = c.Double(),
                        PdWeight = c.Short(nullable: false),
                        TemperatureLayer = c.String(nullable: false, maxLength: 1),
                        PdSort = c.String(nullable: false, maxLength: 12),
                        StopSell = c.Boolean(),
                        LowestSellDay = c.Short(),
                        Supplier_1 = c.String(nullable: false, maxLength: 3),
                        Replenish_Online = c.Short(),
                        Replenish_Reserve = c.Short(),
                        CostPriceAve = c.Decimal(precision: 18, scale: 2),
                        LastInPrice = c.Decimal(precision: 18, scale: 2),
                        SuggestPrice = c.Decimal(precision: 18, scale: 2),
                        SuggestPriceAve = c.Decimal(precision: 18, scale: 2),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        SellPriceAve = c.Decimal(precision: 18, scale: 2),
                        UpdTime = c.DateTime(nullable: false),
                        UpdUser = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Pd_Id)
                .Index(t => t.PdCode, name: "IX_FangGoodsMast_PdCode")
                .Index(t => t.PickStock, name: "IX_FangGoodsMast_PickStock");
            
            CreateTable(
                "dbo.GoodMastBarcode",
                c => new
                    {
                        Barcode_Id = c.Int(nullable: false, identity: true),
                        Pd_Id = c.Int(nullable: false),
                        PdCode = c.String(nullable: false, maxLength: 128),
                        Barcode = c.String(nullable: false, maxLength: 20),
                        EaBoxCase = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => new { t.Pd_Id, t.PdCode, t.Barcode })
                .ForeignKey("dbo.FangGoodsMast", t => t.Pd_Id, cascadeDelete: true)
                .Index(t => t.Pd_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodMastBarcode", "Pd_Id", "dbo.FangGoodsMast");
            DropIndex("dbo.GoodMastBarcode", new[] { "Pd_Id" });
            DropIndex("dbo.FangGoodsMast", "IX_FangGoodsMast_PickStock");
            DropIndex("dbo.FangGoodsMast", "IX_FangGoodsMast_PdCode");
            DropTable("dbo.GoodMastBarcode");
            DropTable("dbo.FangGoodsMast");
        }
    }
}
