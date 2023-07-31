using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Accounting.Repository
{
  public interface IAccountingDataAccess
  {
    Task<Balance> GetBalance(UInt64 coolUserId, CancellationToken ct);

    Task<UInt64> CreateOrder(Order order, CancellationToken ct);
    Task<UInt64> CreateDeposít(Deposit deposit, CancellationToken ct);
    Task<IQueryable<Order>> GetAllOrders(CancellationToken ct);
    Task<IQueryable<OrderItem>> GetAllOrderItems(CancellationToken ct);
  }
}
