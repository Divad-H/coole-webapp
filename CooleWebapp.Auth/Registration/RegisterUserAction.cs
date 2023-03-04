using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Auth.DefaultUsers;
using CooleWebapp.Auth.Managers;
using CooleWebapp.Auth.Model;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CooleWebapp.Auth.Registration;

public class RegisterUserAction : IBusinessAction<RegistrationData, UserRegistrationOutDto>
{
  private readonly IUserManager _userManager;
  private readonly IUserDataAccess _userDataAccess;
  private readonly IUserRoleStore<WebappUser> _userRoleStore;
  private readonly IOptions<AdministratorsConfiguration> _adminOptions;
  public RegisterUserAction(
    IOptions<AdministratorsConfiguration> adminOptions,
    IUserRoleStore<WebappUser> userRoleStore,
    IUserManager userManager,
    IUserDataAccess userDataAccess)
  {
    _adminOptions = adminOptions;
    _userRoleStore = userRoleStore;
    _userManager = userManager;
    _userDataAccess = userDataAccess;
  }

  public async Task<UserRegistrationOutDto> Run(RegistrationData registrationData, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(registrationData.Email);
    if (user is not null)
      throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {registrationData.Email} already exists.");
    if (_adminOptions.Value.AllowedEmailPatterns.Any() &&
      !_adminOptions.Value.AllowedEmailPatterns.Any(pattern => registrationData.Email.IsLike(pattern)))
      throw new ClientError(ErrorType.Forbidden, "The email address is not allowed to be registered.");
    user = new WebappUser() { Email = registrationData.Email, UserName = registrationData.Email };
    await _userManager.CreateAsync(user, registrationData.Password);
    await _userRoleStore.AddToRoleAsync(user, Roles.Registered.ToUpper(), ct);
    if (_adminOptions.Value.FridgeUserEmails.Any(email => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
    {
      await _userRoleStore.AddToRoleAsync(user, Roles.Fridge.ToUpper(), ct);
    }
    else
    {
      if (_adminOptions.Value.AdministratorEmails.Any(email => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        await _userRoleStore.AddToRoleAsync(user, Roles.Administrator.ToUpper(), ct);
      await _userRoleStore.AddToRoleAsync(user, Roles.User.ToUpper(), ct);
    }
    await _userDataAccess.CreateUser(new()
    {
      Name = registrationData.Name,
      Initials = registrationData.Initials,
      Title = registrationData.Title,
      WebappUserId = user!.Id
    }, ct);
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    return new(token);
  }
}
