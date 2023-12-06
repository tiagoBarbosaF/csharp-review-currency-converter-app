using System.Text.RegularExpressions;
using CurrencyConverter.Services;

namespace CurrencyConverter.Main;

public static class Main
{
    public static async void Start()
    {
        while (true)
        {
            Menu();
            Console.Write("\nEnter the option: ");
            var option = Console.ReadLine()!;

            if (option.Equals("0"))
                break;

            switch (option)
            {
                case "1":
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
                    
                    break;
                case "2":
                    var combinations = await ExchangeApi.GetAllCurrencyCombinations();

                    Console.WriteLine();
                    foreach (var combination in combinations) Console.WriteLine(combination);
                    Console.WriteLine();

                    break;
                case "3":
                    Console.Write("Enter the currency for filter: ");
                    var currencyFilterOption = Console.ReadLine();
                    patternCombination = @"^[a-zA-Z]{3}$";

                    if (currencyFilterOption != null && Regex.IsMatch(currencyFilterOption, patternCombination))
                    {
                        var filterCombinationByCurrency =
                            await ExchangeApi.GetFilterCombinationByCurrency(currencyFilterOption.ToUpper());
                        Console.WriteLine();
                        foreach (var filter in filterCombinationByCurrency) Console.WriteLine(filter);
                        Console.WriteLine();
                    }

                    break;
                default:
                    Console.WriteLine("Invalid option...");
                    break;
            }
        }
    }

    private static void Menu()
    {
        var menuBar = new string('*', 50);
        Console.WriteLine($"\n{menuBar}\n" +
                          $"== Exchange Converter ==\n" +
                          $"    1 - Converter\n" +
                          $"    2 - List all combinations currencies\n" +
                          $"    3 - List combinations by currency\n" +
                          $"    0 - Exit\n" +
                          $"{menuBar}");
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