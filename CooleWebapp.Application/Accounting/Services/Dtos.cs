namespace CooleWebapp.Application.Accounting.Services
{
  public record AddBalanceDto(
    UInt64 CoolUserId,
    decimal Amount);
}
