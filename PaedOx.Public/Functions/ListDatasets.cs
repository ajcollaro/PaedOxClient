using PaedOx.Contracts.Datasets;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ListDatasets
{
    public static async Task List(IDatasets Client)
    {
        Console.WriteLine("\nEnter a dataset type:");
        Console.WriteLine("\t0. Sleep Disordered Breathing,");
        Console.WriteLine("\t1. Sleep Stager,");
        Console.WriteLine("\t2. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var Selection);

        if (Selection == 2)
        {
            return;
        }

        DatasetsDto Dto = new(Selection, []);
        var Result = await Client.Post(Dto);

        Console.WriteLine("\nAvailable Datasets:");

        for (var i = 0; i < Result.DatasetNames.Count; i++)
        {
            Console.WriteLine($"\t{i}. {Result.DatasetNames[i]}");
        }
    }
}
