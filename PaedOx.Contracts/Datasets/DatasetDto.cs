namespace PaedOx.Contracts.Datasets;

public sealed record DatasetDto(
    List<List<double>> DataX,
    List<double> DataYClassification,
    List<double> DataYRegression
);
