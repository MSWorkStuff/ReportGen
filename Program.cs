using Azure.AI.OpenAI;

namespace ReportGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        var openAIService = new Services.OpenAIService();
        var reportService = new Services.ReportService();

        reportService.Report(
            new List<Services.Entry<CompletionsOptions, Completions>> {
                new Services.Entry<CompletionsOptions, Completions>(
                    "Title text",
                    new CompletionsOptions()
                        {
                            Prompt = { "Tell me a joke about clouds" },
                            MaxTokens = 512,
                            LogProbability = 1,
                        }
                )
            },
            (entry) => {
                return openAIService.GetCompletionsSimple(entry);
            }
        );
    }
}

