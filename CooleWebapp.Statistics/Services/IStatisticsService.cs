using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Statistics.Services;

public record GetTotalPurchasesResponseModel
{
  [Required] public required decimal TotalPrice { get; init; }
  [Required] public required UInt32 TotalItems { get; init; }
}

public interface IStatisticsService
{
  Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct);
}
