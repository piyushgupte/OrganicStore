namespace OrganicStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryProductAdded1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
        }
    }
}
