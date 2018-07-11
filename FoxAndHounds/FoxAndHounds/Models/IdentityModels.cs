using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoxAndHounds.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expo> Expos { get; set; }
        public DbSet<FoodRunner> FoodRunners { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Busser> Bussers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<AvailabilityDay> AvailabilityDays { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<WorkerScheduleDayJunction> WorkerScheduleDayJuction { get; set; }
        public DbSet<ExpoScheduleDayJunction> ExpoScheduleDayJunction { get; set; }
        public DbSet<FoodRunnerScheduleDayJunction> FoodRunnerScheduleDayJunction { get; set; }
        public DbSet<HostScheduleDayJunction> HostScheduleDayJunction { get; set; }
        public DbSet<BusserScheduleDayJunction> BusserScheduleDayJunction { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }
        public DbSet<RequestOff> RequestsOff { get; set; }
        public DbSet<SwitchShift> SwitchShifts { get; set; }


    }
}