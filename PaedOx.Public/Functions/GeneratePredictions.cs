using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class GeneratePredictions
{
    public static async Task FromUnstructured(IAnalysis Client)
    {
        Console.Write("Enter path to data: ");
        var DataPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to write prediction data: ");
        var WritePath = Console.ReadLine() ?? string.Empty;
        StreamWriter Writer = new(Path.Combine(WritePath!, "PaedOx_processed.txt"));

        Console.WriteLine("\nYou may apply a Sleep Stager for filtering of Wake.");
        Console.WriteLine("\t0. No,");
        Console.WriteLine("\t1. Yes.");
        _ = int.TryParse(Console.ReadLine(), out var Selection);

        var SleepStager = string.Empty;

        switch (Selection)
        {
            case 0:
                Console.WriteLine("Sleep staging disabled.");
                break;
            case 1:
                Console.Write("Enter the name of the Sleep Stager: ");
                SleepStager = Console.ReadLine() ?? string.Empty;
                break;
            default:
                Console.WriteLine("Invalid selection, returning to main menu.");
                return;
        }

        Console.Write("Enter the name of the predictive model: ");
        var PredictiveModel = Console.ReadLine() ?? string.Empty;

        var Data = Directory.GetFiles(DataPath);
        await Writer.WriteLineAsync($"Recording,Prediction");

        for (var i = 0; i < Data.Length; i++)
        {
            var RecordingName = Path.GetFileName(Data[i]);
            var RecordingData = await File.ReadAllTextAsync(Data[i]);

            var Dto = new OximetryDto(
                RecordingData,
                PredictiveModel,
                SleepStager);
            var Result = await Client.Post(Dto);

            Console.WriteLine($"Received prediction data for recording {RecordingName}...");
            await Writer.WriteLineAsync($"{RecordingName},{Math.Round(Result.Score, 2)}");
        }

        Writer.Close();
    }
}
