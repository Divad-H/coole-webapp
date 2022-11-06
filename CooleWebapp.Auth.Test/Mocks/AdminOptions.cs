using CooleWebapp.Auth.DefaultUsers;
using Microsoft.Extensions.Options;

namespace CooleWebapp.Auth.Test.Mocks
{
  internal class AdminOptions : IOptions<AdministratorsConfiguration>
  {
    public AdministratorsConfiguration Value
      => new();
  }
}
