using Identity.MicroService.Data.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data.DatabaseContext
{
    public class UserContext:DbContext
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {
        }

        public  DbSet<Log> Logs { get; set; }

        public  DbSet<User> Users { get; set; }

        public  DbSet<UserRole> UserRoles { get; set; }
        public  DbSet<City> Cities { get; set; }
        public  DbSet<State> States { get; set; }
        public DbSet<PaymentDropDown> PaymentDropDown { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new LogConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentDropDownConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressConfiguration());

        }
    }

}

