using Services;
using Reports;

namespace ReportGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        var openAIService = new OpenAIService();
        var reportService = new ReportService();

        // Reports
        var logitBiasReport = new LogitBias(openAIService, reportService);

        // Generation
        logitBiasReport.generate();

    }
}

