using System.Globalization;
using System.Net;
using System.Text.Json;
using ExchangeConverter.Domains;

namespace ExchangeConverter.Services;

public class ExchangeApi
{
    private static readonly HttpClient HttpClient = new HttpClient();

    private static async Task<string?> Converter(string coins)
    {
        var coinsList = new List<string>();
        var coinsListSplit = coins.Split(",");


        var response = HttpClient.GetAsync($"https://economia.awesomeapi.com.br/json/last/{coins}");
        if (response.Result.StatusCode == HttpStatusCode.OK)
        {
            var readerAsync = await response.Result.Content.ReadAsStringAsync();
            return readerAsync;
        }

        return null;
    }

    public static async Task<List<string>> GetAllCurrencyCombinations()
    {
        var currencyCombinationsList = new List<string>();
        var response = HttpClient.GetAsync("https://economia.awesomeapi.com.br/json/available");

        if (response.Result.StatusCode == HttpStatusCode.OK)
        {
            var readAsStringAsync = await response.Result.Content.ReadAsStringAsync();

            var currencyCombinationsMap =
                JsonSerializer.Deserialize<Dictionary<string, string>>(readAsStringAsync);
            if (currencyCombinationsMap != null)
                currencyCombinationsList.AddRange(
                    currencyCombinationsMap.OrderBy(combination => combination.Value)
                        .Select(combination => $"{combination.Key}: {combination.Value}"));
        }

        return currencyCombinationsList;
    }

    public static async Task<List<string>> GetFilterCombinationByCurrency(string currency)
    {
        var combinationsList = new List<string>();
        var response = HttpClient.GetAsync("https://economia.awesomeapi.com.br/json/available");

        if (response.Result.StatusCode == HttpStatusCode.OK)
        {
            var readAsync = await response.Result.Content.ReadAsStringAsync();

            var combinationsMap = JsonSerializer.Deserialize<Dictionary<string, string>>(readAsync);
            var filter = combinationsMap.Where(item => item.Key.StartsWith(currency, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            combinationsList.AddRange(filter.Select(item => $"{item.Key}: {item.Value}"));
        }

        return combinationsList;
    }

    public static async Task<List<Currency>> GetCurrencies(string currencies)
    {
        var converter = await Converter(currencies);
        var listCurrencies = new List<Currency>();
        var specificCulture = CultureInfo.CreateSpecificCulture("pt-BR");

        if (converter != null)
        {
            var currenciesDictionary = JsonSerializer.Deserialize<Dictionary<string, Currency>>(converter);
            if (currenciesDictionary != null)
                listCurrencies.AddRange(from currency in currenciesDictionary
                    let valueCode = currency.Value.Code
                    let valueCodeIn = currency.Value.CodeIn
                    let valueName = currency.Value.Name
                    let valueHighValue = decimal.Parse(currency.Value.HighValue).ToString("C", specificCulture)
                    let valueLowValue = decimal.Parse(currency.Value.LowValue).ToString("C", specificCulture)
                    let valueVarBid = decimal.Parse(currency.Value.VarBid).ToString("C", specificCulture)
                    let valueBid = decimal.Parse(currency.Value.Bid).ToString("C", specificCulture)
                    let valueAsk = decimal.Parse(currency.Value.Ask).ToString("C", specificCulture)
                    let valueTimestamp = currency.Value.Timestamp
                    let valueCreatedDate = currency.Value.CreatedDate
                    select new Currency(valueCode, valueCodeIn, valueName, valueHighValue, valueLowValue, valueVarBid,
                        valueBid, valueAsk, valueTimestamp, valueCreatedDate));
        }

        return listCurrencies;
    }
}