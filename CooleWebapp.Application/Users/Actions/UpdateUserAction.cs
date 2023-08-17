using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Application.Users.Services;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Application.Users.Actions;

internal sealed class UpdateUserAction : IBusinessAction<UpdateUserDto, Unit>
{
  private readonly IUserDataAccess _userDataAccess;
  public UpdateUserAction(IUserDataAccess userDataAccess)
  {
    _userDataAccess = userDataAccess;
  }

  public async Task<Unit> Run(UpdateUserDto dataIn, CancellationToken ct)
  {
    var isFridge = dataIn.Roles.Contains(UserRole.Fridge);
    var isUser = dataIn.Roles.Contains(UserRole.User);
    
    if (!isFridge && !isUser)
    {
      throw new ClientError(
        ErrorType.InvalidOperation, "At least the User or the Fridge role must be set.");
    }
    else if (isFridge && isUser)
    {
      throw new ClientError(
        ErrorType.InvalidOperation, "A fridge cannot be a normal user.");
    }

    var isAdmin = dataIn.Roles.Contains(UserRole.Administrator);

    List<string> roles = new()
    {
      Core.Entities.Roles.Registered,
      isUser ? Core.Entities.Roles.User : Core.Entities.Roles.Fridge,
    };
    if (isAdmin)
    {
      roles.Add(Core.Entities.Roles.Administrator);
    }

    await _userDataAccess.SetUserRoles(dataIn.Id, roles, ct);
    return Unit.Default;
  }
}
