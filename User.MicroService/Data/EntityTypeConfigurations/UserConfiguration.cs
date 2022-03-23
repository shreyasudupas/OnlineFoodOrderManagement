using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("User");
            builder.Property(e => e.UserName).IsRequired().HasMaxLength(500);
            builder.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PictureLocation).HasMaxLength(2000);
            builder.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            //builder.HasData(
            //    new User { 
            //        UserName = "admin@test.com" ,
            //        RoleId = 1 , 
            //        FullName = "admin" ,
            //        PictureLocation = "https://s.gravatar.com/avatar/5b37040e6200edb3c7f409e994076872?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fad.png",
            //        Points = 0,
            //        CartAmount = 0,
            //        CreatedDate = System.DateTime.Now
            //    }
            //    );
        }
    }
}
