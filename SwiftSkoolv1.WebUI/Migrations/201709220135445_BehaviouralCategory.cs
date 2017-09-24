namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BehaviouralCategory : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BehaviorSkillCategories", new[] { "Name" });
            AlterColumn("dbo.BehaviorSkillCategories", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BehaviorSkillCategories", "Name", c => c.String(maxLength: 30));
            CreateIndex("dbo.BehaviorSkillCategories", "Name", unique: true);
        }
    }
}
