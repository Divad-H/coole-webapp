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

public interface IUserRegistration
{
  Task RegisterUser(RegistrationData registrationData, CancellationToken ct);
}
