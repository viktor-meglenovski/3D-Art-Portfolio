namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectEntries", "Name", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.ProjectEntries", "Description", c => c.String(nullable: false, maxLength: 160));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectEntries", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectEntries", "Name", c => c.String(nullable: false));
        }
    }
}
