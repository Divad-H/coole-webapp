using CooleWebapp.Auth.Model;
using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CooleWebapp.Database.Configuration;

public class WebappUserConfiguration : IEntityTypeConfiguration<WebappUser>
{
  public void Configure(EntityTypeBuilder<WebappUser> builder)
  {
    builder.HasOne<CoolUser>().WithOne().HasForeignKey<CoolUser>(x => x.WebappUserId);
  }
}
