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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActionMethod).HasMaxLength(200).IsRequired();
                entity.Property(e => e.ErrorMessage).IsRequired();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rolename).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StateNames).HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");
                entity.HasKey(e => e.Id);
                entity.ToTable("City");
                entity.Property(e => e.CityNames).HasMaxLength(200);
                entity.HasOne(e => e.State).WithMany(d => d.Cities).HasForeignKey(e => e.StateId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("User");
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PictureLocation).HasMaxLength(2000);
                entity.HasOne(e => e.Role).WithMany(d => d.Users).HasForeignKey(e => e.RoleId);
                entity.HasOne(e => e.City).WithMany(d => d.Users).HasForeignKey(e => e.CityId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.State).WithMany(d => d.Users).HasForeignKey(e => e.StateId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentDropDown>(entity =>
            {
                entity.ToTable("PaymentDropDown");
                entity.HasKey(e => e.Id);
                entity.HasData(
                new PaymentDropDown { Id = 1,Label = "Credit Card", Value = "Credit Card" },
                new PaymentDropDown { Id = 2,Label = "UPI", Value = "UPI" },
                new PaymentDropDown { Id = 3,Label = "Debit Card", Value = "Debit Card" },
                new PaymentDropDown { Id = 4,Label = "Wallet", Value = "Wallet" }); ;
            });
        }
    }
}
