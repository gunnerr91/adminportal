namespace AdminPortal.Migrations.EmployeeDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewEmployeeViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Department = c.Int(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        ContractEndDate = c.DateTime(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        CurrentSalary = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.YearlyWageExpenditureViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        EmployeeName = c.String(),
                        BusinessYear = c.DateTime(nullable: false),
                        CurrentSalaryStartDate = c.DateTime(nullable: false),
                        CurrentSalaryEndDate = c.DateTime(nullable: false),
                        OtherSalaryStartDate = c.DateTime(),
                        OtherSalaryEndDate = c.DateTime(),
                        OtherSalary = c.Int(nullable: false),
                        LoyaltyBonus = c.Int(nullable: false),
                        SalesCommissionBonus = c.Int(nullable: false),
                        HolidayBonus = c.Int(nullable: false),
                        MissionBonus = c.Int(nullable: false),
                        ReferalBonus = c.Int(nullable: false),
                        OtherBonus = c.Int(nullable: false),
                        YearTotal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.YearlyWageExpenditureViewModels");
            DropTable("dbo.NewEmployeeViewModels");
        }
    }
}
