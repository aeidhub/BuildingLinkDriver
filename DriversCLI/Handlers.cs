using Bogus;
using BuildingLinkDriver.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using static DriversCLI.CommandsOptions;

namespace DriversCLI
{
    public static class Handlers
    {
        public static async Task<int> HandleGetCommand(HttpClient httpClient, GetOptions options)
        {
            if (options.Id.HasValue)
            {
                Console.WriteLine($"Retrieving driver with ID {options.Id}...");
                var response = await httpClient.GetAsync($"/api/Drivers/{options.Id}");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Failed to retrieve driver with ID {options.Id}. Status code: {response.StatusCode}");

                string jsonContent = await response.Content.ReadAsStringAsync();

                JObject jsonObject = JObject.Parse(jsonContent);

                string prettifiedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                Console.WriteLine(prettifiedJson);
            }
            else
            {
                Console.WriteLine("Retrieving all drivers...");
                var response = await httpClient.GetAsync("/api/Drivers");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Failed to retrieve drivers. Status code: {response.StatusCode}");

                var content = await response.Content.ReadAsStringAsync();

                var drivers = JsonConvert.DeserializeObject<List<Driver>>(content);

                string prettifiedJson = JsonConvert.SerializeObject(drivers, Formatting.Indented);

                Console.WriteLine(prettifiedJson);
            }
            return 0;
        }

        public static async Task<int> HandleAddCommand(HttpClient httpClient, AddOptions _)
        {
            Faker faker = new();
            Driver newDriver = new()
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Phone.PhoneNumberFormat()
            };

            string jsonBody = JsonConvert.SerializeObject(newDriver);

            Console.WriteLine("Adding a new driver...");
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage? response = await httpClient.PostAsync("/api/Drivers", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("New driver has been added successfully.");
            else
                Console.WriteLine($"Failed to add a new driver. Status code: {response.StatusCode}");

            return 0;
        }

        public static async Task<int> HandleUpdateCommand(HttpClient httpClient, UpdateOptions options)
        {
            Driver newDriver = new()
            {
                Id = options.Id,
                FirstName = options.FirstName,
                LastName = options.LastName,
                Email = options.Email,
                PhoneNumber = options.PhoneNumber
            };

            string jsonBody = JsonConvert.SerializeObject(newDriver);

            Console.WriteLine($"Updating driver with ID {options.Id}...");
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage? response = await httpClient.PatchAsync("/api/Drivers", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine($"Updating driver with ID {options.Id} has been added successfully.");
            else
                Console.WriteLine($"Failed to update driver with ID {options.Id}. Status code: {response.StatusCode}");

            return 0;
        }

        public static async Task<int> HandleDeleteCommand(HttpClient httpClient, DeleteOptions options)
        {
            Console.WriteLine($"Deleting driver with ID {options.Id}...");
            var response = await httpClient.DeleteAsync($"/api/Drivers/{options.Id}");
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Failed to delete driver with ID {options.Id}. Status code: {response.StatusCode}");
            else
                Console.WriteLine($"Driver with ID {options.Id} has been deleted.");

            return 0;
        }

        public static async Task<int> HandleAddFakeCommand(HttpClient httpClient, AddFakeOptions options)
        {
            Console.WriteLine($"Adding {options.Count} fake driver/s...");
            var response = await httpClient.PostAsync($"/api/Fake/GenerateDrivers/{options.Count}", null);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"Failed to add {options.Count} fake driver/s. Status code: {response.StatusCode}");
            else
                Console.WriteLine($"{options.Count} fake driver/s have been added.");
            return 0;
        }

        public static async Task<int> HandleGetAlphabetizedCommand(HttpClient httpClient, GetAlphabetizeOptions options)
        {
            if (options.Id.HasValue)
            {
                Console.WriteLine($"Retrieving alphabetized driver with ID {options.Id}...");
                var response = await httpClient.GetAsync($"/api/Fake/GetAlphabetized/{options.Id}");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Failed to retrieve alphabetized driver with ID {options.Id}. Status code: {response.StatusCode}");

                string alphabetizedDriverName = await response.Content.ReadAsStringAsync();

                Console.WriteLine(alphabetizedDriverName);
            }
            else
            {
                Console.WriteLine("Retrieving all drivers...");
                var response = await httpClient.GetAsync("/api/Fake/GetAlphabetized");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Failed to retrieve alphabetized drivers. Status code: {response.StatusCode}");

                var content = await response.Content.ReadAsStringAsync();

                var alphabetizedDriversNames = JsonConvert.DeserializeObject<List<string>>(content);

                string prettifiedJson = JsonConvert.SerializeObject(alphabetizedDriversNames, Formatting.Indented);

                Console.WriteLine(prettifiedJson);
            }
            return 0;
        }

        public static int HandleUnknownCommand(string userInput)
        {
            if (!string.Equals(userInput, "help", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"Unknown command: {userInput}");
            return 1;
        }
    }
}