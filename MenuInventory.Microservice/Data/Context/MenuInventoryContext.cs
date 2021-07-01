using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data.Context
{
    public class MenuInventoryContext:DbContext
    {
        public MenuInventoryContext()
        {}

        public MenuInventoryContext(DbContextOptions<MenuInventoryContext> options):base(options)
        {
        }

        public virtual DbSet<VendorList> VendorLists { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<VendorList>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("VendorLists");
                entity.Property(e => e.VendorDescription).HasMaxLength(500);
                entity.Property(e => e.VendorImgLink).HasMaxLength(1000);
                entity.Property(e => e.VendorName).IsRequired().HasMaxLength(1000).IsUnicode(false);
            });
            modelBuilder.Entity<MenuType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("MenuTypes");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.MenuTypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
                entity.Property(e => e.ImagePath).HasMaxLength(500);
            });
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Menus");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ImagePath).HasMaxLength(1000);
                entity.Property(e => e.MenuItem).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
                entity.HasOne(d => d.MenuType).WithMany(p => p.Menus).HasForeignKey(d => d.MenuTypeId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.Vendor).WithMany(p => p.Menus).HasForeignKey(d => d.VendorId).OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
