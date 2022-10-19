namespace CooleWebapp.Core.Entities
{
  public record CoolUser
  {
    public ulong Id { get; set; }
    public string WebappUserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Initials { get; set; } = string.Empty;
  }
}
