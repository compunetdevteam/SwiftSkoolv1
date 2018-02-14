namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermToFeeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeeTypes", "TermName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeeTypes", "TermName");
        }
    }
}
