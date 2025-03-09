namespace CooleWebapp.Application.Accounting.Services
{
  public record AddBalanceDto(
    Int64 CoolUserId,
    decimal Amount);
}
