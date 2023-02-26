namespace CooleWebapp.Application.Users.Services;

public interface IPinCodeHashing
{
  string GetHash(string pinCode);
  bool VerifyHash(string pinCode, string hash);
}
