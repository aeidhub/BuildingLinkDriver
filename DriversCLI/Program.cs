using CommandLine;
using DriversCLI;
using Microsoft.Extensions.Configuration;
using static DriversCLI.CommandsOptions;

Console.WriteLine("Available commands:");
Console.WriteLine("\tget [-i <id>]:\nRetrieve drivers data. Include ID for a specific driver.\n");
Console.WriteLine("\tadd:\nAdd a fake driver.\n");
Console.WriteLine("\tupdate -id <id> -f <first name> -l <last name> -e <email> -p <phone number>:\nUpdate an existing driver.\n");
Console.WriteLine("\tdelete -i <id>:\nDelete a driver by ID.\n");
Console.WriteLine("\tfake -c <count>:\nAdds fake drivers. Default value for count is 10.\n");
Console.WriteLine("\talphabetized -c <count>:\nRetrieve alphabetized drivers' names. Include ID for a specific driver.\n");
Console.WriteLine("\texit:\nExit the program.\n");
Console.WriteLine("\thelp:\nShow available commands.");

bool exit = false;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var apiBaseUrl = configuration["ApiBaseUrl"];

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    Console.WriteLine("ApiBaseUrl is not configured in appsettings.json.");
    return;
}

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
};

while (!exit)
{
    Console.Write("\nEnter a command: ");
    string? userInput = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(userInput))
        continue;
    else if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
        exit = true;

    string[] userArgs = userInput.Split(' ');
    var result = await Parser.Default.ParseArguments<GetOptions, AddOptions, UpdateOptions, DeleteOptions, AddFakeOptions, GetAlphabetizeOptions>(userArgs)
        .WithParsedAsync(async (object obj) =>
        {
            _ = obj switch
            {
                GetOptions => await Handlers.HandleGetCommand(httpClient, (GetOptions)obj),
                AddOptions => await Handlers.HandleAddCommand(httpClient, (AddOptions)obj),
                UpdateOptions => await Handlers.HandleUpdateCommand(httpClient, (UpdateOptions)obj),
                DeleteOptions => await Handlers.HandleDeleteCommand(httpClient, (DeleteOptions)obj),
                AddFakeOptions => await Handlers.HandleAddFakeCommand(httpClient, (AddFakeOptions)obj),
                GetAlphabetizeOptions => await Handlers.HandleGetAlphabetizedCommand(httpClient, (GetAlphabetizeOptions)obj),
                _ => Handlers.HandleUnknownCommand(userInput)
            };
        });
}