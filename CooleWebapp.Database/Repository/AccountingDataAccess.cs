using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Core.Entities;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Repository
{
  internal class AccountingDataAccess : IAccountingDataAccess
  {
    private readonly WebappDbContext _dbContext;
    public AccountingDataAccess(WebappDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<UInt64> CreateDeposit(Deposit deposit, CancellationToken ct)
    {
      var res = await _dbContext.Deposits.AddAsync(deposit, ct);
      return res.Entity.Id;
    }

    public async Task<UInt64> CreateOrder(Order order, CancellationToken ct)
    {
      var res = await _dbContext.Orders.AddAsync(order, ct);
      return res.Entity.Id;
    }

    public async Task<Balance> GetBalance(UInt64 coolUserId, CancellationToken ct)
    {
      var res = await _dbContext.Balance
        .SingleOrDefaultAsync(b => b.CoolUserId == coolUserId);
      if (res is null)
      {
        res = new() { CoolUserId = coolUserId, Value = 0 };
        _dbContext.Attach(res);
      }
      return res;
    }

    public Task<IQueryable<Order>> GetAllOrders(CancellationToken ct)
    {
      return Task.FromResult<IQueryable<Order>>(_dbContext.Orders);
    }

    public Task<IQueryable<OrderItem>> GetAllOrderItems(CancellationToken ct)
    {
      return Task.FromResult<IQueryable<OrderItem>>(_dbContext.OrderItems);
    }
  }
}
