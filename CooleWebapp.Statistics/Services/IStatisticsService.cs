using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Statistics.Services;

public record GetTotalPurchasesResponseModel
{
  [Required] public required decimal TotalPrice { get; init; }
  [Required] public required UInt32 TotalItems { get; init; }
}

public enum TimePeriod
{
  OneMonth,
  ThisMonth,
  OneYear,
  ThisYear,
  Total
}

public record GetTopSpendersResponseModel
{
  [Required] public required string Name { get; init; }
  [Required] public required string Initials { get; init; }
  [Required] public required UInt64 CoolUserId { get; init; }
  [Required] public decimal AmountSpent { get; init; }
}

public record GetTopSpendersRequestModel
{
  public UInt32 NumberOfSpenders { get; init; } = 5;
  public TimePeriod TimePeriod { get; init; } = TimePeriod.OneMonth;
}

public interface IStatisticsService
{
  Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct);

  Task<IReadOnlyCollection<GetTopSpendersResponseModel>> GetTopSpenders(
    GetTopSpendersRequestModel getTopSpendersRequest, 
    CancellationToken ct);
}
