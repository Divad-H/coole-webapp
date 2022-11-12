using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public record MonthlyClosing
{
  public UInt64 Id { get; set; }
  public decimal Balance { get; set; }
  public UInt64 CoolUserId { get; set; }
  [Required] public DateTime Timestamp { get; set; }
  public UInt64 Number { get; set; }

  public CoolUser? CoolUser { get; set; }
}
