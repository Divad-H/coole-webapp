using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public record Order
{
  public Int64 Id { get; set; }
  public Int64 CoolUserId { get; set; }
  public Int64? MonthlyClosingId { get; set; }
  [Required] public DateTime Timestamp { get; set; }

  public ICollection<OrderItem>? OrderItems { get; set; }
  public CoolUser? CoolUser { get; set; }
  public MonthlyClosing? MonthlyClosing { get; set; }
}

public record OrderItem
{
  public Int64 Id { get; set; }
  public Int64 OrderId { get; set; }
  public UInt16 Quantity { get; set; }
  public Int64 ProductId { get; set; }
  public decimal Price { get; set; }

  public Order? Order { get; set; }
}
