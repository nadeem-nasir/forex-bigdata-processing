namespace AzureFuncs.ForexBigDataProcessingFunctions.Models;
public class ForexExchangeRateRowModel
{

    [JsonPropertyName("record_date")]
    public DateTime RecordDate { get; set; } = default!;


    [JsonPropertyName("country")]
    public string Country { get; set; } = default!;


    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;


    [JsonPropertyName("country_currency_desc")]
    public string CountryCurrencyDesc { get; set; } = default!;


    [JsonPropertyName("exchange_rate")]
    public double ExchangeRate { get; set; } = default!;


    [JsonPropertyName("effective_date")]
    public DateTime EffectiveDate { get; set; } = default!;


    [JsonPropertyName("src_line_nbr")]
    public int SrcLineNbr { get; set; } = default!;


    [JsonPropertyName("record_fiscal_year")]
    public int RecordFiscalYear { get; set; } = default!;


    [JsonPropertyName("record_fiscal_quarter")]
    public int RecordCalendarYear { get; set; } = default!;


    [JsonPropertyName("record_calendar_quarter")]
    public int RecordCalendarQuarter { get; set; } = default!;


    [JsonPropertyName("record_calendar_month")]
    public int RecordCalendarMonth { get; set; } = default!;


    [JsonPropertyName("record_calendar_day")]
    public int RecordCalendarDay { get; set; } = default!;
}
