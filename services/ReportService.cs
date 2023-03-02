using System.Text.Json;
using System.IO;
namespace Services;

public class ReportService {

    public void Report<Input, Output>(
            List<Entry<Input, Output>> entries,
            Func<Input, Output> generator) {
        using var fileWriter = GetFileHandle();

        foreach(var entry in entries) {
            fileWriter.WriteLine($"## {entry.title}");
            fileWriter.WriteLine();

            fileWriter.WriteLine($"### Input JSON");
            var inputJson = JsonSerializer.Serialize(entry.input, new JsonSerializerOptions { WriteIndented = true });
            fileWriter.WriteLine(inputJson);
            fileWriter.WriteLine();

            fileWriter.WriteLine($"### Output JSON");
            var output = generator.Invoke(entry.input);
            var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });
            fileWriter.WriteLine(outputJson);
            fileWriter.WriteLine();
        }
    }

    private StreamWriter GetFileHandle() {
        var logFile = File.Create("report.md");
        return new System.IO.StreamWriter(logFile);
    }
}

public class Entry<Input, Output> {
    public string title { get; init;}
    public Input input { get; init; }

    public Entry(string title, Input input) {
        this.title = title;
        this.input = input;
    }
}

/*

## Entry.Title

```json
Entry.InputJson.Serialize()
```

```json
Entry.OutputJson.Serialize()
```

<todo>
...

*/
