using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Auth.Registration
{
  public class ResetPasswordAction : IBusinessAction<ResetPasswordDto, Unit>
  {
    private readonly IUserManager _userManager;
    public ResetPasswordAction(IUserManager userManager)
    {
      _userManager = userManager;
    }

    public async Task<Unit> Run(ResetPasswordDto dataIn, CancellationToken ct)
    {
      var user = await _userManager.FindByEmailAsync(dataIn.Email);
      if (user is null)
        throw new ClientError(ErrorType.NotFound, "A user with that e-mail was not found.");
      await _userManager.ResetPasswordAsync(user, dataIn.Token, dataIn.Password);
      return Unit.Default;
    }
  }
}
