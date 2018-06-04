namespace FoxAndHounds.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        MondayId = c.Int(),
                        TuesdayId = c.Int(),
                        WednesdayId = c.Int(),
                        ThursdayId = c.Int(),
                        FridayId = c.Int(),
                        SaturdayId = c.Int(),
                        SundayId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.AvailabilityDays", t => t.FridayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.MondayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.SaturdayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.SundayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.ThursdayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.TuesdayId)
                .ForeignKey("dbo.AvailabilityDays", t => t.WednesdayId)
                .Index(t => t.EmployeeId)
                .Index(t => t.MondayId)
                .Index(t => t.TuesdayId)
                .Index(t => t.WednesdayId)
                .Index(t => t.ThursdayId)
                .Index(t => t.FridayId)
                .Index(t => t.SaturdayId)
                .Index(t => t.SundayId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AvailabilityDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AbleToWork = c.Boolean(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bussers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.BusserScheduleDayJunctions",
                c => new
                    {
                        BusserId = c.Int(nullable: false),
                        ScheduleDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusserId, t.ScheduleDayId })
                .ForeignKey("dbo.Shifts", t => t.BusserId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleDays", t => t.ScheduleDayId, cascadeDelete: true)
                .Index(t => t.BusserId)
                .Index(t => t.ScheduleDayId);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ScheduleDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DayOffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Expoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ExpoScheduleDayJunctions",
                c => new
                    {
                        ExpoId = c.Int(nullable: false),
                        ScheduleDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ExpoId, t.ScheduleDayId })
                .ForeignKey("dbo.Shifts", t => t.ExpoId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleDays", t => t.ScheduleDayId, cascadeDelete: true)
                .Index(t => t.ExpoId)
                .Index(t => t.ScheduleDayId);
            
            CreateTable(
                "dbo.FoodRunners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.FoodRunnerScheduleDayJunctions",
                c => new
                    {
                        FoodRunnerId = c.Int(nullable: false),
                        ScheduleDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FoodRunnerId, t.ScheduleDayId })
                .ForeignKey("dbo.Shifts", t => t.FoodRunnerId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleDays", t => t.ScheduleDayId, cascadeDelete: true)
                .Index(t => t.FoodRunnerId)
                .Index(t => t.ScheduleDayId);
            
            CreateTable(
                "dbo.Hosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.HostScheduleDayJunctions",
                c => new
                    {
                        HostId = c.Int(nullable: false),
                        ScheduleDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HostId, t.ScheduleDayId })
                .ForeignKey("dbo.Shifts", t => t.HostId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleDays", t => t.ScheduleDayId, cascadeDelete: true)
                .Index(t => t.HostId)
                .Index(t => t.ScheduleDayId);
            
            CreateTable(
                "dbo.RequestOffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShiftId = c.Int(),
                        EmployeeId = c.Int(),
                        EmployeeAccepted = c.Boolean(nullable: false),
                        ManagerAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Shifts", t => t.ShiftId)
                .Index(t => t.ShiftId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MondayId = c.Int(),
                        TuesdayId = c.Int(),
                        WednesdayId = c.Int(),
                        ThursdayId = c.Int(),
                        FridayId = c.Int(),
                        SaturdayId = c.Int(),
                        SundayId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleDays", t => t.FridayId)
                .ForeignKey("dbo.ScheduleDays", t => t.MondayId)
                .ForeignKey("dbo.ScheduleDays", t => t.SaturdayId)
                .ForeignKey("dbo.ScheduleDays", t => t.SundayId)
                .ForeignKey("dbo.ScheduleDays", t => t.ThursdayId)
                .ForeignKey("dbo.ScheduleDays", t => t.TuesdayId)
                .ForeignKey("dbo.ScheduleDays", t => t.WednesdayId)
                .Index(t => t.MondayId)
                .Index(t => t.TuesdayId)
                .Index(t => t.WednesdayId)
                .Index(t => t.ThursdayId)
                .Index(t => t.FridayId)
                .Index(t => t.SaturdayId)
                .Index(t => t.SundayId);
            
            CreateTable(
                "dbo.WorkerScheduleDayJunctions",
                c => new
                    {
                        WorkerId = c.Int(nullable: false),
                        ScheduleDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkerId, t.ScheduleDayId })
                .ForeignKey("dbo.ScheduleDays", t => t.ScheduleDayId, cascadeDelete: true)
                .ForeignKey("dbo.Shifts", t => t.WorkerId, cascadeDelete: true)
                .Index(t => t.WorkerId)
                .Index(t => t.ScheduleDayId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkerScheduleDayJunctions", "WorkerId", "dbo.Shifts");
            DropForeignKey("dbo.WorkerScheduleDayJunctions", "ScheduleDayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "WednesdayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "TuesdayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "ThursdayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "SundayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "SaturdayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "MondayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.Schedules", "FridayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RequestOffs", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.RequestOffs", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.HostScheduleDayJunctions", "ScheduleDayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.HostScheduleDayJunctions", "HostId", "dbo.Shifts");
            DropForeignKey("dbo.Hosts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.FoodRunnerScheduleDayJunctions", "ScheduleDayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.FoodRunnerScheduleDayJunctions", "FoodRunnerId", "dbo.Shifts");
            DropForeignKey("dbo.FoodRunners", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ExpoScheduleDayJunctions", "ScheduleDayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.ExpoScheduleDayJunctions", "ExpoId", "dbo.Shifts");
            DropForeignKey("dbo.Expoes", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.DayOffs", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.BusserScheduleDayJunctions", "ScheduleDayId", "dbo.ScheduleDays");
            DropForeignKey("dbo.BusserScheduleDayJunctions", "BusserId", "dbo.Shifts");
            DropForeignKey("dbo.Shifts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Bussers", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Availabilities", "WednesdayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "TuesdayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "ThursdayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "SundayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "SaturdayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "MondayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "FridayId", "dbo.AvailabilityDays");
            DropForeignKey("dbo.Availabilities", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.WorkerScheduleDayJunctions", new[] { "ScheduleDayId" });
            DropIndex("dbo.WorkerScheduleDayJunctions", new[] { "WorkerId" });
            DropIndex("dbo.Schedules", new[] { "SundayId" });
            DropIndex("dbo.Schedules", new[] { "SaturdayId" });
            DropIndex("dbo.Schedules", new[] { "FridayId" });
            DropIndex("dbo.Schedules", new[] { "ThursdayId" });
            DropIndex("dbo.Schedules", new[] { "WednesdayId" });
            DropIndex("dbo.Schedules", new[] { "TuesdayId" });
            DropIndex("dbo.Schedules", new[] { "MondayId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RequestOffs", new[] { "EmployeeId" });
            DropIndex("dbo.RequestOffs", new[] { "ShiftId" });
            DropIndex("dbo.HostScheduleDayJunctions", new[] { "ScheduleDayId" });
            DropIndex("dbo.HostScheduleDayJunctions", new[] { "HostId" });
            DropIndex("dbo.Hosts", new[] { "EmployeeId" });
            DropIndex("dbo.FoodRunnerScheduleDayJunctions", new[] { "ScheduleDayId" });
            DropIndex("dbo.FoodRunnerScheduleDayJunctions", new[] { "FoodRunnerId" });
            DropIndex("dbo.FoodRunners", new[] { "EmployeeId" });
            DropIndex("dbo.ExpoScheduleDayJunctions", new[] { "ScheduleDayId" });
            DropIndex("dbo.ExpoScheduleDayJunctions", new[] { "ExpoId" });
            DropIndex("dbo.Expoes", new[] { "EmployeeId" });
            DropIndex("dbo.DayOffs", new[] { "EmployeeId" });
            DropIndex("dbo.Shifts", new[] { "EmployeeId" });
            DropIndex("dbo.BusserScheduleDayJunctions", new[] { "ScheduleDayId" });
            DropIndex("dbo.BusserScheduleDayJunctions", new[] { "BusserId" });
            DropIndex("dbo.Bussers", new[] { "EmployeeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Employees", new[] { "UserId" });
            DropIndex("dbo.Availabilities", new[] { "SundayId" });
            DropIndex("dbo.Availabilities", new[] { "SaturdayId" });
            DropIndex("dbo.Availabilities", new[] { "FridayId" });
            DropIndex("dbo.Availabilities", new[] { "ThursdayId" });
            DropIndex("dbo.Availabilities", new[] { "WednesdayId" });
            DropIndex("dbo.Availabilities", new[] { "TuesdayId" });
            DropIndex("dbo.Availabilities", new[] { "MondayId" });
            DropIndex("dbo.Availabilities", new[] { "EmployeeId" });
            DropTable("dbo.WorkerScheduleDayJunctions");
            DropTable("dbo.Schedules");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RequestOffs");
            DropTable("dbo.HostScheduleDayJunctions");
            DropTable("dbo.Hosts");
            DropTable("dbo.FoodRunnerScheduleDayJunctions");
            DropTable("dbo.FoodRunners");
            DropTable("dbo.ExpoScheduleDayJunctions");
            DropTable("dbo.Expoes");
            DropTable("dbo.DayOffs");
            DropTable("dbo.ScheduleDays");
            DropTable("dbo.Shifts");
            DropTable("dbo.BusserScheduleDayJunctions");
            DropTable("dbo.Bussers");
            DropTable("dbo.AvailabilityDays");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Employees");
            DropTable("dbo.Availabilities");
        }
    }
}
