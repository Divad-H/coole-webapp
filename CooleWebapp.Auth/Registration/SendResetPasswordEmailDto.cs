namespace CooleWebapp.Auth.Registration
{
  public record SendResetPasswordEmailDto(
    string Link,
    string Email);
}
