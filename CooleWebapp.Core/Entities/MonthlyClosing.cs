using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public record MonthlyClosing
{
  public Int64 Id { get; set; }
  public decimal Balance { get; set; }
  public Int64 CoolUserId { get; set; }
  [Required] public DateTime Timestamp { get; set; }
  public Int64 Number { get; set; }

  public CoolUser? CoolUser { get; set; }
}
