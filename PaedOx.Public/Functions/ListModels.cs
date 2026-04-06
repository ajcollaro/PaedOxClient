using PaedOx.Contracts.Models;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ListModels
{
    public static async Task List(IModels Client)
    {
        Console.WriteLine("\nEnter a model type:");
        Console.WriteLine("\t0. Sleep Disordered Breathing,");
        Console.WriteLine("\t1. Sleep Stager,");
        Console.WriteLine("\t2. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var Selection);

        if (Selection == 2)
        {
            return;
        }

        ModelsDto Dto = new(Selection, []);
        var Result = await Client.Post(Dto);

        Console.WriteLine("\nAvailable Models:");

        for (var i = 0; i < Result.ModelNames.Count; i++)
        {
            Console.WriteLine($"\t{i}. {Result.ModelNames[i]}");
        }
    }
}
