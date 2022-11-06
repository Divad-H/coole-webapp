namespace CooleWebapp.Auth.DefaultUsers
{
  public record AdministratorsConfiguration
  {
    /// <summary>
    /// When a user registers with an email address that is contained in this
    /// list, the user will get the administrator role.
    /// </summary>
    public string[] AdministratorEmails { get; set; } = Array.Empty<string>();
  }
}
