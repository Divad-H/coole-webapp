namespace CooleWebapp.Auth.Registration;

public record SendConfirmationRequestDto(
  string ConfirmationLink,
  string Name,
  string Email);
