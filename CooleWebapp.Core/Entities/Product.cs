using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

public enum ProductState
{
  Available,
  SoldOut,
  Hidden,
}

public record ProductImage
{
  public UInt64 Id { get; set; }
  public UInt64 ProductId { get; set; }
  public byte[] Data { get; set; } = Array.Empty<byte>();
}

public record Product
{
  public UInt64 Id { get; set; }
  [MaxLength(256)][Required] public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public decimal Price { get; set; }
  public ProductState State { get; set; }
  public ProductImage? ProductImage { get; set; }
}
