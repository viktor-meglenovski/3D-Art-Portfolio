namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class likeModelChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Likes", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Likes", "UserId", c => c.Int(nullable: false));
        }
    }
}
