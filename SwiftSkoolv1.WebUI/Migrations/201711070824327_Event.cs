namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchoolEvents",
                c => new
                    {
                        SchoolEventId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IsHoliday = c.Boolean(nullable: false),
                        IsCommonToAll = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.SchoolEventId)
                .Index(t => t.SchoolId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SchoolEvents", new[] { "SchoolId" });
            DropTable("dbo.SchoolEvents");
        }
    }
}
