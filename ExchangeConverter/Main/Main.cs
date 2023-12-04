using System.Text.RegularExpressions;
using ExchangeConverter.Services;

namespace ExchangeConverter.Main;

public class Main
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
                    var patternCombination = @"^[A-Z]{3}-[A-Z]{3}$";

                    while (true)
                    {
                        MenuCombinations();
                        Console.Write($"\nEnter the option: ");
                        var optionCombinations = Console.ReadLine();
                        
                        if (optionCombinations.Equals("0"))
                            break;
                        
                        Console.Write("\nEnter the current combination for converter: ");
                        var combinationOption = Console.ReadLine()!;

                        if (Regex.IsMatch(combinationOption, patternCombination))
                        {
                            combinationList.Add(combinationOption);
                        }
                        else
                        {
                            Console.WriteLine("Invalid combination, enter the right pattern (AAA-BBB).");
                        }
                    }

                    var currencyCombinations = string.Join(",", combinationList);

                    var currencies = await ExchangeApi.GetCurrencies(currencyCombinations);
                    Console.WriteLine();
                    foreach (var currency in currencies)
                    {
                        Console.WriteLine($"Convert: {currency.Code} to {currency.CodeIn}: {currency.Ask}");
                    }

                    Console.WriteLine();
                    break;
                case "2":
                    var combinations = await ExchangeApi.GetAllCurrencyCombinations();
                    
                    foreach (var combination in combinations)
                    {
                        Console.WriteLine(combination);
                    }
                    break;
                case "3":
                    Console.Write("Enter the currency for filter: ");
                    var currenctFilterOption = Console.ReadLine();
                    patternCombination = @"^[A-Z]{3}$";

                    if (Regex.IsMatch(currenctFilterOption,patternCombination))
                    {
                        var filterCombinationByCurrency = await ExchangeApi.GetFilterCombinationByCurrency(currenctFilterOption);

                        foreach (var filter in filterCombinationByCurrency)
                        {
                            Console.WriteLine(filter);
                        }
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
        Console.WriteLine($"{menuBar}\n" +
                          $"== Exchange Converter ==\n" +
                          $"    1 - Converter\n" +
                          $"    2 - List all combinations currencies\n" +
                          $"    3 - List combinations by currency\n" +
                          $"    0 - Exit\n" +
                          $"{menuBar}");
    }

    private static void MenuCombinations()
    {
        Console.WriteLine($"\n1 - Enter combinations for converter\n" +
                          $"0 - Exit");
    }
}