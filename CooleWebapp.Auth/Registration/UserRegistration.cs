using CooleWebapp.Application.EmailService;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration;

public sealed class UserRegistration : IUserRegistration
{
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  private readonly IEmailSender _emailSender;
  public UserRegistration(
    IUserManager userManager,
    IUserDataAccess userDataAccess,
    IEmailSender emailSender)
  {
    _userManager = userManager;
    _userDataAccess = userDataAccess;
    _emailSender = emailSender;
  }

  public async Task RegisterUser(
    RegistrationData registrationData,
    Func<(string Token, string Email), string> createEmailLink,
    CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(registrationData.Email);
    if (user is not null)
      throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {registrationData.Email} already exists.");
    user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
    await _userManager.CreateAsync(user, registrationData.Password);
    try
    {
      user = await _userManager.FindByEmailAsync(registrationData.Email);
      await _userDataAccess.CreateUser(new()
      {
        Name = registrationData.Name,
        Initials = registrationData.Initials,
        Title = registrationData.Title,
        WebappUserId = user.Id
      }, ct);
    }
    catch (Exception)
    {
      await _userManager.DeleteAsync(user);
      throw;
    }
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    var confirmationLink = createEmailLink((Token: token, registrationData.Email));

    await _emailSender.SendEmailAsync(
      new Message(
        new (string Name, string Address)[]{ 
          (registrationData.Name, Address: registrationData.Email) },
        "Coole Webapp: Confirm E-Mail Address",
        $"Confirm your e-mail by following this link: {confirmationLink}"), 
      ct);
  }

  public async Task ConfirmEmailAsync(string email, string token, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user is null)
      throw new ClientError(ErrorType.NotFound, "Invalid e-mail confirmation link. User not found.");

    await _userManager.ConfirmEmailAsync(user, token);
  }
}
