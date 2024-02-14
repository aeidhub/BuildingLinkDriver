using Bogus;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Models;
using Microsoft.Extensions.Logging;

namespace BuildingLinkDriver.Services
{
    public class DriverService : IDriverService
    {
        private readonly ILogger<DriverService> _logger;
        private readonly IDriverRepository _driversRepository;

        public DriverService(IDriverRepository driversRepository, ILogger<DriverService> logger)
        {
            _driversRepository = driversRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all drivers.
        /// </summary>
        /// <returns>A list of drivers.</returns>
        public List<Driver> Get()
        {
            try
            {
                _logger.LogInformation("Getting all drivers...");

                var drivers = _driversRepository.Get();

                _logger.LogInformation("Got {DriverCount} drivers.", drivers.Count);

                return drivers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all drivers.");
                throw;
            }
        }

        /// <summary>
        /// Gets a driver by id.
        /// </summary>
        /// <param name="id">The id of the driver to get.</param>
        /// <returns>A driver or null if the driver does not exist.</returns>
        public Driver? Get(int id)
        {
            try
            {
                _logger.LogInformation("Getting driver with id {DriverId}...", id);

                var driver = _driversRepository.Get(id);

                if (driver is null)
                    _logger.LogWarning("Driver with id {DriverId} does not exist.", id);
                else
                    _logger.LogInformation("Got driver with id {DriverId}.", id);

                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting driver with id {DriverId}.", id);
                throw;
            }
        }

        /// <summary>
        /// Adds a new driver.
        /// </summary>
        /// <param name="driver">The driver to add.</param>
        /// <returns>The number of rows affected by the insert.</returns>
        public int Add(Driver driver)
        {
            try
            {
                _logger.LogInformation("Adding driver...");

                int affectedRows = _driversRepository.Add(driver);

                if (affectedRows > 0)
                    _logger.LogInformation("Added driver successfully.");
                else
                    _logger.LogWarning("Driver could not be added due to no affected rows.");

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding driver.");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing driver.
        /// </summary>
        /// <param name="driver">The driver to update.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public int Update(Driver driver)
        {
            try
            {
                _logger.LogInformation("Updating driver with id {DriverId}...", driver.Id);

                int affectedRows = _driversRepository.Update(driver);

                if (affectedRows > 0)
                    _logger.LogInformation("Updated driver with id {DriverId} successfully.", driver.Id);
                else
                    _logger.LogWarning("Driver with id {DriverId} could not be updated as no rows were affected.", driver.Id);

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating driver with id {DriverId}.", driver.Id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a driver by id.
        /// </summary>
        /// <param name="id">The id of the driver to delete.</param>
        /// <returns>The number of rows affected by the delete.</returns>
        public int Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting driver with id {DriverId}...", id);

                int affectedRows = _driversRepository.Delete(id);

                if (affectedRows > 0)
                    _logger.LogInformation("Deleted driver with id {DriverId} successfully.", id);
                else
                    _logger.LogWarning("Driver with id {DriverId} could not be deleted as no rows were affected.", id);

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting driver with id {DriverId}.", id);
                throw;
            }
        }
    }
}
