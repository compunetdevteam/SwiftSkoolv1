namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class examTYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JambStudentQuestions", "ExamYear", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JambStudentQuestions", "ExamYear");
        }
    }
}
