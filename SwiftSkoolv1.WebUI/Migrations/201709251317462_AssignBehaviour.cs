namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignBehaviour : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AssignBehaviors", "SkillScore", c => c.String(maxLength: 35));
            AlterColumn("dbo.AssignBehaviors", "StudentId", c => c.String(maxLength: 155));
            AlterColumn("dbo.AssignBehaviors", "TermName", c => c.String(maxLength: 55));
            AlterColumn("dbo.AssignBehaviors", "SessionName", c => c.String(maxLength: 55));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AssignBehaviors", "SessionName", c => c.String(maxLength: 15));
            AlterColumn("dbo.AssignBehaviors", "TermName", c => c.String(maxLength: 15));
            AlterColumn("dbo.AssignBehaviors", "StudentId", c => c.String(maxLength: 25));
            AlterColumn("dbo.AssignBehaviors", "SkillScore", c => c.String(maxLength: 15));
        }
    }
}
