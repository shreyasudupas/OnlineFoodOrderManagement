using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Rolename).HasMaxLength(200).IsRequired();

            //builder.HasData(
            //    new UserRole { Rolename = "user" },
            //    new UserRole { Rolename = "admin" }
            //);
        }
    }
}
