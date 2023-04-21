using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CooleWebapp.Database.Configuration;

public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
  public void Configure(EntityTypeBuilder<Balance> builder)
  {
    builder
      .Property(p => p.Version)
      .IsConcurrencyToken();
    builder
      .Property(p => p.Value)
      .HasPrecision(18, 2);
  }
}
