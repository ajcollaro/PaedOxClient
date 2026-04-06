using PaedOx.Contracts.Datasets;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ListDatasets
{
    public static async Task List(IDatasets client)
    {
        Console.WriteLine("\nEnter a dataset type:");
        Console.WriteLine("\t0. Sleep-Disordered Breathing,");
        Console.WriteLine("\t1. Sleep Staging,");
        Console.WriteLine("\t2. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var selection);

        if (selection == 2)
        {
            return;
        }

        DatasetsDto dto = new(selection, []);
        var result = await client.Post(dto);

        Console.WriteLine("\nAvailable Datasets:");

        for (var i = 0; i < result.DatasetNames.Count; i++)
        {
            Console.WriteLine($"\t{i}. {result.DatasetNames[i]}");
        }
    }
}
