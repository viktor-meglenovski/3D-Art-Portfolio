namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Images", "ProjectId");
            AddForeignKey("dbo.Images", "ProjectId", "dbo.ProjectEntries", "ProjectId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "ProjectId", "dbo.ProjectEntries");
            DropIndex("dbo.Images", new[] { "ProjectId" });
        }
    }
}
