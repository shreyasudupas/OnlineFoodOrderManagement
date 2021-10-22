using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CityNames).HasMaxLength(200);
            builder.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            //builder.HasData(
            //    new City { CityNames = "Bangalore" , StateId = 1 , CreatedDate = System.DateTime.Now },
            //    new City { CityNames = "Mumbai", StateId = 2, CreatedDate = System.DateTime.Now }
            //);
        }
    }
}
