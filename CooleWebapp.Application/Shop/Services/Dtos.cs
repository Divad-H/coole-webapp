namespace CooleWebapp.Application.Shop.Services
{
  public record BuyProductsDto
  {
    public string WebappUserId { get; init; } = string.Empty;
    public IEnumerable<ProductAmount> Products { get; init; } = Array.Empty<ProductAmount>();
  }
}
