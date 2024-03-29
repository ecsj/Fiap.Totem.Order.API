using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Domain.Entities.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.HasOne(a => a.Order);
    }
}