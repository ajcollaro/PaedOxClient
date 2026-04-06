using PaedOx.Contracts.Datasets;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;
using System.Text.Json;

namespace PaedOx.Public.Functions;

internal static class GenerateFeatureData
{
    public static async Task FromUnstructured(IFeatureData Client)
    {
        Console.WriteLine("\nOximetry data and labels must be stored separately with identical (paired) naming.");
        Console.Write("Enter path to training data: ");
        var DataPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to labels: ");
        var LabelPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to write feature data: ");
        var WritePath = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("\nYou may apply a Sleep Stager for filtering of probable Wake.");
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

        var Data = Directory.GetFiles(DataPath);
        var Labels = Directory.GetFiles(LabelPath);

        List<List<double>> DataX = [];
        List<double> DataYClassification = [];
        List<double> DataYRegression = [];

        for (var i = 0; i < Data.Length; i++)
        {
            var RecordingName = Path.GetFileName(Data[i]);
            var RecordingData = await File.ReadAllTextAsync(Data[i]);
            var RecordingLabel = await File.ReadAllTextAsync(Labels[i]);

            var Dto = new OximetryDto(
                RecordingData,
                string.Empty,
                SleepStager);
            var Result = await Client.Post(Dto);

            if (Result.Value is not null)
            {
                Console.WriteLine($"Received feature data for recording {RecordingName}...");
            }
            else
            {
                Console.WriteLine($"No feature data for recording {RecordingName}, skipping...");
                continue;
            }

            _ = double.TryParse(RecordingLabel, out var Label);

            DataX.Add(Result.Value);
            DataYClassification.Add(Label >= 5 ? +1 : -1);
            DataYRegression.Add(Label);
        }

        DatasetDto Dataset = new(DataX, DataYClassification, DataYRegression);

        var Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var DatasetData = JsonSerializer.Serialize(Dataset, Options);
        await File.WriteAllTextAsync(Path.Combine(WritePath!, "Dataset.json"), DatasetData);
    }
}
