namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrincipalComment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrincipalComments", "Remark", c => c.String(nullable: false, maxLength: 135));
            AlterColumn("dbo.PrincipalComments", "ClassName", c => c.String(maxLength: 65));
            AlterColumn("dbo.TeacherComments", "StudentId", c => c.String(nullable: false, maxLength: 65));
            AlterColumn("dbo.TeacherComments", "Remark", c => c.String(nullable: false, maxLength: 135));
            AlterColumn("dbo.TeacherComments", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeacherComments", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TeacherComments", "Remark", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.TeacherComments", "StudentId", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.PrincipalComments", "ClassName", c => c.String(maxLength: 15));
            AlterColumn("dbo.PrincipalComments", "Remark", c => c.String(nullable: false, maxLength: 35));
        }
    }
}
