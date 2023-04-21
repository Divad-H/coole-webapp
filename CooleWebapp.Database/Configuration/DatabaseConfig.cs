namespace CooleWebapp.Database.Configuration;

public sealed record DatabaseConfig
{
  public bool InitializeDatabase { get; set; } = true;
  public string DatabaseConnectionString { get; set; } = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;";
}
