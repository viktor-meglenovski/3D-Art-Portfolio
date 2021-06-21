namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class softwareModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Softwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        ProjectEntry_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectEntries", t => t.ProjectEntry_ProjectId)
                .Index(t => t.ProjectEntry_ProjectId);
            
            AddColumn("dbo.ProjectEntries", "TimeStamp", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Softwares", "ProjectEntry_ProjectId", "dbo.ProjectEntries");
            DropIndex("dbo.Softwares", new[] { "ProjectEntry_ProjectId" });
            DropColumn("dbo.ProjectEntries", "TimeStamp");
            DropTable("dbo.Softwares");
        }
    }
}
