using System.Text.Json;
using PaedOx.Contracts.Datasets;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class GenerateFeatureData
{
    public static async Task FromUnstructured(IFeatureData client)
    {
        Console.WriteLine(
            "\nOximetry data and labels must be stored separately with identical (paired) naming."
        );
        Console.Write("Enter path to training data: ");
        var dataPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to labels: ");
        var labelPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Enter path to write feature data: ");
        var writePath = Console.ReadLine() ?? string.Empty;

        // API currently requires a saved model as an input.
        // The selected model does not change training data generation.
        // To be resolved in a future API update.
        Console.Write("\nEnter name of predictive model: ");
        var model = Console.ReadLine() ?? string.Empty;

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

        var data = Directory.GetFiles(dataPath);
        var labels = Directory.GetFiles(labelPath);

        List<List<double>> dataX = [];
        List<double> dataYClassification = [];
        List<double> dataYRegression = [];

        for (var i = 0; i < data.Length; i++)
        {
            var recordingName = Path.GetFileName(data[i]);
            var recordingData = await File.ReadAllTextAsync(data[i]);
            var recordingLabel = await File.ReadAllTextAsync(labels[i]);

            var dto = new OximetryDto(recordingData, model, sleepStager);
            var result = await client.Post(dto);

            if (result.Value is not null)
            {
                Console.WriteLine($"Received feature data for recording {recordingName}...");
            }
            else
            {
                Console.WriteLine($"No feature data for recording {recordingName}, skipping...");

                continue;
            }

            _ = double.TryParse(recordingLabel, out var label);

            dataX.Add(result.Value);
            dataYClassification.Add(label >= 5 ? +1 : -1);
            dataYRegression.Add(label);
        }

        DatasetDto dataset = new(dataX, dataYClassification, dataYRegression);

        var Options = new JsonSerializerOptions { WriteIndented = true };

        var datasetData = JsonSerializer.Serialize(dataset, Options);
        await File.WriteAllTextAsync(Path.Combine(writePath!, "Dataset.json"), datasetData);
    }
}
