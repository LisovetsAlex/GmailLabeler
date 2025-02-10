using TaskEmailCategorize;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var apiSettings = config.GetSection("Settings").Get<ApiSettings>();

        var email = new EmailHandler(apiSettings);

        email.Connect("imap.gmail.com");

        await email.CategorizeUnreadEmailsAsync();

        email.Disconnect();
    }
}