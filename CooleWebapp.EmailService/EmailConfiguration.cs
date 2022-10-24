namespace CooleWebapp.EmailService;

internal record EmailConfiguration
{
  public string FromAddress { get; set; } = string.Empty;
  public string FromName { get; set; } = "Coole Webapp";
  public string SmtpServer { get; set; } = string.Empty;
  public int Port { get; set; }
  public string UserName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
