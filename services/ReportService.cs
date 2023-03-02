using System.Text.Json;

namespace Services;

public class ReportService {

    public void Report<Input, Output>(
        List<Entry<Input, Output>> entries,
        Func<Input, Output> generator) {

        foreach(var entry in entries) {
            var inputJson = JsonSerializer.Serialize(entry.input, new JsonSerializerOptions { WriteIndented = true });

            var output = generator.Invoke(entry.input);

            var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions { WriteIndented = true });

            Console.WriteLine(inputJson);
            Console.WriteLine(outputJson);
        }
    }
}

// CompletionsOptions == Input
// Completions == Output
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
