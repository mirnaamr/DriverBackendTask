using DriverBackendTask.Models;
using Microsoft.Data.Sqlite;

namespace DriverBackendTask.Interfaces
{
    public interface IDriver
    {
        public void CreateDriverTable();
        public List<DriverModel> GetAllDrivers();
        public DriverModel GetDriverById(int driverId);
        public int AddDriver(DriverModel driverModel);
        public void UpdateDriver(DriverModel driverModel);
        public void DeleteDriver(int driverId);
        public List<DriverModel> InsertRandomDriverNames(int driversCountToBeInserted = 10);
        public List<AlphabetizedDriverModel> GetAllDriversAlphabetized();
        public string GetAlphabetizedDriverName(string driverName);
        public bool IsDriverExists(int driverId);


    }
}
