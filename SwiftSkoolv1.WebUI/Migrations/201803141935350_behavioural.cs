namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class behavioural : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AssignBehaviors", "SkillScore", c => c.String(maxLength: 135));
            AlterColumn("dbo.AssignBehaviors", "TeacherComment", c => c.String(maxLength: 700));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AssignBehaviors", "TeacherComment", c => c.String(maxLength: 50));
            AlterColumn("dbo.AssignBehaviors", "SkillScore", c => c.String(maxLength: 35));
        }
    }
}
