using DriverBackendTask.Handlers;
using DriverBackendTask.Interfaces;
using DriverBackendTask.Models;
using Microsoft.Data.Sqlite;

namespace DriverBackendTask.Services
{
    /// <summary>
    /// Handles all driver operations
    /// </summary>
    public class DriverService : IDriver
    {

        private readonly string _connectionString;
        private readonly ILogger<DriverService> _logger;
        public DriverService(ILogger<DriverService> logger)
        {
            _logger = logger;
            
            if (ConfigurationHandler.AppSetting["ConnectionStrings:SQLite"] == null)
            {
                throw new Exception("No connection string found in the app settings");
            }
            _connectionString = ConfigurationHandler.AppSetting["ConnectionStrings:SQLite"];
            CreateDriverTable();

        }

        /// <summary>
        /// Create Driver table if not exists
        /// </summary>
        public void CreateDriverTable()
        {
            _logger.LogInformation("Create Driver Table");
            try
            {
                using (var connetion = new SqliteConnection(_connectionString))
                {
                    connetion.Open();
                    var command = connetion.CreateCommand();
                    command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Drivers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL
                )";
                    command.ExecuteNonQuery();
                }
                _logger.LogInformation("Driver table is created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating driver table: {ex.Message}");
                throw;

            }
        }

        /// <summary>
        /// Get all drivers from drivers table
        /// </summary>
        /// <returns>List<DriverModel></returns>
        public List<DriverModel> GetAllDrivers()
        {
            _logger.LogInformation("Start Getting All Drivers");
            List<DriverModel> allDrivers = null;
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"Select * from Drivers order by FirstName";
                    allDrivers = new List<DriverModel>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allDrivers.Add(new DriverModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString()
                            });
                        }
                    }

                }
                _logger.LogInformation("Getting All Drivers Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting all drivers: {ex.Message}");
                throw;
            }
            return allDrivers;
        }

        /// <summary>
        /// Get driver by id
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns>DriverModel</returns>
        public DriverModel GetDriverById(int driverId)
        {
            _logger.LogInformation("Start Getting Driver By");
            DriverModel driverModel = new DriverModel();
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"Select * from Drivers where id=@Id";
                    command.Parameters.AddWithValue("@Id", driverId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            driverModel.Id = Convert.ToInt32(reader["Id"]);
                            driverModel.FirstName = reader["FirstName"].ToString();
                            driverModel.LastName = reader["LastName"].ToString();
                            driverModel.Email = reader["Email"].ToString();
                            driverModel.PhoneNumber = reader["PhoneNumber"].ToString();

                        }
                    }

                }
                _logger.LogInformation("Getting Driver By Id Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting driver by Id: {ex.Message}");
                throw;
            }
            return driverModel;
        }

        /// <summary>
        /// Add new driver and returns the newly added id 
        /// </summary>
        /// <param name="driverModel"></param>
        /// <returns>int</returns>
        public int AddDriver(DriverModel driverModel)
        {
            _logger.LogInformation("Start Adding Driver");
            int addedDriverId = 0;
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber)
                VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
                    command.Parameters.AddWithValue("@FirstName", driverModel.FirstName);
                    command.Parameters.AddWithValue("@LastName", driverModel.LastName);
                    command.Parameters.AddWithValue("@Email", driverModel.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", driverModel.PhoneNumber);
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT last_insert_rowid()";
                     addedDriverId = Convert.ToInt32(command.ExecuteScalar());
                }
                _logger.LogInformation("Adding Driver Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error adding driver: {ex.Message}");
                throw;
            }
            return addedDriverId;
        }

        /// <summary>
        /// Update existing driver 
        /// </summary>
        /// <param name="driverModel"></param>
        public void UpdateDriver(DriverModel driverModel)
        {
            _logger.LogInformation("Start Updating Driver");
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText = @"UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, 
                    Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @Id";

                    command.Parameters.AddWithValue("@FirstName", driverModel.FirstName);
                    command.Parameters.AddWithValue("@LastName", driverModel.LastName);
                    command.Parameters.AddWithValue("@Email", driverModel.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", driverModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Id", driverModel.Id);
                    command.ExecuteNonQuery();

                }
                _logger.LogInformation("Updating Driver Finished");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating driver: {ex.Message}");
                throw;
            }

        }
        /// <summary>
        /// Deletes driver by id 
        /// </summary>
        /// <param name="driverId"></param>
        public void DeleteDriver(int driverId)
        {
            _logger.LogInformation("Start Deleting Driver");
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Drivers WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", driverId);
                    command.ExecuteNonQuery();
                }
                _logger.LogInformation("Deleting Driver Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error deleting driver: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Instert random driver names, the default is 10 if no drivers count is defined 
        /// </summary>
        /// <param name="driversCountToBeInserted"></param>
        /// <returns>List<DriverModel></DriverModel></returns>
        public List<DriverModel> InsertRandomDriverNames(int driversCountToBeInserted = 10)
        {
            _logger.LogInformation("Start Inserting Random Driver Names");
            List<DriverModel> randomDrivers = new List<DriverModel>();
            try
            {
                Random random = new Random();
                for (int i = 0; i < driversCountToBeInserted; i++)
                {
                    string firstName = RandomString(6);
                    string lastName = RandomString(6);
                    DriverModel driverModel = new DriverModel
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = $"{firstName}.{lastName}@driver.com",
                        PhoneNumber = $"888-{random.Next(1000, 9999)}"

                    };
                   
                    int addedDriverId =  AddDriver(driverModel);
                    driverModel.Id = addedDriverId;
                    randomDrivers.Add(driverModel);
                }
                _logger.LogInformation("Inserting Random Driver Names Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error inserting random driver names: {ex.Message}");
                throw;
            }
            return randomDrivers;
        }

        /// <summary>
        /// Get all drivers alphabetized
        /// </summary>
        /// <returns> List<AlphabetizedDriverModel></returns>
        public List<AlphabetizedDriverModel> GetAllDriversAlphabetized()
        {
            _logger.LogInformation("Start Get All Driver Names Alphabetized");
            List<AlphabetizedDriverModel> alphabetizedDriverModel = new List<AlphabetizedDriverModel>();
            try
            {
                List<DriverModel>  allDrivers = GetAllDrivers();
                if (allDrivers == null) {
                    throw new Exception();
                }

                foreach (DriverModel driver in allDrivers)
                {
                    alphabetizedDriverModel.Add(new AlphabetizedDriverModel
                    {
                        Id = driver.Id,
                        AlphabetizedFullName = $"{AlphabetizeName(driver.FirstName)} {AlphabetizeName(driver.LastName)}",
                        Email = driver.Email,
                        PhoneNumber = driver.PhoneNumber
                    });
                }
                _logger.LogInformation("Get All Driver Names Alphabetized Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting all driver names alphabetized : {ex.Message}");
                throw;
            }

            return alphabetizedDriverModel;
        }
        /// <summary>
        /// Get alphabetized driver name
        /// </summary>
        /// <param name="driverFullName"></param>
        /// <returns>string</returns>
        public string GetAlphabetizedDriverName(string driverFullName)
        {
            _logger.LogInformation("Alphabetizing Driver Name");
            string[] driverName = driverFullName.Split(" ");
            return $"{AlphabetizeName(driverName[0])} {AlphabetizeName(driverName[1])}";
        }

        /// <summary>
        /// Check if driver exists in drivers table
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns>bool</returns>
        public bool IsDriverExists(int driverId)
        {
            _logger.LogInformation("Start Is Driver Exists");
            int count =0;
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = "SELECT COUNT(1) FROM Drivers WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", driverId);

                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                _logger.LogInformation("Is Driver Exists Finished");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error is driver exists: {ex.Message}");
                throw;
            }
            return count > 0;
        }

        /// <summary>
        /// Get random strings
        /// </summary>
        /// <param name="length"></param>
        /// <returns>string</returns>
        private string RandomString(int length)
        {
            _logger.LogInformation("Getting Random Strings");
            Random random = new Random();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(letters, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Get alphabetize name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>string</returns>
        private string AlphabetizeName(string name)
        {
            _logger.LogInformation("Alphabetize Name");
            return new string(name.OrderBy(c => char.ToLower(c)).ToArray());
        }
        }
}
