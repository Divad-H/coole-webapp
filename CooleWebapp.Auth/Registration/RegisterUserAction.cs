using CooleWebapp.Application.EmailService;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Auth.Registration
{
  internal class RegisterUserAction : IBusinessAction<RegistrationData, Unit>
  {
    private readonly Func<(string Token, string Email), string> _createEmailLink;
    private readonly IUserManager _userManager;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IEmailSender _emailSender;
    public RegisterUserAction(
      Func<(string Token, string Email), string> createEmailLink,
      IUserManager userManager,
      IUserDataAccess userDataAccess,
      IEmailSender emailSender)
    {
      _createEmailLink = createEmailLink;
      _userManager = userManager;
      _userDataAccess = userDataAccess;
      _emailSender = emailSender;
    }

    public async Task<Unit> Run(RegistrationData registrationData, CancellationToken ct)
    {
      var user = await _userManager.FindByEmailAsync(registrationData.Email);
      if (user is not null)
        throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {registrationData.Email} already exists.");
      user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
      await _userManager.CreateAsync(user, registrationData.Password);
      user = await _userManager.FindByEmailAsync(registrationData.Email);
      await _userDataAccess.CreateUser(new()
      {
        Name = registrationData.Name,
        Initials = registrationData.Initials,
        Title = registrationData.Title,
        WebappUserId = user.Id
      }, ct);
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      var confirmationLink = _createEmailLink((Token: token, registrationData.Email));

      await _emailSender.SendEmailAsync(
        new Message(
          new (string Name, string Address)[]{
          (registrationData.Name, Address: registrationData.Email) },
          "Coole Webapp: Confirm E-Mail Address",
          $"Confirm your e-mail by following this link: {confirmationLink}"),
        ct);
      return Unit.Default;
    }
  }
}
