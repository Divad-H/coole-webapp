using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public class Deposit
{
  public UInt64 Id { get; set; }
  public UInt64 CoolUserId { get; set; }
  [Required] public DateTime Timestamp { get; set; }
  public decimal Amount { get; set; }
  public UInt64? MonthlyClosingId { get; set; }

  public CoolUser? CoolUser { get; set; }
  public MonthlyClosing? MonthlyClosing { get; set; }
}
