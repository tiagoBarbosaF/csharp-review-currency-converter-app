using System.Text.RegularExpressions;

namespace CurrencyConverter.Services;

public class ConverterMenuOptions
{
    public static async Task Converter()
    {
        var combinationList = new List<string>();
        var patternCombination = @"^[a-zA-Z]{3}-[a-zA-Z]{3}$";

        while (true)
        {
            MenuCombinations();
            Console.Write($"\nEnter the option: ");
            var optionCombinations = Console.ReadLine();

            if (optionCombinations is "0")
                break;

            switch (optionCombinations)
            {
                case "1":
                    Console.Write("\nEnter the currency combination for conversion: ");
                    var combinationOption = Console.ReadLine()!;

                    if (Regex.IsMatch(combinationOption, patternCombination))
                        combinationList.Add(combinationOption.ToUpper());
                    else
                        Console.WriteLine("Invalid combination, enter the right pattern (AAA-BBB).");
                    break;
                default:
                    Console.WriteLine("Invalid option...");
                    break;
            }
        }

        var currencyCombinations = string.Join(",", combinationList);
        var currencies = await ExchangeApi.GetCurrencies(currencyCombinations);
        Console.WriteLine($"\n{currencies}");
    }
    
    private static void MenuCombinations()
    {
        var menuBar = new string('*', 50);
        Console.WriteLine($"\n{menuBar}\n" +
                          $"    1 - Enter combinations for converter\n" +
                          $"    0 - Exit\n" +
                          $"{menuBar}");
    }
}