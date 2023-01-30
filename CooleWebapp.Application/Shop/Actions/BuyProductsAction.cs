using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Application.Users;
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
        ?? throw new ClientError(ErrorType.NotFound, "User not found");
      var balance = await _accountingDataAccess.GetBalance(user.Id, ct);
      balance.Version = Guid.NewGuid();

      if (!dataIn.Products.Any())
        throw new ClientError(ErrorType.InvalidOperation, "At least one product must be bought");
      List<OrderItem> items = new();
      decimal totalPrice = 0;
      foreach (var product in dataIn.Products)
      {
        if (product.Amount == 0)
          throw new ClientError(ErrorType.InvalidOperation, "Invalid amount (zero)");
        if (product.Amount > 100)
          throw new ClientError(ErrorType.InvalidOperation, "Maximum amount is 100");
        var dbProduct = (await _productDataAccess.GetProduct(product.ProductId, ct))
          ?? throw new ClientError(ErrorType.NotFound, $"The product with id {product.ProductId} was not found");
        if (dbProduct.State != ProductState.Available)
          throw new ClientError(ErrorType.InvalidOperation, $"{dbProduct.Name} is not availiable.");
        var actualPrice = dbProduct.Price * product.Amount;
        if (actualPrice != product.ExpectedPrice)
          throw new ClientError(ErrorType.InvalidOperation, "Actual price differs from displayed price.");
        totalPrice += actualPrice;
        items.Add(new OrderItem()
        {
          Quantity = (UInt16)product.Amount,
          Price = actualPrice,
          ProductId = product.ProductId,
        });
      }
      if (totalPrice > balance.Value)
        throw new ClientError(ErrorType.InvalidOperation, "Insufficient funds.");
      balance.Value -= totalPrice;
      Order order = new()
      {
        CoolUserId = user.Id,
        Timestamp = DateTime.Now,
        OrderItems = items
      };
      await _accountingDataAccess.CreateOrder(order, ct);

      return Unit.Default;
    }
  }
}
