using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public record Balance
{
  public Int64 Id { get; set; }
  public Int64 CoolUserId { get; set; }
  public decimal Value { get; set; }
  public Guid Version { get; set; }

  public CoolUser? CoolUser { get; set; }
}
