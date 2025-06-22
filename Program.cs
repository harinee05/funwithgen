using System;
using System.IO;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;

namespace ChatConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var openAiApiKey = "your_api_key";

            if (string.IsNullOrEmpty(openAiApiKey))
            {
                Console.WriteLine("❌ OPEN_AI_KEY is missing in your .env file.");
                return;
            }

            var openAiApi = new OpenAIAPI(new APIAuthentication(openAiApiKey));

            var mcpEvent = new McpEvent();

            Console.WriteLine("🧑‍💼 Enter your role (e.g. Junior Developer):");
            mcpEvent.Role = Console.ReadLine();

            Console.WriteLine("🏢 Enter your department (e.g. IT, HR, Marketing):");
            mcpEvent.Department = Console.ReadLine();

            Console.WriteLine("📈 Enter your experience level (e.g. Beginner, Intermediate, Advanced):");
            mcpEvent.ExperienceLevel = Console.ReadLine();

            Console.WriteLine("📚 What’s your preferred onboarding style? (Checklist, Conversational, Mixed):");
            mcpEvent.PreferredStyle = Console.ReadLine();

            Console.WriteLine("🎙️ Choose a tone (Friendly, Formal, Supportive):");
            mcpEvent.Tone = Console.ReadLine();

            try
            {
                string prompt = $"""
Event: onboarding_assistant

You are an intelligent onboarding assistant for a corporate company named XYZ.

The user is joining as a {mcpEvent.Role} in the {mcpEvent.Department} department. They have {mcpEvent.ExperienceLevel} experience and prefer a {mcpEvent.PreferredStyle} onboarding style. Please suggest 3 helpful and relevant goals for their first week at work.

Respond in a {mcpEvent.Tone.ToLower()} tone and keep it concise but clear.
""";

                var completionRequest = new CompletionRequest
                {
                    Prompt = prompt,
                    Model = "gpt-3.5-turbo-instruct",
                    MaxTokens = 150,
                    Temperature = 0.7
                };

                var completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);
                var generatedText = completionResult.Completions[0].Text.Trim();

                Console.WriteLine("\n📌 Here are your suggested first-week goals:");
                Console.WriteLine("----------------------------------");
                Console.WriteLine(generatedText);
                Console.WriteLine("----------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }

    public class McpEvent
    {
        public string Type { get; set; } = "onboarding_assistant";
        public string Role { get; set; }
        public string Department { get; set; }
        public string ExperienceLevel { get; set; }
        public string PreferredStyle { get; set; }
        public string Tone { get; set; }
    }
}
