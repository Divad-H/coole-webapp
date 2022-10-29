using CooleWebapp.Auth.Managers;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Auth.Registration;

public class ConfirmEmailAction : IBusinessAction<ConfirmEmailActionDto, Unit>
{
  private readonly IUserManager _userManager;
  public ConfirmEmailAction(IUserManager userManager) 
  { 
    _userManager = userManager; 
  }
  public async Task<Unit> Run(ConfirmEmailActionDto dataIn, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(dataIn.Email);
    if (user is null)
      throw new ClientError(ErrorType.NotFound, "Invalid e-mail confirmation link. User not found.");

    await _userManager.ConfirmEmailAsync(user, dataIn.Token);
    return Unit.Default;
  }
}
