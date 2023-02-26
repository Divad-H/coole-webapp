using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CooleWebapp.Database.Configuration;

public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
  public void Configure(EntityTypeBuilder<UserSettings> builder)
  {
    builder.Property(x => x.CoolUserId).IsRequired();
  }
}
