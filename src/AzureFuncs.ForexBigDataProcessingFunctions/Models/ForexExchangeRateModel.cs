namespace AzureFuncs.ForexBigDataProcessingFunctions.Models;
public class ForexExchangeRateModel
{
    public ForexExchangeRateModel()
    {
        ForexExchangeRateRows = [];
    }

    [JsonPropertyName("data")]
    public List<ForexExchangeRateRowModel> ForexExchangeRateRows { get; set; }
}
