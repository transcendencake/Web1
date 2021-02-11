namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationDBA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AspNetUsers", "Blocked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Blocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
    }
}
