using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaedOx.Public;
using PaedOx.Public.Clients;
using PaedOx.Public.Functions;
using PaedOx.Public.Interfaces;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var apiSettings = config.GetSection("Api").Get<ApiSettings>() ?? new ApiSettings();

        Console.WriteLine("PaedOx");
        Console.WriteLine($"Selected Server: {apiSettings.BaseAddress}");
        Console.WriteLine("Server selection may be manually configured in 'appsettings.json'.");

        void ConfigureClient(HttpClient client) =>
            client.BaseAddress = new Uri(apiSettings.BaseAddress);
        var services = new ServiceCollection();
        services.AddHttpClient<IAnalysis, Analysis>(ConfigureClient);
        services.AddHttpClient<IDatasets, Datasets>(ConfigureClient);
        services.AddHttpClient<IFeatureData, FeatureData>(ConfigureClient);
        services.AddHttpClient<IModels, Models>(ConfigureClient);
        services.AddHttpClient<IStorage, Storage>(ConfigureClient);
        var serviceProvider = services.BuildServiceProvider();

        var running = true;

        while (running)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("\t0. List datasets,");
            Console.WriteLine("\t1. List models,");
            Console.WriteLine("\t2. Manage storage,");
            Console.WriteLine("\t3. Generate training data,");
            Console.WriteLine("\t4. Generate predictions.");
            Console.Write("Select an option: ");
            _ = int.TryParse(Console.ReadLine(), out var selection);

            switch (selection)
            {
                case 0:
                    var datasetClient = serviceProvider.GetRequiredService<IDatasets>();
                    await ListDatasets.List(datasetClient);

                    break;
                case 1:
                    var modelClient = serviceProvider.GetRequiredService<IModels>();
                    await ListModels.List(modelClient);

                    break;
                case 2:
                    var storageClient = serviceProvider.GetRequiredService<IStorage>();
                    await ManageStorage.Manage(storageClient);

                    break;
                case 3:
                    var featureGenerationClient =
                        serviceProvider.GetRequiredService<IFeatureData>();
                    await GenerateFeatureData.FromUnstructured(featureGenerationClient);

                    break;
                case 4:
                    var analysisClient = serviceProvider.GetRequiredService<IAnalysis>();
                    await GeneratePredictions.FromUnstructured(analysisClient);

                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again...");
                    Console.ReadKey();

                    break;
            }
        }
    }
}
