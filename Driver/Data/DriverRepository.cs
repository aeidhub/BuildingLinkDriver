using BuildingLinkDriver.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Services;

namespace BuildingLinkDriver.Data
{
    public class DriverRepository: IDriverRepository
    {
        private readonly SQLiteConnection _connection;

        public DriverRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Gets a list of all drivers.
        /// </summary>
        /// <returns>A list of drivers.</returns>
        public List<Driver> Get()
        {
            List<Driver> drivers = new();
            try
            {
                using var connection = new SQLiteConnection(_connection);
                connection.Open();

                using var command = new SQLiteCommand("SELECT * FROM drivers", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    drivers.Add(new Driver
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.GetString(4)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while getting the list of drivers. Details: {ex}");
            }

            return drivers;
        }

        /// <summary>
        /// Gets a driver by id.
        /// </summary>
        /// <param name="id">The id of the driver to get.</param>
        /// <returns>A driver or null if the driver does not exist.</returns>
        public Driver? Get(int id)
        {
            Driver? driver = null;
            try
            {
                using var connection = new SQLiteConnection(_connection);
                connection.Open();

                using var command = new SQLiteCommand("SELECT * FROM drivers WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    driver = new Driver
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.GetString(4)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while getting the driver. Details: {ex}");
            }

            return driver;
        }

        /// <summary>
        /// Adds a new driver to the database.
        /// </summary>
        /// <param name="driver">The driver to add.</param>
        ///<returns>The number of rows affected by the insert.</returns>
        public int Add(Driver driver)
        {
            try
            {
                using var connection = new SQLiteConnection(_connection);
                connection.Open();

                using var command = new SQLiteCommand("INSERT INTO drivers (firstName, lastName, email, phoneNumber) VALUES (@firstName, @lastName, @email, @phoneNumber)", connection);
                command.Parameters.AddWithValue("@firstName", driver.FirstName);
                command.Parameters.AddWithValue("@lastName", driver.LastName);
                command.Parameters.AddWithValue("@email", driver.Email);
                command.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while adding a new driver. Details: {ex}");
            }
        }

        /// <summary>
        /// Updates an existing driver in the database.
        /// </summary>
        /// <param name="driver">The driver to update.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public int Update(Driver driver)
        {
            try
            {
                using var connection = new SQLiteConnection(_connection);
                connection.Open();

                using var command = new SQLiteCommand("UPDATE drivers SET firstName = @firstName, lastName = @lastName, email = @email, phoneNumber = @phoneNumber WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", driver.Id);
                command.Parameters.AddWithValue("@firstName", driver.FirstName);
                command.Parameters.AddWithValue("@lastName", driver.LastName);
                command.Parameters.AddWithValue("@email", driver.Email);
                command.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while updating the driver. Details: {ex}");
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
                using var connection = new SQLiteConnection(_connection);
                connection.Open();

                using var command = new SQLiteCommand("DELETE FROM drivers WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while deleting the driver. Details: {ex}");
            }
        }

        /// <summary>
        /// Bulk inserts drivers into the database.
        /// </summary>
        /// <param name="drivers">The drivers to insert.</param>
        public void BulkInsert(IEnumerable<Driver> drivers)
        {
            try
            {
                using var connection = new SQLiteConnection(_connection);
                connection.Open();
                using var transaction = connection.BeginTransaction();

                using var command = new SQLiteCommand("INSERT INTO drivers (firstName, lastName, email, phoneNumber) VALUES (@firstName, @lastName, @email, @phoneNumber)", connection);
                foreach (Driver driver in drivers)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@firstName", driver.FirstName);
                    command.Parameters.AddWithValue("@lastName", driver.LastName);
                    command.Parameters.AddWithValue("@email", driver.Email);
                    command.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error while adding the list of drivers. Details: {ex}");
            }
        }
    }
}
