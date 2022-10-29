namespace CooleWebapp.Core.ErrorHandling;

public enum ErrorType
{
  InvalidOperation,
  Unauthorized,
  Forbidden,
  NotFound,
};

public class ClientError : Exception
{
  public ClientError(ErrorType type, string message) : base(message) => Type = type;

  public ErrorType Type { get; }
}
