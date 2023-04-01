namespace CooleWebapp.Application.Shop.Services
{
  public record BuyProductsDto
  {
    public required string WebappUserId { get; init; }
    public required IEnumerable<ProductAmount> Products { get; init; }
  }

  public record BuyProductsAsFridgeDto
  {
    public required UInt64 CoolUserId { get; init; }
    public required IEnumerable<ProductAmount> Products { get; init; }
    public string? PinCode { get; init; }
  }
}
