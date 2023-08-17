using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Configuration;

public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
  public void Configure(EntityTypeBuilder<Deposit> builder)
  {
    builder
      .Property(p => p.Amount)
      .HasPrecision(18, 2);
    builder.HasQueryFilter(d => !d.CoolUser!.IsDeleted);
  }
}
