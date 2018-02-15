namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToJambExamRule : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JambExamRules", "ResultDivision", c => c.Int());
            AlterColumn("dbo.JambExamRules", "MaximumTime", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JambExamRules", "MaximumTime", c => c.Int(nullable: false));
            AlterColumn("dbo.JambExamRules", "ResultDivision", c => c.Int(nullable: false));
        }
    }
}
