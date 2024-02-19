using Bogus;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Models;

namespace BuildingLinkDriver.Services
{
    public class OperationService : IOperationService
    {
        private readonly ILogger<OperationService> _logger;
        private readonly IRepository _repository;

        public OperationService(IRepository repository, ILogger<OperationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a list of random drivers using the Bogus library.
        /// </summary>
        /// <param name="count">The number of drivers to create.</param>
        /// <returns>A list of random drivers.</returns>
        public void CreateRandomDrivers(int count)
        {
            try
            {
                _logger.LogInformation("Creating {count} random drivers...", count);

                List<Driver> drivers = new();

                for (int i = 0; i < count; i++)
                {
                    Faker faker = new();
                    drivers.Add(new Driver
                    {
                        FirstName = faker.Person.FirstName,
                        LastName = faker.Person.LastName,
                        Email = faker.Person.Email,
                        PhoneNumber = faker.Phone.PhoneNumberFormat()
                    });
                }

                _repository.BulkInsert(drivers);

                _logger.LogInformation("Created {driverCount} random drivers.", drivers.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating {count} random drivers.", count);
                throw;
            }
        }

        /// <summary>
        /// Sorts the driver's name alphabetically.
        /// </summary>
        /// <param name="driver">The driver that will have it's name sorted.</param>
        /// <returns>A string of the driver's sorted name.</returns>
        public string Alphabetize(Driver driver)
        {
            try
            {
                _logger.LogInformation("Alphabetizing driver: {driver}...", $"{driver.FirstName} {driver.LastName}");

                string alphabetizedFirstName = new(driver.FirstName
                    .ToCharArray()
                    .OrderBy(c => c.ToString(), StringComparer.OrdinalIgnoreCase)
                    .ToArray());

                string alphabetizedLastName = new(driver.LastName
                    .ToCharArray()
                    .OrderBy(c => c.ToString(), StringComparer.OrdinalIgnoreCase)
                    .ToArray());

                _logger.LogInformation("Alphabetized driver: {driver} to {alphabetizedName}.",
                    $"{driver.FirstName} {driver.LastName}",
                    $"{alphabetizedFirstName} {alphabetizedLastName}");

                return $"{alphabetizedFirstName} {alphabetizedLastName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error alphabetizing name for driver: {driver}.", $"{driver.FirstName} {driver.LastName}");
                throw;
            }
        }

        /// <summary>
        /// Sort all the drivers' names.
        /// </summary>
        /// <returns>A list of all the drivers' sorted name.</returns>
        public List<string> Alphabetize()
        {
            try
            {
                List<string> alphabetizedNames = new();

                foreach (Driver driver in _repository.Get())
                {
                    alphabetizedNames.Add(Alphabetize(driver));
                }

                return alphabetizedNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error alphabetizing names for all drivers.");
                throw;
            }
        }
    }
}
