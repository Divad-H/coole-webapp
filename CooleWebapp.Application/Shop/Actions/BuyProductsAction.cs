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
    private readonly IProductDataAccess _productDataAccess;
    private readonly IAccountingDataAccess _accountingDataAccess;
    private readonly IUserDataAccess _userDataAccess;
    public BuyProductsAction(
      IProductDataAccess productDataAccess,
      IAccountingDataAccess balanceDataAccess,
      IUserDataAccess userDataAccess)
    {
      _productDataAccess = productDataAccess;
      _accountingDataAccess = balanceDataAccess;
      _userDataAccess = userDataAccess;
    }
    public async Task<Unit> Run(BuyProductsDto dataIn, CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(dataIn.WebappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      var balance = await _accountingDataAccess.GetBalance(user.Id, ct);
      balance.Version = Guid.NewGuid();

      if (!dataIn.Products.Any())
        throw new ClientError(ErrorType.InvalidOperation, "At least one product must be bought");
      List<OrderItem> orderItems = new();

      decimal totalPrice = 0;
      foreach(var productAmount in dataIn.Products)
      {
        if (productAmount.Amount < 1)
          throw new ClientError(ErrorType.InvalidOperation, "Amount must not be zero.");
        if (productAmount.Amount > 100)
          throw new ClientError(ErrorType.InvalidOperation, "Amount must not be larger than 100.");
        var product = await _productDataAccess.GetProduct(productAmount.ProductId, ct);
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

      await _accountingDataAccess.CreateOrder(new()
      {
        CoolUserId = user.Id,
        Timestamp = DateTime.Now,
        OrderItems = orderItems
      }, ct);

      return Unit.Default;
    }
  }
}
