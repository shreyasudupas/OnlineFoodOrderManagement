using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Log");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ActionMethod).HasMaxLength(200).IsRequired();
            builder.Property(e => e.ErrorMessage).IsRequired();
        }
    }
}
