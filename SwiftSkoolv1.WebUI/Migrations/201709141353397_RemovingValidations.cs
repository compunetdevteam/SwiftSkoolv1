namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guardians", "Email", c => c.String(maxLength: 85));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guardians", "Email", c => c.String(maxLength: 35));
        }
    }
}
