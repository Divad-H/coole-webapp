using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Accounting.Services
{
  public record UserBalanceResponseModel
  {
    [Required] public string UserName { get; init; } = string.Empty;
    [Required] public decimal Balance { get; init; }
  }

  public interface IUserAccount
  {
    Task<UserBalanceResponseModel> GetUserBalance(string webappUserId, CancellationToken ct);
  }
}
