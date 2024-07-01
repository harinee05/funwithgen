using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Completions;

namespace ChatConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            var configuration = builder.Build();
            var openAiApiKey = configuration["OpenAI:ApiKey"];

            if (string.IsNullOrEmpty(openAiApiKey))
            {
                Console.WriteLine("API key is missing. Please add the API key to user secrets.");
                return;
            }

            APIAuthentication aPIAuthentication = new APIAuthentication(openAiApiKey);
            OpenAIAPI openAiApi = new OpenAIAPI(aPIAuthentication);

            Console.WriteLine("Do you mind telling me who can I help you charm? (Gender/ Pronouns)");
            var gender = Console.ReadLine();
            Console.WriteLine("Tell me something about your personality, your likes, dislikes, and quirks? What gets you going?");
            var personality = Console.ReadLine();
            Console.WriteLine("Tell us about this person you wanna spill your charm on?");
            var charmer = Console.ReadLine();
            Console.WriteLine("Hm... That's so cool! What situation do you want the pickup line?");
            var context = Console.ReadLine();

            try
            {
                string prompt = $"Craft a pickup line which is short, quirky and fresh for someone with the personality of {personality} to a {gender} with a personality of: {charmer}. This pickup line has to be in the genre of {context}. Do not make it too long, please.";
                string model = "gpt-3.5-turbo-instruct";
                int maxTokens = 80;

                var completionRequest = new CompletionRequest
                {
                    Prompt = prompt,
                    Model = model,
                    MaxTokens = maxTokens,
                    Temperature = 0.5
                };

                var completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);
                var generatedText = completionResult.Completions[0].Text;

                Console.WriteLine("Generated text:");
                Console.WriteLine(generatedText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
