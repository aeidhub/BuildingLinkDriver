using BuildingLinkDriver.Models;

namespace BuildingLinkDriver.Interfaces
{
    public interface IDriverService
    {
        List<Driver> Get();
        Driver? Get(int id);
        int Add(Driver driver);
        int Update(Driver driver);
        int Delete(int id);
    }
}
