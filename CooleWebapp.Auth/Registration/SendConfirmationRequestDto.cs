namespace CooleWebapp.Auth.Registration
{
  internal record SendConfirmationRequestDto(
    string ConfirmationLink,
    string Name,
    string Email);
}
