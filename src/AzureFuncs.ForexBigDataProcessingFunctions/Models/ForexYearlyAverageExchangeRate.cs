namespace AzureFuncs.ForexBigDataProcessingFunctions.Models;
public class ForexYearlyAverageExchangeRate
{
    public ForexYearlyAverageExchangeRate(int recordFiscalYear, double average) => (RecordFiscalYear, Average) = (recordFiscalYear, average);
    public ForexYearlyAverageExchangeRate()
    {

    }
    public int RecordFiscalYear { get; set; } = default!;
    public double Average { get; set; } = default!;
}
