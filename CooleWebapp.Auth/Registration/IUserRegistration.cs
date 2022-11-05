using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Auth.Registration;

public record RegistrationData 
{
  [Required] public string Name { get; init; } = string.Empty;
  [Required] public string Initials { get; init; } = string.Empty;
  [Required] public string Password { get; init; } = string.Empty;
  [Required] public string Email { get; init; } = string.Empty;
  public string? Title { get; set; } = null;
}

public record InitiatePasswordResetData 
{
  [Required] public string Email { get; init; } = string.Empty;
}

public record ResetPasswordData 
{
  [Required] public string Email { get; init; } = string.Empty;
  [Required] public string Token { get; init; } = string.Empty;
  [Required] public string Password { get; init; }  = string.Empty;
}

public interface IUserRegistration
{
  Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct);
  Task ConfirmEmailAsync(string email, string token, CancellationToken ct);
  Task InitiatePasswordReset(
    InitiatePasswordResetData initiatePasswordResetData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct);
  Task ResetPassword(ResetPasswordData resetPasswordData, CancellationToken ct);
}
