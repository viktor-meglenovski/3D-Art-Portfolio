namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectSoftwareTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectSoftwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        SoftwareId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectSoftwares");
        }
    }
}
