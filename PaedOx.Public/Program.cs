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
        var Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var apiSettings = Config
            .GetSection("Api")
            .Get<ApiSettings>() ?? new ApiSettings();

        Console.WriteLine("PaedOx");
        Console.WriteLine($"Selected Server: {apiSettings.BaseAddress}");
        Console.WriteLine("Server selection may be manually configured in 'appsettings.json'.");

        void ConfigureClient(HttpClient Client) => Client.BaseAddress = new Uri(apiSettings.BaseAddress);
        var Services = new ServiceCollection();
        Services.AddHttpClient<IAnalysis, Analysis>(ConfigureClient);
        Services.AddHttpClient<IDatasets, Datasets>(ConfigureClient);
        Services.AddHttpClient<IFeatureData, FeatureData>(ConfigureClient);
        Services.AddHttpClient<IModels, Models>(ConfigureClient);
        Services.AddHttpClient<IStorage, Storage>(ConfigureClient);
        var ServiceProvider = Services.BuildServiceProvider();

        var Running = true;

        while (Running)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("\t0. List datasets,");
            Console.WriteLine("\t1. List models,");
            Console.WriteLine("\t2. Manage storage,");
            Console.WriteLine("\t3. Generate training data,");
            Console.WriteLine("\t4. Generate predictions.");
            Console.Write("Select an option: ");
            _ = int.TryParse(Console.ReadLine(), out var Selection);

            switch (Selection)
            {
                case 0:
                    var DatasetClient = ServiceProvider.GetRequiredService<IDatasets>();
                    await ListDatasets.List(DatasetClient);

                    break;
                case 1:
                    var ModelClient = ServiceProvider.GetRequiredService<IModels>();
                    await ListModels.List(ModelClient);

                    break;
                case 2:
                    var StorageClient = ServiceProvider.GetRequiredService<IStorage>();
                    await ManageStorage.Manage(StorageClient);

                    break;
                case 3:
                    var FeatureGenerationClient = ServiceProvider.GetRequiredService<IFeatureData>();
                    await GenerateFeatureData.FromUnstructured(FeatureGenerationClient);

                    break;
                case 4:
                    var AnalysisClient = ServiceProvider.GetRequiredService<IAnalysis>();
                    await GeneratePredictions.FromUnstructured(AnalysisClient);

                    break;
                default:
                    Console.WriteLine("Invalid selection. Press any key to try again...");
                    Console.ReadKey();

                    break;
            }
        }
    }
}