using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.MicroService.Data.EntityTypeConfigurations
{
    public class PaymentDropDownConfiguration : IEntityTypeConfiguration<PaymentDropDown>
    {
        public void Configure(EntityTypeBuilder<PaymentDropDown> builder)
        {
            builder.ToTable("PaymentDropDown");
            builder.HasKey(e => e.Id);

            builder.HasData(
            new PaymentDropDown { Id = 1, Label = "Credit Card", Value = "Credit Card" },
            new PaymentDropDown { Id = 2, Label = "UPI", Value = "UPI" },
            new PaymentDropDown { Id = 3, Label = "Debit Card", Value = "Debit Card" },
            new PaymentDropDown { Id = 4, Label = "Wallet", Value = "Wallet" }
            );
        }
    }
}
