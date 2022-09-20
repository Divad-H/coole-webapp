namespace Database.Configuration
{
  public sealed record DatabaseConfig
  {
    public string DatabaseName { get; set; } = "coole-webapp/coole-webapp-db";
  }
}
