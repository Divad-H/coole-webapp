using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration;

public class RegenerateConfirmationEmailTokenAction : IBusinessAction<string, UserRegistrationOutDto>
{
  private readonly IUserManager _userManager;
  public RegenerateConfirmationEmailTokenAction(IUserManager userManager)
  {
    _userManager = userManager;
  }

  public async Task<UserRegistrationOutDto> Run(string dataIn, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(dataIn) 
      ?? throw new ClientError(ErrorType.InvalidOperation, $"A user with the email {dataIn} does not exist.");
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    return new(token);
  }
}
