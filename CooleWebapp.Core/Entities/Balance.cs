namespace CooleWebapp.Core.Entities
{
  public record Balance
  {
    public ulong Id { get; set; }
    public UInt64 CoolUserId { get; set; }
    public decimal Value { get; set; }
    public Guid Version { get; set; }

    public CoolUser? CoolUser { get; set; }
  }
}
