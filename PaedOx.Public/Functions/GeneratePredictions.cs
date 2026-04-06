using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class GeneratePredictions
{
    public static async Task FromUnstructured(IAnalysis client)
    {
        Console.Write("Enter path to data: ");
        var dataPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to write prediction data: ");
        var writePath = Console.ReadLine() ?? string.Empty;
        StreamWriter writer = new(Path.Combine(writePath!, "PaedOx_processed.txt"));

        Console.WriteLine("Select a Sleep Stager to remove periods of Wake?");
        Console.WriteLine("\t0. No,");
        Console.WriteLine("\t1. Yes.");
        _ = int.TryParse(Console.ReadLine(), out var selection);

        var sleepStager = string.Empty;

        switch (selection)
        {
            case 0:
                Console.WriteLine("Sleep staging disabled.");

                break;
            case 1:
                Console.Write("Enter the name of the Sleep Stager: ");
                sleepStager = Console.ReadLine() ?? string.Empty;

                break;
            default:
                Console.WriteLine("Invalid selection, returning to main menu.");

                return;
        }

        Console.Write("Enter the name of the predictive model: ");
        var predictiveModel = Console.ReadLine() ?? string.Empty;

        var data = Directory.GetFiles(dataPath);
        await writer.WriteLineAsync($"Recording,Prediction");

        for (var i = 0; i < data.Length; i++)
        {
            var recordingName = Path.GetFileName(data[i]);
            var recordingData = await File.ReadAllTextAsync(data[i]);

            var dto = new OximetryDto(recordingData, predictiveModel, sleepStager);
            var Result = await client.Post(dto);

            Console.WriteLine($"Received prediction data for recording {recordingName}...");
            await writer.WriteLineAsync($"{recordingName},{Math.Round(Result.Score, 2)}");
        }

        writer.Close();
    }
}
