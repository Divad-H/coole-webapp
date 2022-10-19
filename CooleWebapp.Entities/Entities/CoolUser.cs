namespace CooleWebapp.Core.Entities
{
  /// <summary>
  /// Application user data
  /// </summary>
  public record CoolUser
  {
    /// <summary>
    /// Unique id that identifies the user
    /// </summary>
    public ulong Id { get; set; }
    /// <summary>
    /// The id of the related WebappUser that contains login information
    /// </summary>
    public string WebappUserId { get; set; } = string.Empty;
    /// <summary>
    /// The name of the user
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The title of the user (e.g. Lord)
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// The three letter initials of the user
    /// </summary>
    public string Initials { get; set; } = string.Empty;
  }
}
