using Azure.AI.OpenAI;

namespace Services;

public class ReportService {

    public void Report(
            string fileName,
            List<CompletionsEntry> entries)
    {
        using var fileWriter = GetFileHandle(fileName);

        foreach(var entry in entries) {
            fileWriter.WriteLine($"## {entry.title}");
            fileWriter.WriteLine();

            entry.inputQuery.Invoke(fileWriter, entry.input);

            var output = entry.generator.Invoke(entry.input);

            entry.ouputQuery.Invoke(fileWriter, output);
        }
    }

    public string CollapsableCode(string language, string title, string code ) {
        return @$"
<details>
<summary>{title}</summary>
```{language}
{code}
```
</details>";
    }

    private StreamWriter GetFileHandle(string fileName) {
        System.IO.Directory.CreateDirectory(REPORT_ROOT_FOLDER);
        var logFile = File.Create($"{REPORT_ROOT_FOLDER}/{fileName}");
        return new StreamWriter(logFile);
    }

    private static string REPORT_ROOT_FOLDER = "gen_report";
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
