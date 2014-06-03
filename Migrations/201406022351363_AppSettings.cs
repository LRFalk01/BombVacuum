namespace BombVacuum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AppSettings",
                c => new
                    {
                        Property = c.String(nullable: false, maxLength: 50, unicode: false),
                        Value = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Property)                
                .Index(t => t.Property);
            
        }
        
        public override void Down()
        {
            DropIndex("AppSettings", new[] { "Property" });
            DropTable("AppSettings");
        }
    }
}
