using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddress");
            builder.HasKey(e => e.Id);

            //builder.HasData(
            //    new UserAddress { CityId = 1 , StateId = 1 , UserId = 1 , Address= "Citi Center Building, Balmatta" }
            //);
        }
    }
}
