using Bogus;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Models;
using BuildingLinkDriver.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace YourProject.Tests
{
    [TestClass]
    public class DriverServiceTests
    {
        private Mock<ILogger<DriverService>> loggerMock;
        private Mock<IDriverRepository> driversRepositoryMock;
        private DriverService driversService;

        [TestInitialize]
        public void Initialize()
        {
            loggerMock = new Mock<ILogger<DriverService>>();
            driversRepositoryMock = new Mock<IDriverRepository>();
            driversService = new DriverService(driversRepositoryMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public void Get_ReturnsListOfDrivers()
        {
            // Arrange
            List<Driver> drivers = new();
            for (int i = 0; i < 5; i++)
            {
                Faker faker = new();
                drivers.Add(new Driver { FirstName = faker.Person.FirstName, LastName = faker.Person.LastName, Email = faker.Person.Email, PhoneNumber = faker.Phone.PhoneNumberFormat() });
            }

            driversRepositoryMock.Setup(repo => repo.Get()).Returns(drivers);

            // Act
            var result = driversService.Get();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(drivers, result);
        }

        [TestMethod]
        public void Get_ReturnsEmptyListOfDrivers()
        {
            // Arrange
            List<Driver> drivers = new();

            driversRepositoryMock.Setup(repo => repo.Get()).Returns(drivers);

            // Act
            var result = driversService.Get();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(drivers, result);
        }

        [TestMethod]
        public void Get_WithValidId_ReturnsDriver()
        {
            Faker faker = new();
            // Arrange
            var sampleDriver = new Driver // Create a sample driver here
            {
                Id = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Phone.PhoneNumberFormat()
            };

            driversRepositoryMock.Setup(repo => repo.Get(sampleDriver.Id)).Returns(sampleDriver);

            // Act
            var result = driversService.Get(sampleDriver.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(sampleDriver.Id, result.Id);
        }

        [TestMethod]
        public void Get_WithInvalidId_ReturnsNull()
        {
            // Arrange
            const int invalidDriverId = -1; // Use an invalid driver ID

            driversRepositoryMock.Setup(repo => repo.Get(invalidDriverId)).Returns((Driver)null);

            // Act
            var result = driversService.Get(invalidDriverId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Add_ValidDriver_ReturnsAffectedRows()
        {
            // Arrange
            Faker faker = new();
            Driver newDriver = new()
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Phone.PhoneNumberFormat()
            };

            driversRepositoryMock.Setup(repo => repo.Add(newDriver)).Returns(1);

            // Act
            var result = driversService.Add(newDriver);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Add_InvalidDriver_ReturnsZeroAffectedRows()
        {
            // Arrange
            Driver invalidDriver = new();

            driversRepositoryMock.Setup(repo => repo.Add(invalidDriver)).Returns(0);

            // Act
            var result = driversService.Add(invalidDriver);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Update_ValidDriver_ReturnsAffectedRows()
        {
            Faker faker = new();
            // Arrange
            var existingDriver = new Driver // Create a sample driver here
            {
                Id = 1,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Email = faker.Person.Email,
                PhoneNumber = faker.Phone.PhoneNumberFormat()
            };

            driversRepositoryMock.Setup(repo => repo.Update(existingDriver)).Returns(1); // Assuming 1 row affected

            // Act
            var result = driversService.Update(existingDriver);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Update_InvalidDriver_ReturnsZeroAffectedRows()
        {
            // Arrange
            Driver invalidDriver = new();

            driversRepositoryMock.Setup(repo => repo.Update(invalidDriver)).Returns(0);

            // Act
            var result = driversService.Update(invalidDriver);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Delete_WithValidId_ReturnsAffectedRows()
        {
            // Arrange
            const int driverIdToDelete = 1;

            driversRepositoryMock.Setup(repo => repo.Delete(driverIdToDelete)).Returns(1); // Assuming 1 row affected

            // Act
            var result = driversService.Delete(driverIdToDelete);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Delete_WithInvalidId_ReturnsZeroAffectedRows()
        {
            // Arrange
            const int invalidDriverId = -1;

            driversRepositoryMock.Setup(repo => repo.Delete(invalidDriverId)).Returns(0);

            // Act
            var result = driversService.Delete(invalidDriverId);

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}
