using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Email).HasMaxLength(250);
        builder.Property(p => p.CPF).HasMaxLength(11);
        builder.Property(p => p.CreatedAt);
    }
}