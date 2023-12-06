using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using CurrencyConverter.Domains;
using CurrencyConverter.Main;

Main.Start();

// var httpClient = new HttpClient();
// var currencies = "USD-BRL,EUR-BRL,AAA-AAA";
// var currencySplit = currencies.Split(",");
//
//
// var responseList = new List<string>();
// var listCurrencies = new List<Currency>();
// foreach (var currency in currencySplit)
// {
//     var response = httpClient.GetAsync($"https://economia.awesomeapi.com.br/json/{currency}/1");
//     if (response.Result.StatusCode == HttpStatusCode.OK)
//     {
//         var responseApi = await response.Result.Content.ReadAsStringAsync();
//         var test = JsonSerializer.Deserialize<List<Currency>>(responseApi);
//         var specificCulture = CultureInfo.CreateSpecificCulture("pt-BR");
//         listCurrencies.AddRange(from currencyResponse in test
//             let valueCode = currencyResponse.Code
//             let valueCodeIn = currencyResponse.CodeIn
//             let valueName = currencyResponse.Name
//             let valueHighValue = decimal.Parse(currencyResponse.HighValue).ToString("C", specificCulture)
//             let valueLowValue = decimal.Parse(currencyResponse.LowValue).ToString("C", specificCulture)
//             let valueVarBid = decimal.Parse(currencyResponse.VarBid).ToString("C", specificCulture)
//             let valueBid = decimal.Parse(currencyResponse.Bid).ToString("C", specificCulture)
//             let valueAsk = decimal.Parse(currencyResponse.Ask).ToString("C", specificCulture)
//             let valueTimestamp = currencyResponse.Timestamp
//             let valueCreatedDate = currencyResponse.CreatedDate
//             select new Currency(valueCode, valueCodeIn, valueName, valueHighValue, valueLowValue, valueVarBid,
//                 valueBid, valueAsk, valueTimestamp, valueCreatedDate)
//         );
//     }
//     else
//     {
//         Console.WriteLine($"Currency not found: {currency}");
//         break;
//     }
// }
//
// responseList.AddRange(
//     listCurrencies.Select(result => $"Convert: {result.Code.Replace("R$ ", "")} " +
//                                     $"to {result.CodeIn.Replace("R$ ", "")}: " +
//                                     $"{result.Ask.Replace("R$ ", "")}\n")
// );
//
// var join = string.Join("",responseList);
// Console.WriteLine(join);