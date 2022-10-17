using CooleWebapp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CooleWebapp.Database.Configuration;

public class CoolUserConfiguration : IEntityTypeConfiguration<CoolUser>
{
  public void Configure(EntityTypeBuilder<CoolUser> builder)
  {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.WebappUserId).IsRequired();
    builder.Property(x => x.FirstName).IsRequired();
    builder.Property(x => x.LastName).IsRequired();
    builder.Property(x => x.Initials).IsRequired();
  }
}
