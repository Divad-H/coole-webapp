namespace CooleWebapp.Database.Configuration;

public sealed record DatabaseConfig
{
  public string DatabaseConnectionString { get; set; } = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;";
}
