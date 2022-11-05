using CooleWebapp.Auth.Model;
using CooleWebapp.Auth.Registration;
using CooleWebapp.Auth.Test.Mocks;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Auth.Test.Registration
{
  [TestClass]
  public class RegisterUserActionTest
  {
    private class UserManagerWithExistingEmail : UserManager
    {
      public const string ExistingEmail = "e@mail.com";
      public override Task<WebappUser?> FindByEmailAsync(string email)
      {
        if (email == ExistingEmail)
          return Task.FromResult<WebappUser?>(new ()
          {
            Email = ExistingEmail
          });
        return base.FindByEmailAsync(email);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(ClientError))]
    public async Task ThrowsClientErrorWhenUserEmailAlreadyExists()
    {
      var action = new RegisterUserAction(
        new UserRoleStore(),
        new UserManagerWithExistingEmail(),
        new UserDataAccess());
      await action.Run(
        new()
        {
          Name = "Name",
          Initials = "INS",
          Password = "P@ssw0rd",
          Email = UserManagerWithExistingEmail.ExistingEmail
        }, 
        CancellationToken.None);
    }

    private class UserManagerMock : UserManager
    {
      public const string ExpectedPassword = "P@ssw0rd";
      public const string ExpectedEmail = "my@e.mail";
      public const string ExpectedToken = "token";
      private WebappUser? _registeredUser;
      public override Task<WebappUser?> FindByEmailAsync(string email)
      {
        if (_registeredUser is not null && _registeredUser.Email == email)
          return Task.FromResult<WebappUser?>(_registeredUser);
        return Task.FromResult<WebappUser?>(null);
      }

      public override Task CreateAsync(WebappUser user, string password)
      {
        Assert.AreEqual(ExpectedPassword, password);
        
        _registeredUser = new()
        {
          AccessFailedCount = user.AccessFailedCount,
          ConcurrencyStamp = user.ConcurrencyStamp,
          Email = user.Email,
          TwoFactorEnabled = user.TwoFactorEnabled,
          EmailConfirmed = user.EmailConfirmed,
          PhoneNumberConfirmed = user.PhoneNumberConfirmed,
          PasswordHash = user.PasswordHash,
          PhoneNumber = user.PhoneNumber,
          Id = user.Id,
      };
        return Task.CompletedTask;
      }

      public override Task<string> GenerateEmailConfirmationTokenAsync(WebappUser webappUser)
      {
        Assert.AreEqual(_registeredUser?.Id, webappUser.Id);
        return Task.FromResult(ExpectedToken);
      }
    }

    [TestMethod]
    public async Task RegisteredUserIsAddedToUserManagerAndUserData()
    {
      var userManagerMock = new UserManagerMock();
      var userDataAccessMock = new UserDataAccess();
      var action = new RegisterUserAction(
        new UserRoleStore(),
        userManagerMock,
        userDataAccessMock);
      var res = await action.Run(
        new()
        {
          Name = "Name",
          Initials = "INS",
          Password = UserManagerMock.ExpectedPassword,
          Email = UserManagerMock.ExpectedEmail,
          Title = "Title"
        },
        CancellationToken.None);
      Assert.AreEqual(UserManagerMock.ExpectedToken, res.Token);
      Assert.AreEqual(1, userDataAccessMock.CreatedUsers.Count);
      var createdWebappUser = await userManagerMock.FindByEmailAsync(UserManagerMock.ExpectedEmail);
      Assert.IsNotNull(createdWebappUser);
      var createdCoolUser = userDataAccessMock.CreatedUsers.First();
      Assert.AreEqual(createdWebappUser.Id, createdCoolUser.WebappUserId);
      Assert.AreEqual("INS", createdCoolUser.Initials);
      Assert.AreEqual("Name", createdCoolUser.Name);
      Assert.AreEqual("Title", createdCoolUser.Title);
    }
  }
}
