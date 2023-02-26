using CooleWebapp.Application.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace CooleWebapp.Auth.PinCode;

internal class DummyUser { }

internal class PinCodeHashing : IPinCodeHashing
{
  private readonly IPasswordHasher<DummyUser> _passwordHasher;
  public PinCodeHashing(IPasswordHasher<DummyUser> passwordHasher)
  {
    _passwordHasher = passwordHasher;
  }

  public string GetHash(string pinCode) => 
    _passwordHasher.HashPassword(new(), pinCode);

  public bool VerifyHash(string pinCode, string hash) =>
    _passwordHasher.VerifyHashedPassword(new(), hash, pinCode) != PasswordVerificationResult.Failed;
}
