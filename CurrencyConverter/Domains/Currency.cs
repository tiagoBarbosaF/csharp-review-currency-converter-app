using System.Text.Json.Serialization;

namespace CurrencyConverter.Domains;

public record Currency(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("codein")] string CodeIn,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("high")] string HighValue,
    [property: JsonPropertyName("low")] string LowValue,
    [property: JsonPropertyName("varBid")] string VarBid,
    [property: JsonPropertyName("bid")] string Bid,
    [property: JsonPropertyName("ask")] string Ask,
    [property: JsonPropertyName("timestamp")]
    string Timestamp,
    [property: JsonPropertyName("create_date")]
    string CreatedDate
);