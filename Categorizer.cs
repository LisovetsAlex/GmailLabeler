using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using Tiktoken;

namespace TaskEmailCategorize
{
    public class Categorizer
    {
        private ApiSettings _apiSettings;

        public Categorizer(ApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<string> CategorizeEmailAsync(string emailSubject, string emailBody)
        {
            var encoder = ModelToEncoder.For("gpt-3.5-turbo");
            using var api = new OpenAIClient(new OpenAIAuthentication(
                _apiSettings.OpenAiApiKey,
                _apiSettings.OpenAiOrgKey,
                _apiSettings.OpenAiProjKey));

            int tokenCount = encoder.CountTokens(emailBody);

            string trimmedBody = emailBody;
            if (tokenCount > 500)
            {
                var tokens = encoder.Encode(emailBody).Take(500).ToList();
                trimmedBody = encoder.Decode(tokens) + "...";
            }

            List<string> cats = new List<string>();
            foreach (var category in _apiSettings.Categories)
            {
                cats.Add($"{category.Name} - {category.Description} \n");
            }

            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant, who can categorize incoming EMails into following categories: \n" +
                    cats +
                    "The answer should contain only the category name"),
                new Message(Role.User, $"Subject: {emailSubject}\nBody: {trimmedBody}")
            };
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            var choice = response.FirstChoice;

            return $"{choice.Message.Content}";
        }
    }
}
