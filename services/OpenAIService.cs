using Azure;
using Azure.AI.OpenAI;

namespace Services;
public class OpenAIService
{

    private OpenAIClient _openAIClient { get; init; }
    public OpenAIService() => _openAIClient = CreateClient();

    public Completions GetCompletionsSimple(CompletionsOptions completionsOptions) {
        Response<Completions> response = _openAIClient.GetCompletions(
                s_Completions_Deployment_Id,
                completionsOptions);

        return response.Value;
    }

    private OpenAIClient CreateClient()
    {
        var uri = new Uri(s_Azure_OpenAI_Endpoint);
        var credentials = new AzureKeyCredential(s_Azure_OpenAI_Secret);
        return new OpenAIClient(uri, credentials);
    }

    private string s_Azure_OpenAI_Endpoint {
        get  => EnvironmentExt.GetEnvOrThrow("AZURE_OPENAI_ENDPOINT");
        set {}
    }

    private string s_Azure_OpenAI_Secret {
        get  => EnvironmentExt.GetEnvOrThrow("AZURE_OPENAI_SECRET");
        set {}
    }

    private string s_Completions_Deployment_Id = "text-davinci-003";
}

public static class EnvironmentExt {
    public static string GetEnvOrThrow(string envVarName) {
        var a = Environment.GetEnvironmentVariable(envVarName);
        return a ?? throw new ArgumentNullException($"Environment variable named \"{envVarName}\" is null or empty");
    }
}
