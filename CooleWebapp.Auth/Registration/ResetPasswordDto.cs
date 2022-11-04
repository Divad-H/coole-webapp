namespace CooleWebapp.Auth.Registration;

public record ResetPasswordDto(
  string Email,
  string Token,
  string Password);
