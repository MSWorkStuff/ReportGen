using System.Text.Json;
using System.IO;
using Azure.AI.OpenAI;

namespace Services;

public class ReportService {

    public void Report(
            List<CompletionsEntry> entries)
    {
        using var fileWriter = GetFileHandle();

        foreach(var entry in entries) {
            fileWriter.WriteLine($"## {entry.title}");
            fileWriter.WriteLine();

            entry.inputQuery.Invoke(fileWriter, entry.input);

            var output = entry.generator.Invoke(entry.input);

            entry.ouputQuery.Invoke(fileWriter, output);
        }
    }

    private StreamWriter GetFileHandle() {
        var logFile = File.Create("report.md");
        return new StreamWriter(logFile);
    }
}

public abstract class Entry<Input, Output> {

    public string title { get; init; }
    public Input input { get; init; }

    public Func<Input, Output> generator { get; init; }
    public Action<StreamWriter, Input> inputQuery { get; init; }
    public Action<StreamWriter, Output> ouputQuery { get; init; }
}

public class CompletionsEntry: Entry<CompletionsOptions, Completions> {

    public CompletionsEntry(
        string title,
        CompletionsOptions input,
        Action<StreamWriter, CompletionsOptions> inputQuery,
        Action<StreamWriter, Completions> outputQuery,
        OpenAIService openAIService)
    {
        this.title = title;
        this.input = input;
        this.inputQuery = inputQuery;
        this.ouputQuery = outputQuery;
        this.generator = (entry) => {
                return openAIService.GetCompletionsSimple(entry);
            };
    }
}
