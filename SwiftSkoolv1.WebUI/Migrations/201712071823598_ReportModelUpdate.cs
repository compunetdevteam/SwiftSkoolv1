namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportModelUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignReportCards",
                c => new
                    {
                        AssignReportCardId = c.Int(nullable: false, identity: true),
                        ReportCardType = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        ClassName = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.AssignReportCardId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.ClassId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ReportCardSettings",
                c => new
                    {
                        ReportCardSettingId = c.Int(nullable: false, identity: true),
                        ResumptionDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                        PrincipalSignature = c.Binary(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ReportCardSettingId)
                .Index(t => t.SchoolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignReportCards", "ClassId", "dbo.Classes");
            DropIndex("dbo.ReportCardSettings", new[] { "SchoolId" });
            DropIndex("dbo.AssignReportCards", new[] { "SchoolId" });
            DropIndex("dbo.AssignReportCards", new[] { "ClassId" });
            DropTable("dbo.ReportCardSettings");
            DropTable("dbo.AssignReportCards");
        }
    }
}
