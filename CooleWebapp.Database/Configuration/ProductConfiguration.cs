using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder
      .Property(p => p.Price)
      .HasPrecision(18, 2);
  }
}
