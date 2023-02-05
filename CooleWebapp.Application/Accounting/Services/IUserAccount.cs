using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Accounting.Services
{
  public record UserBalanceResponseModel
  {
    [Required] public string UserName { get; init; } = string.Empty;
    [Required] public decimal Balance { get; init; }
  }


  public record AddBalanceRequestModel
  {
    [Required] public decimal Amount { get; init; }
  }

  public interface IUserAccount
  {
    Task<UserBalanceResponseModel> GetUserBalance(string webappUserId, CancellationToken ct);
    Task<UserBalanceResponseModel> AddBalance(
      string webappUserId, 
      decimal amount,
      CancellationToken ct);
  }
}
