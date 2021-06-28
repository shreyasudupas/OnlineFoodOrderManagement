using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderAPI.BuisnessLayer.DBModels;

#nullable disable

namespace MenuDatabase.Data.Database
{
    public partial class MenuOrderManagementContext : DbContext
    {
        public MenuOrderManagementContext()
        {
        }

        public MenuOrderManagementContext(DbContextOptions<MenuOrderManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblMenu> TblMenus { get; set; }
        public virtual DbSet<TblMenuType> TblMenuTypes { get; set; }
        public virtual DbSet<TblVendorList> TblVendorLists { get; set; }

        public virtual DbSet<tblUser> tblUsers { get; set; }

        public virtual DbSet<tblRole> tblRoles { get; set; }

        public virtual DbSet<tblLog> tblLogs { get; set; }

        public virtual DbSet<tblPaymentType> tblPaymentTypes { get; set; }
        public virtual DbSet<tblUserOrder> tblUserOrders { get; set; }

        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblState> tblStates { get; set; }
        public virtual DbSet<tblPaymentMode> tblPaymentModes { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=DESKTOP-MV64S7M\\SQLEXPRESS;Database=MenuOrderManagement;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.HasKey(e => e.MenuId)
                    .HasName("PK__tblMenu__C99ED2307583CC6C");

                entity.ToTable("tblMenu");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagePath).HasMaxLength(1000);

                entity.Property(e => e.MenuItem)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.MenuType)
                    .WithMany(p => p.TblMenus)
                    .HasForeignKey(d => d.MenuTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMenuType_MenuTypeId");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.TblMenus)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMenu_VendorId");
            });

            modelBuilder.Entity<TblMenuType>(entity =>
            {
                entity.HasKey(e => e.MenuTypeId)
                    .HasName("PK__tblMenuT__8E7B2D6AAD34CA44");

                entity.ToTable("tblMenuType");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MenuTypeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImagePath).HasMaxLength(500);
            });

            modelBuilder.Entity<TblVendorList>(entity =>
            {
                entity.HasKey(e => e.VendorId)
                    .HasName("PK__tblVendo__FC8618F3EE5C5634");

                entity.ToTable("tblVendorList");

                entity.Property(e => e.VendorDescription).HasMaxLength(500);

                entity.Property(e => e.VendorImgLink).HasMaxLength(1000);

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblUser>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("tblUser");
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PictureLocation).HasMaxLength(2000);
                entity.HasOne<tblRole>(e => e.tblRole).WithMany(d => d.tblUsers).HasForeignKey(e => e.RoleId);
                entity.HasOne(e => e.City).WithMany(d => d.Users).HasForeignKey(e => e.CityId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.State).WithMany(d => d.Users).HasForeignKey(e => e.StateId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblRole>(entity => 
            {
                entity.HasKey(e => e.RoleId);
                entity.ToTable("tblRole");
                entity.Property(e => e.Rolename).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<tblLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.ToTable("tblLog");
                entity.Property(e => e.ControllerName).IsRequired();
                entity.Property(e => e.ErrorMessage).IsRequired();
            });

            modelBuilder.Entity<tblPaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId);
                entity.ToTable("tblPaymentType");
                entity.Property(e => e.PaymentType).HasMaxLength(100);
                entity.Property(e => e.PaymentDescription).HasMaxLength(200);
            });

            modelBuilder.Entity<tblUserOrder>(entity =>
            {
                entity.HasKey(e => e.UserOrderId);
                entity.ToTable("tblUserOrder");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
                entity.HasOne(e => e.User).WithMany(d => d.tblUserOrders).HasForeignKey(e=>e.UserId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.VendorList).WithMany(d => d.UserOrders).HasForeignKey(e => e.VendorId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(e => e.Menu).WithMany(d => d.UserOrders).HasForeignKey(e => e.MenuId).OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<tblState>(entity =>
            {
                entity.HasKey(e => e.StateId);
                entity.ToTable("tblState");
                entity.Property(e => e.StateNames).HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblCity>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.ToTable("tblCity");
                entity.Property(e => e.CityNames).HasMaxLength(200);
                entity.HasOne(e => e.State).WithMany(d => d.Cities).HasForeignKey(e => e.StateId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tblPaymentMode>(entity =>
            {
                entity.HasKey(e => e.PaymentModeId);
                entity.ToTable("tblPaymentMode");
                entity.Property(e=>e.PaymenentType).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
