using PaedOx.Contracts.Models;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ListModels
{
    public static async Task List(IModels client)
    {
        Console.WriteLine("\nEnter a model type:");
        Console.WriteLine("\t0. Sleep-Disordered Breathing,");
        Console.WriteLine("\t1. Sleep Stager,");
        Console.WriteLine("\t2. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var selection);

        if (selection == 2)
        {
            return;
        }

        ModelsDto dto = new(selection, []);
        var result = await client.Post(dto);

        Console.WriteLine("\nAvailable Models:");

        for (var i = 0; i < result.ModelNames.Count; i++)
        {
            Console.WriteLine($"\t{i}. {result.ModelNames[i]}");
        }
    }
}
