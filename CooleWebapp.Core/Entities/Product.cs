using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public enum ProductState
{
  Available,
  SoldOut,
  Removed,
}

public record Product
{
  public UInt64 Id { get; set; }
  [MaxLength(256)][Required] public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public decimal Price { get; set; }
  public byte[]? Image { get; set; }
  public ProductState State { get; set; }
}
