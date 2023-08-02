using Microsoft.EntityFrameworkCore.Design;
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

public enum PurchaseStatisticsTimePeriod
{
  OneYear,
  Total,
}

public record GetPurchasesPerTimeStatisticsRequestModel
{

  public UInt64? ProductIdFilter { get; init; }
  public PurchaseStatisticsTimePeriod PurchaseStatisticsTimePeriod { get; init; }
    = PurchaseStatisticsTimePeriod.OneYear;
}

public record GetPurchasesPerTimeStatisticsResponseModel
{
  /// <summary>
  /// Starting month - Januaray is 1
  /// </summary>
  [Required] public required UInt32 StartMonth { get; init; }
  [Required] public required UInt32 StartYear { get; init; }
  [Required] public required IReadOnlyCollection<UInt32> NumberOfPurchases { get; init; }
}

public record GetProductStatisticsRequestModel
{
  public TimePeriod TimePeriod { get; init; } = TimePeriod.Total;
}

public record GetProductStatisticsResponseModel
{
  [Required] public required int NumberOfPurchases { get; init; }
  [Required] public required UInt64 ProductId { get; init; }
  [Required] public required string ProductName { get; init; }
}

public interface IStatisticsService
{
  Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct);

  Task<IReadOnlyCollection<GetTopSpendersResponseModel>> GetTopSpenders(
    GetTopSpendersRequestModel getTopSpendersRequest,
    CancellationToken ct);

  Task<GetPurchasesPerTimeStatisticsResponseModel> GetPurchasesPerTimeStatistics(
    GetPurchasesPerTimeStatisticsRequestModel getPurchasesPerTimeStatisticsRequest,
    CancellationToken ct);

  Task<IReadOnlyCollection<GetProductStatisticsResponseModel>> GetProductStatistics(
    GetProductStatisticsRequestModel getProductStatisticsRequest,
    CancellationToken ct);
}
