using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Auth.Registration;

public record RegistrationData(
  [Required]
  string Name,
  [Required]
  string Initials,
  [Required]
  string Password,
  [Required]
  string Email,
  string? Title = null);

public record InitiatePasswordResetData(
  [Required]
  string Email
);

public interface IUserRegistration
{
  Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct);
  Task ConfirmEmailAsync(string email, string token, CancellationToken ct);
  Task InitiatePasswordReset(
    StartInitiatePasswordResetDto initiatePasswordResetDto,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct);
}
