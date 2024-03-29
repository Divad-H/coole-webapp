﻿using CooleWebapp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CooleWebapp.Database.Configuration;

public class MonthlyClosingConfiguration : IEntityTypeConfiguration<MonthlyClosing>
{
  public void Configure(EntityTypeBuilder<MonthlyClosing> builder)
  {
    builder.HasIndex(c => new { c.Number, c.CoolUserId }).IsUnique();
    builder.Property(p => p.Balance).HasPrecision(18, 2);
    builder.HasQueryFilter(mc => !mc.CoolUser!.IsDeleted);
  }
}
