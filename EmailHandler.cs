using MailKit.Search;
using MailKit;
using MailKit.Net.Imap;
using TaskEmailCategorize.Models;
using Microsoft.Extensions.Configuration;

namespace TaskEmailCategorize
{
    public class EmailHandler
    {
        private ImapClient _client;
        private Categorizer _categorizer;
        private ApiSettings _apiSettings;

        public EmailHandler(ApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
            _categorizer = new Categorizer(apiSettings);
            _client = new ImapClient();
        }

        public void Connect(string host)
        {
            Console.WriteLine($"IMAP Client connecting...");

            try
            {
                _client.Connect(host, 993, true);

                _client.Authenticate(_apiSettings.ImapAddress, _apiSettings.ImapPass);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine($"IMAP Client connected");
        }

        public void Disconnect()
        {
            _client.Disconnect(true);

            Console.WriteLine($"IMAP Client disconnected");
        }

        public List<EMailViewModel> GetLastEmail(int count)
        {
            if (!_client.IsConnected || !_client.IsAuthenticated)
            {
                Console.WriteLine($"IMAP client not connected or not authenticated");
                return new List<EMailViewModel>();
            }

            var inbox = _client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var uids = inbox.Search(SearchQuery.All).TakeLast(count);
            List <EMailViewModel> emails = new List<EMailViewModel>();

            foreach (var id in uids)
            {
                var message = inbox.GetMessage(id);

                emails.Add(new EMailViewModel {
                    Id = id.ToString(),
                    Body = message.TextBody ?? message.HtmlBody,
                    Subject = message.Subject,
                });
            }

            return emails;
        }

        public async Task CategorizeUnreadEmailsAsync()
        {
            if (!_client.IsConnected || !_client.IsAuthenticated)
            {
                Console.WriteLine($"IMAP client not connected or not authenticated");
                return;
            }

            Console.WriteLine($"Categorizing emails...");

            var emails = GetLastEmail(25);

            var inbox = _client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);

            foreach (var email in emails)
            {
                var category = await _categorizer.CategorizeEmailAsync(email.Subject, email.Body);

                UniqueId emailId = UniqueId.Parse(email.Id);

                await inbox.AddLabelsAsync(emailId, new[] { category }, false);
            }

            Console.WriteLine($"Finished categorizing");
        }
    }
}
