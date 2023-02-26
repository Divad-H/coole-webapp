using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Dashboard.Services;

public record BuyerResponseModel
{
  public BuyerResponseModel(
    UInt64 coolUserId,
    string name,
    string initials,
    decimal balance,
    bool canBuyOnFridge)
  {
    CoolUserId = coolUserId;
    Name = name;
    Initials = initials;
    Balance = balance;
    CanBuyOnFridge = canBuyOnFridge;
  }

  [Required] public UInt64 CoolUserId { get; }
  [Required] public string Name { get; }
  [Required] public string Initials { get; }
  [Required] public decimal Balance { get; }
  [Required] public bool CanBuyOnFridge { get; }
}

public record GetRecentBuyersResponeModel
{
  public GetRecentBuyersResponeModel(Paginated<BuyerResponseModel> buyers)
  {
    Buyers = buyers;
  }
  [Required] public Paginated<BuyerResponseModel> Buyers { get; }
}

public interface IDashboardService
{
  Task<GetRecentBuyersResponeModel> ReadRecentBuyers(
    UInt32 pageIndex,
    UInt32 pageSize, 
    CancellationToken ct);
}
