namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastOnline", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Blocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Blocked");
            DropColumn("dbo.AspNetUsers", "LastOnline");
            DropColumn("dbo.AspNetUsers", "RegDate");
        }
    }
}
