namespace AzureFuncs.ForexBigDataProcessingFunctions.Functions.Activities;
public sealed class ForexBigDataProcessingReducerActivity
{
    [Function(nameof(Reducer))]
    public string Reducer([ActivityTrigger] List<ForexYearlyAverageExchangeRate> mapresults, ILogger log)
    {
        // Use var when the type is obvious from the right side of the assignment
        var results = mapresults.Where(x => x.RecordFiscalYear > 0).ToList();

        // Return the serialized string
        return JsonSerializer.Serialize(results);

    }
}
