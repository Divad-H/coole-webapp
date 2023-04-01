using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Application.Shop.Actions
{
  internal class BuyProductsAction : IBusinessAction<BuyProductsDto, Unit>
  {
    private readonly IUserDataAccess _userDataAccess;
    private readonly IProductDataAccess _productDataAccess;
    private readonly IAccountingDataAccess _accountingDataAccess;
    public BuyProductsAction(
      IUserDataAccess userDataAccess,
      IProductDataAccess productDataAccess,
      IAccountingDataAccess accountingDataAccess)
    {
      _userDataAccess = userDataAccess;
      _productDataAccess = productDataAccess;
      _accountingDataAccess = accountingDataAccess;
    }

    public async Task<Unit> Run(BuyProductsDto dataIn, CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(dataIn.WebappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      return await BuyProducts(
        dataIn.Products,
        user.Id,
        _productDataAccess,
        _accountingDataAccess,
        ct);
    }

    public static async Task<Unit> BuyProducts(
      IEnumerable<ProductAmount> products,
      UInt64 coolUserId,
      IProductDataAccess productDataAccess,
      IAccountingDataAccess accountingDataAccess,
      CancellationToken ct)
    {
      var balance = await accountingDataAccess.GetBalance(coolUserId, ct);
      balance.Version = Guid.NewGuid();

      if (!products.Any())
        throw new ClientError(ErrorType.InvalidOperation, "At least one product must be bought");
      List<OrderItem> orderItems = new();

      decimal totalPrice = 0;
      foreach (var productAmount in products)
      {
        if (productAmount.Amount < 1)
          throw new ClientError(ErrorType.InvalidOperation, "Amount must not be zero.");
        if (productAmount.Amount > 100)
          throw new ClientError(ErrorType.InvalidOperation, "Amount must not be larger than 100.");
        var product = await productDataAccess.GetProduct(productAmount.ProductId, ct);
        if (product is null)
          throw new ClientError(ErrorType.NotFound, $"A product with id {productAmount.ProductId} does not exist.");
        if (product.State != ProductState.Available)
          throw new ClientError(ErrorType.InvalidOperation, $"{product.Name} is not available.");
        var actualPrice = product.Price * productAmount.Amount;
        if (actualPrice != productAmount.ExpectedPrice)
          throw new ClientError(ErrorType.InvalidOperation, "The actual price differs from the displayed price.");
        totalPrice += actualPrice;
        orderItems.Add(new OrderItem()
        {
          Price = product.Price,
          Quantity = (UInt16)productAmount.Amount,
          ProductId = product.Id,
        });
      }

      if (balance.Value < totalPrice)
        throw new ClientError(ErrorType.InvalidOperation, "Insufficient funds.");
      balance.Value -= totalPrice;

      await accountingDataAccess.CreateOrder(new()
      {
        CoolUserId = coolUserId,
        Timestamp = DateTime.Now,
        OrderItems = orderItems
      }, ct);

      return Unit.Default;
    }
  }
}
