using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Accounting.Services
{
  public record UserBalanceResponseModel
  {
    [Required] public required string UserName { get; init; }
    [Required] public required decimal Balance { get; init; }
  }


  public record AddBalanceRequestModel
  {
    [Required] public required decimal Amount { get; init; }
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
