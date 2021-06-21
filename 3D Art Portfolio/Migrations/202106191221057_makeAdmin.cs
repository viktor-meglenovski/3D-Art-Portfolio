namespace _3D_Art_Portfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeAdmin : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('b6daf09b-c614-4a29-8066-8511a231ad97', '1')");
        }
        
        public override void Down()
        {
        }
    }
}
