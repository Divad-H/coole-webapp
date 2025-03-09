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
  [Required] public required Int64 CoolUserId { get; init; }
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

  public Int64? ProductIdFilter { get; init; }
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
  [Required] public required Int64 ProductId { get; init; }
  [Required] public required string ProductName { get; init; }
}

public record GetMostRecentPurchasesRequestModel
{
  public int MaxNumberOfPurchases { get; init; } = 5;
}

public record GetMostRecentPurchasesResponseModel
{
  [Required] public required string BuyerInitials { get; init; }
  [Required] public required string BuyerName { get; init; }
  [Required] public required Int64 BuyerCoolUserId { get; init; }
  [Required] public required UInt32 Quantity { get; init; }
  [Required] public required string ProductName { get; init; }
  [Required] public required Int64 ProductId { get; init; }
  [Required] public required decimal Price { get; init; }
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

  Task<IReadOnlyCollection<GetMostRecentPurchasesResponseModel>> GetMostRecentPurchases(
    GetMostRecentPurchasesRequestModel getMostRecentPurchasesRequest,
    CancellationToken ct);
}
