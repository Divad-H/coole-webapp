using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Accounting.Repository
{
  public interface IAccountingDataAccess
  {
    Task<Balance> GetBalance(Int64 coolUserId, CancellationToken ct);

    Task<Int64> CreateOrder(Order order, CancellationToken ct);
    Task<Int64> CreateDeposit(Deposit deposit, CancellationToken ct);
    Task<IQueryable<Order>> GetAllOrders(CancellationToken ct);
    Task<IQueryable<OrderItem>> GetAllOrderItems(CancellationToken ct);
  }
}
