namespace CooleWebapp.Application.Shop.Services
{
  public record BuyProductsDto
  {
    public required string WebappUserId { get; init; }
    public required IEnumerable<ProductAmount> Products { get; init; }
  }
}
