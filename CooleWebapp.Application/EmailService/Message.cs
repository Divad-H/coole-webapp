namespace CooleWebapp.Application.EmailService
{
  public record Message(
    IReadOnlyCollection<(string Name, string Address)> To,
    string Subject,
    string Content);
  // Attachments?
}
