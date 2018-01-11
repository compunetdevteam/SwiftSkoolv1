namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingGrading : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grades", "ClassName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grades", "ClassName");
        }
    }
}
