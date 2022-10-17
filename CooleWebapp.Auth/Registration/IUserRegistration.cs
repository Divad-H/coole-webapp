using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Auth.Registration;

public record RegistrationData(
  [Required]
  string FirstName,
  [Required]
  string LastName,
  [Required]
  string Password,
  [Required]
  string Email,
  string Title);

public interface IUserRegistration
{
  Task RegisterUser(RegistrationData registrationData, CancellationToken ct);
}
