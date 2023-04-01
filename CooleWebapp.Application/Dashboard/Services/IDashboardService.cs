using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Dashboard.Services;

public record BuyerResponseModel
{
  [Required] public required UInt64 CoolUserId { get; init; }
  [Required] public required string Name { get; init; }
  [Required] public required string Initials { get; init; }
  [Required] public required decimal Balance { get; init; }
  [Required] public required bool CanBuyOnFridge { get; init; }
}

public record GetRecentBuyersResponeModel
{
  [Required] public required Paginated<BuyerResponseModel> Buyers { get; init; }
}

public record GetBuyerResponseModel
{
  [Required] public required UInt64 CoolUserId { get; init; }
  [Required] public required string Name { get; init; }
  [Required] public required decimal Balance { get; init; }
  [Required] public required BuyOnFridgePermission BuyOnFridgePermission { get; init; }
}

public interface IDashboardService
{
  Task<GetRecentBuyersResponeModel> ReadRecentBuyers(
    UInt32 pageIndex,
    UInt32 pageSize, 
    CancellationToken ct);

  Task<GetBuyerResponseModel> ReadBuyerDetails(
    UInt64 CoolUserId,
    CancellationToken ct);
}
