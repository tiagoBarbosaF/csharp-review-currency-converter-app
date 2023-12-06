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
                    string patternCombination;
                    await ConverterMenuOptions.Converter();

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
}