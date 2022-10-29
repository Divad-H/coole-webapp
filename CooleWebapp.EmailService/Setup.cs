using CooleWebapp.Application.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.EmailService;

public static class Setup
{
  public static void AddEmailSender(
    this IServiceCollection serviceDescriptors,
    IConfigurationRoot configurationRoot)
  {
    serviceDescriptors
      .AddScoped<IEmailSender, EmailSender>();
    serviceDescriptors
      .AddOptions<EmailConfiguration>()
      .Configure(o => o.FromName = "")
      .Bind(configurationRoot.GetSection($"{nameof(EmailSender)}"));
  }
}
