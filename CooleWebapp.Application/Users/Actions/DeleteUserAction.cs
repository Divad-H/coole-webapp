using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Application.Users.Actions;

internal sealed class DeleteUserAction : IBusinessAction<UInt64, Unit>
{
  private readonly IUserDataAccess _userDataAccess;
  public DeleteUserAction(IUserDataAccess userDataAccess)
  {
    _userDataAccess = userDataAccess;
  }

  public async Task<Unit> Run(UInt64 coolUserid, CancellationToken ct)
  {
    await _userDataAccess.DeleteUser(coolUserid, ct);
    return Unit.Default;
  }
}
