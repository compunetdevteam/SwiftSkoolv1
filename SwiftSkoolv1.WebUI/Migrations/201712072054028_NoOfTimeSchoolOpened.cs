namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoOfTimeSchoolOpened : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportCardSettings", "NoOfTimesSchoolOpened", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportCardSettings", "NoOfTimesSchoolOpened");
        }
    }
}
