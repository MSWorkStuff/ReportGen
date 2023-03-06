using Services;
using Azure.AI.OpenAI;
using System.Text.Json;

namespace Reports;

public class LogitBias {

    private OpenAIService openAIService { get; init; }
    private ReportService reportService { get; init; }

    public LogitBias(OpenAIService openAIService, ReportService reportService) {
         this.openAIService = openAIService;
         this.reportService = reportService;
    }

    public void generate() {
        reportService.Report(
            "logit_bias.md",
            reportEntries()
        );
    }

    private List<CompletionsEntry> reportEntries() {
        return new List<CompletionsEntry> {
                new CompletionsEntry (
                    "LogitBias value -100",
                    new CompletionsOptions()
                        {
                            Prompt = { "Write a haiku about lollipops" },
                            MaxTokens = 40,
                            LogProbability = 1,
                            LogitBias = {
                                { "34751", -100}
                            }
                        },
                    (fileWriter, input) => {
                        fileWriter.WriteLine($"### Input JSON");
                        var inputJson = JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true });
                        // fileWriter.WriteLine(inputJson);
                        fileWriter.WriteLine(reportService.CollapsableCode("json", "Input Json", inputJson));
                        fileWriter.WriteLine();
                    },
                    (fileWriter, output) => {
                        fileWriter.WriteLine($"### Output JSON");
                        var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
                        // fileWriter.WriteLine(outputJson);
                        fileWriter.WriteLine(reportService.CollapsableCode("json", "Output Json", outputJson));
                        fileWriter.WriteLine();
                    },
                    openAIService
                ),

                new CompletionsEntry (
                    "LogitBias value 100",
                    new CompletionsOptions()
                        {
                            Prompt = { "Write a haiku about lollipops" },
                            MaxTokens = 40,
                            LogProbability = 1,
                            LogitBias = {
                                { "34751", 100}
                            }
                        },
                    (fileWriter, input) => {
                        fileWriter.WriteLine($"### Input JSON");
                        var inputJson = JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true });
                        fileWriter.WriteLine(reportService.CollapsableCode("json", "Input Json", inputJson));
                        fileWriter.WriteLine();
                    },
                    (fileWriter, output) => {
                        fileWriter.WriteLine($"### Output JSON");
                        var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
                        // fileWriter.WriteLine(outputJson);
                        fileWriter.WriteLine(reportService.CollapsableCode("json", "Output Json", outputJson));
                        fileWriter.WriteLine();
                    },
                    openAIService
                )
            };

    }
}
