using Azure.AI.OpenAI;
using Services;
using System.Text.Json;

namespace ReportGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        var openAIService = new OpenAIService();
        var reportService = new ReportService();

        reportService.Report(
            new List<CompletionsEntry> {
                new CompletionsEntry (
                    "LogitBias value -100",
                    new CompletionsOptions()
                        {
                            Prompt = { "Tell me a joke about clouds" },
                            MaxTokens = 512,
                            LogProbability = 1,
                        },
                    (fileWriter, input) => {
                        fileWriter.WriteLine($"### Input JSON");
                        var inputJson = JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true });
                        fileWriter.WriteLine(inputJson);
                        fileWriter.WriteLine();
                    },
                    (fileWriter, output) => {
                        fileWriter.WriteLine($"### Output JSON");
                        var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
                        fileWriter.WriteLine(outputJson);
                        fileWriter.WriteLine();
                    },
                    openAIService
                )
            }
        );
    }
}

