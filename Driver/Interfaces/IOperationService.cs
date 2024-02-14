using BuildingLinkDriver.Models;

namespace BuildingLinkDriver.Interfaces
{
    public interface IOperationService
    {
        void CreateRandomDrivers(int count);
        string Alphabetize(Driver driver);
        List<string> Alphabetize();
    }
}
