namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuradianUpload : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guardians", "Salutation", c => c.String(maxLength: 45));
            AlterColumn("dbo.Guardians", "FirstName", c => c.String(maxLength: 85));
            AlterColumn("dbo.Guardians", "MiddleName", c => c.String(maxLength: 85));
            AlterColumn("dbo.Guardians", "LastName", c => c.String(maxLength: 85));
            AlterColumn("dbo.Guardians", "Gender", c => c.String(maxLength: 20));
            AlterColumn("dbo.Guardians", "Address", c => c.String(maxLength: 105));
            AlterColumn("dbo.Guardians", "Occupation", c => c.String(maxLength: 95));
            AlterColumn("dbo.Guardians", "Relationship", c => c.String(maxLength: 55));
            AlterColumn("dbo.Guardians", "Religion", c => c.String(maxLength: 55));
            AlterColumn("dbo.Guardians", "LGAOforigin", c => c.String(maxLength: 55));
            AlterColumn("dbo.Guardians", "StateOfOrigin", c => c.String(maxLength: 55));
            AlterColumn("dbo.Guardians", "MotherName", c => c.String(maxLength: 85));
            AlterColumn("dbo.Guardians", "MotherMaidenName", c => c.String(maxLength: 85));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guardians", "MotherMaidenName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "MotherName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "StateOfOrigin", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "LGAOforigin", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "Religion", c => c.String(maxLength: 15));
            AlterColumn("dbo.Guardians", "Relationship", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "Occupation", c => c.String(maxLength: 35));
            AlterColumn("dbo.Guardians", "Address", c => c.String(maxLength: 75));
            AlterColumn("dbo.Guardians", "Gender", c => c.String(maxLength: 10));
            AlterColumn("dbo.Guardians", "LastName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "MiddleName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "FirstName", c => c.String(maxLength: 25));
            AlterColumn("dbo.Guardians", "Salutation", c => c.String(maxLength: 15));
        }
    }
}
