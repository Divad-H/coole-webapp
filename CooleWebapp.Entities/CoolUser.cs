namespace CooleWebapp.Entities
{
  public record CoolUser
  {
    public UInt64 Id { get; set; }
    public string WebappUserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Initials { get; set; } = string.Empty;
  }
}
