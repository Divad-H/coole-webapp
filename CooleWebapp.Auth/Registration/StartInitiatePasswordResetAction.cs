using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Registration
{
  public class StartInitiatePasswordResetAction : IBusinessAction<StartInitiatePasswordResetDto, string>
  {
    private readonly IUserManager _userManager;
    public StartInitiatePasswordResetAction(
      IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<string> Run(StartInitiatePasswordResetDto dataIn, CancellationToken ct)
    {
      var user = await _userManager.FindByEmailAsync(dataIn.Email);
      if (user is null)
        throw new ClientError(ErrorType.NotFound, "No user with this e-mail was found.");
      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      return token;
    }
  }
}
