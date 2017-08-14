namespace AdminPortal.Migrations.EmployeeDbContext2
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEmployeeDbContext : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.YearlyWageExpenditureModels", "BusinessYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.YearlyWageExpenditureModels", "BusinessYear", c => c.DateTime(nullable: false));
        }
    }
}
