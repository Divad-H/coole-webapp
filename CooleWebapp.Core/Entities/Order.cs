using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public record Order
{
  public UInt64 Id { get; set; }
  public UInt64 CoolUserId { get; set; }
  public UInt64? MonthlyClosingId { get; set; }
  [Required] public DateTime Timestamp { get; set; }

  public ICollection<OrderItem>? OrderItems { get; set; }
  public CoolUser? CoolUser { get; set; }
  public MonthlyClosing? MonthlyClosing { get; set; }
}

public record OrderItem
{
  public UInt64 Id { get; set; }
  public UInt64 OrderId { get; set; }
  public UInt16 Quantity { get; set; }
  public UInt64 ProductId { get; set; }
  public decimal Price { get; set; }

  public Order? Order { get; set; }
}
