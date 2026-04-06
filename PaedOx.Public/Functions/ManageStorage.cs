using PaedOx.Contracts.Storage;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Functions;

internal static class ManageStorage
{
    public static async Task Manage(IStorage Client)
    {
        Console.WriteLine("\nSelect from the following storage operations:");
        Console.WriteLine("\t0. Reload saved datasets,");
        Console.WriteLine("\t1. Reload saved models,");
        Console.WriteLine("\t2. Delete a model,");
        Console.WriteLine("\t3. Return to main menu.");
        _ = int.TryParse(Console.ReadLine(), out var Selection);

        switch (Selection)
        {
            case 0 or 1:
                var Dto = new StorageDto(string.Empty, Selection, 0);
                var Result = await Client.Post(Dto);

                if (Result.Response == 0)
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
                var Model = Console.ReadLine() ?? string.Empty;
                var Dto2 = new StorageDto(Model, Selection, 0);
                var Result2 = await Client.Post(Dto2);

                // Refresh after deletion.
                var Dto3 = new StorageDto(string.Empty, 0, 0);
                var Result3 = await Client.Post(Dto3);

                if (Result2.Response == 0 && Result3.Response == 0)
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
