namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreModelsAdded1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectEntries", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectEntries", "UserId", c => c.Int(nullable: false));
        }
    }
}
