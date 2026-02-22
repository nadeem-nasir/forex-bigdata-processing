namespace AzureFuncs.ForexBigDataProcessingFunctions.Functions.Activities;
public sealed class ForexBigDataProcessingMapperActivity
{
    /// <summary>
    /// // A function that returns a ForexYearlyAverageExchangeRate object from a ForexExchangeRateModel object
    /// </summary>
    /// <param name="paras"></param>
    /// <param name="executionContext"></param>
    /// <returns></returns>
    [Function(nameof(YearlyAverageExchangeRateMapperAsync))]
    public ForexYearlyAverageExchangeRate YearlyAverageExchangeRateMapperAsync([ActivityTrigger] ForexExchangeRateModel paras, FunctionContext executionContext)
    {
        // Validate the parameter
        ArgumentNullException.ThrowIfNull(paras, nameof(paras));
        // Group the forex exchange rate rows by fiscal year and calculate the average exchange rate for each year
        //Best practice is to use the AsParallel() method before the part of the query that you want to execute in parallel, and avoid using it after the part of the query that you want to execute sequentially
        var yearlyAverages = paras
                                .ForexExchangeRateRows
                                .AsParallel()
                                .GroupBy(x => x.RecordFiscalYear)
                                .Select(g => new ForexYearlyAverageExchangeRate(g.Key, g.Average(y => y.ExchangeRate)));


        // Return the first element of the yearly averages or a default value if empty
        return yearlyAverages.FirstOrDefault() ?? new ForexYearlyAverageExchangeRate();
    }
}
