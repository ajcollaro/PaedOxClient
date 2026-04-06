using PaedOx.Contracts.Storage;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ManageStorage
{
    public static async Task Manage(IStorage client)
    {
        Console.WriteLine("\nSelect from the following storage operations:");
        Console.WriteLine("\t0. Reload saved datasets,");
        Console.WriteLine("\t1. Reload saved models,");
        Console.WriteLine("\t2. Delete a model,");
        Console.WriteLine("\t3. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var selection);

        switch (selection)
        {
            case 0 or 1:
                var dto = new StorageDto(string.Empty, selection, 0);
                var result = await client.Post(dto);

                if (result.Response == 0)
                {
                    Console.WriteLine("Reload successful.");
                }
                else
                {
                    Console.WriteLine("Reload not successful.");
                }

                return;
            case 2:
                Console.Write("Enter the name of the model to delete: ");
                var model = Console.ReadLine() ?? string.Empty;
                var dto2 = new StorageDto(model, selection, 0);
                var result2 = await client.Post(dto2);

                // Refresh after deletion.
                var dto3 = new StorageDto(string.Empty, 0, 0);
                var result3 = await client.Post(dto3);

                if (result2.Response == 0 && result3.Response == 0)
                {
                    Console.WriteLine("Deletion successful.");
                }
                else
                {
                    Console.WriteLine("Deletion not successful.");
                }

                return;
            case 3:
                return;
            default:
                Console.WriteLine("Invalid selection, returning to main menu.");

                return;
        }
    }
}
