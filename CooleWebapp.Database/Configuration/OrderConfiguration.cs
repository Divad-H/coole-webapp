using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.HasQueryFilter(o => !o.CoolUser!.IsDeleted);
  }
}
