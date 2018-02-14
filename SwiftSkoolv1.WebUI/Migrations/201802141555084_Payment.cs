namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RemitaPaymentLogs",
                c => new
                    {
                        RemitaPaymentLogId = c.Int(nullable: false, identity: true),
                        SchoolId = c.String(),
                        OrderId = c.String(),
                        Amount = c.String(),
                        Rrr = c.String(),
                        StatusCode = c.String(),
                        TransactionMessage = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentName = c.String(),
                        FeeCategory = c.String(),
                    })
                .PrimaryKey(t => t.RemitaPaymentLogId);
            
            AddColumn("dbo.Students", "StudentStatus", c => c.String());
            AddColumn("dbo.FeePayments", "FeeCategory", c => c.String());
            AddColumn("dbo.RemitaFeeSettings", "FeeCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RemitaFeeSettings", "FeeCategory");
            DropColumn("dbo.FeePayments", "FeeCategory");
            DropColumn("dbo.Students", "StudentStatus");
            DropTable("dbo.RemitaPaymentLogs");
        }
    }
}
