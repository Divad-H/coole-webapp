namespace CooleWebapp.Auth.DefaultUsers;

public record AdministratorsConfiguration
{
  /// <summary>
  /// When a user registers with an email address that is contained in this
  /// list, the user will get the administrator role.
  /// </summary>
  public string[] AdministratorEmails { get; set; } = Array.Empty<string>();

  /// <summary>
  /// When a user registers with en email address that is contained in this
  /// list, the user will get the Fridge role. This user will not get the User 
  /// or Administrator role.
  /// </summary>
  public string[] FridgeUserEmails { get; set; } = Array.Empty<string>();

  /// <summary>
  /// A list of allowed email address patterns using wild cards. Only matching email
  /// addresses are allowed to register.
  /// "*" stands for any sequence of characters.
  /// "?" stands for any single character.
  /// <para>
  /// Example: "*@coolewebapp.com" would only allow email addresses ending with
  /// "@coolewebapp.com" to register.
  /// </para>
  /// </summary>
  public string[] AllowedEmailPatterns { get; set; } = new string[] { };
}
