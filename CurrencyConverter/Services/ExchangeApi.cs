using System.Globalization;
using System.Net;
using System.Text.Json;
using CurrencyConverter.Domains;

namespace CurrencyConverter.Services;

public class ExchangeApi
{
    private static readonly HttpClient HttpClient = new();

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
            if (combinationsMap != null)
            {
                var filter = combinationsMap
                    .Where(item => item.Key.StartsWith(currency, StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                combinationsList.AddRange(filter.Select(item => $"{item.Key}: {item.Value}"));
            }
        }

        return combinationsList;
    }

    public static async Task<string?> GetCurrencies(string currencies)
    {
        var listCurrencies = new List<Currency>();
        var resultList = new List<string>();
        var currencySplit = currencies.Split(",");
        var specificCulture = CultureInfo.CreateSpecificCulture("pt-BR");

        foreach (var currency in currencySplit)
        {
            var response = HttpClient.GetAsync($"https://economia.awesomeapi.com.br/json/{currency}/1");
            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                var responseApi = await response.Result.Content.ReadAsStringAsync();
                var test = JsonSerializer.Deserialize<List<Currency>>(responseApi);
                
                listCurrencies.AddRange(from currencyResponse in test
                    let valueCode = currencyResponse.Code
                    let valueCodeIn = currencyResponse.CodeIn
                    let valueName = currencyResponse.Name
                    let valueHighValue = decimal.Parse(currencyResponse.HighValue).ToString("C", specificCulture)
                    let valueLowValue = decimal.Parse(currencyResponse.LowValue).ToString("C", specificCulture)
                    let valueVarBid = decimal.Parse(currencyResponse.VarBid).ToString("C", specificCulture)
                    let valueBid = decimal.Parse(currencyResponse.Bid).ToString("C", specificCulture)
                    let valueAsk = decimal.Parse(currencyResponse.Ask).ToString("C", specificCulture)
                    let valueTimestamp = currencyResponse.Timestamp
                    let valueCreatedDate = currencyResponse.CreatedDate
                    select new Currency(valueCode, valueCodeIn, valueName, valueHighValue, valueLowValue, valueVarBid,
                        valueBid, valueAsk, valueTimestamp, valueCreatedDate)
                );
            }
            else
            {
                resultList.Add($"Currency not found: {currency}\n");
            }
        }

        resultList.AddRange(
            listCurrencies.Select(result => $"Convert: {result.Code.Replace("R$ ", "")} " +
                                            $"to {result.CodeIn.Replace("R$ ", "")}: " +
                                            $"{result.Ask.Replace("R$ ", "")}\n"));
        return string.Join("", resultList);
    }
}