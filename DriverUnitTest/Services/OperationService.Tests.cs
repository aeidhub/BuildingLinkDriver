using Bogus;
using BuildingLinkDriver.Interfaces;
using BuildingLinkDriver.Models;
using BuildingLinkDriver.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace DriverUnitTest.Services
{
    [TestClass]
    public class OperationServiceTests
    {
        private Mock<ILogger<OperationService>> _loggerMock;
        private Mock<IRepository> _repositoryMock;
        private IOperationService _operationService;

        [TestInitialize]
        public void Initialize()
        {
            _loggerMock = new Mock<ILogger<OperationService>>();
            _repositoryMock = new Mock<IRepository>();
            _operationService = new OperationService(_repositoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void Alphabetize_ReturnsAlphabetizeDriverNameCorrectly()
        {
            // Arrange
            var driver = new Driver { FirstName = "John", LastName = "Doe" };

            // Act
            var result = _operationService.Alphabetize(driver);

            // Assert
            Assert.AreEqual("hJno Deo", result); // Alphabetized "John Doe" -> "hJno Deo"
        }

        [TestMethod]
        public void Alphabetize_ShouldReturnAlphabetizedNamesForAllDrivers()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver { FirstName = "John", LastName = "Doe" },
                new Driver { FirstName = "Jane", LastName = "Mickel" }
            };
            _repositoryMock.Setup(r => r.Get()).Returns(drivers);

            // Expected alphabetized names for "hJno Deo" and "aeJn ceiklM"
            var expected = new List<string> { "hJno Deo", "aeJn ceiklM" };

            // Act
            var result = _operationService.Alphabetize();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
