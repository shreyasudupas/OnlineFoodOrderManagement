using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("State");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.StateNames).HasMaxLength(200);
            builder.Property(e => e.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            //builder.HasData(
            //    new State { StateNames = "Karnataka" , CreatedDate = System.DateTime.Now },
            //    new State { StateNames = "Maharastra", CreatedDate = System.DateTime.Now }
            //);
        }
    }
}
