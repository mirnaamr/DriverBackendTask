DriverBackendTask

Overview
DriverBackendTask is a simple CRUD API built using ASP.NET Core and SQLite as the database. 

The application allows users to Create, Read, Update, and Delete (CRUD) driver records and includes additional functionality such as:

✅ Swagger UI for API testing.
✅ Unit testing using xUnit and Moq.
✅ Logging with ILogger.
✅ Dependency Injection for better maintainability.
✅ Input validation and exception handling.
✅ Basic Authentication Handler.
✅ Configuration Handler.

Features:
✅ Add a new driver
✅ Get all drivers (sorted alphabetically by first name)
✅ Retrieve a driver by ID
✅ Update an existing driver
✅ Delete an existing driver
✅ Generate 10 random drivers
✅ Get all drivers alphabetized 
✅ Return given driver name alphabetized 
✅ Unit tests using xUnit and Moq

Technologies Used
✅ ASP.NET Core 8.0 (For App Implementation)
✅ SQLite (For Database)
✅ xUnit & Moq (for Unit Testing)
✅ Swagger (API Documentation)
✅ ILogger (Logging)

Installation & Setup
Prerequisites
.NET SDK 8.0+
Visual Studio 2022+
SQLite (Embedded in .NET)

Step 1: Clone the Repository
git clone https://github.com/mirnaamr/DriverBackendTask.git
cd DriverBackendTask
Step 2: Build the Application
dotnet build
Step 3: Run the Application
dotnet run
The API will start on https://localhost:7199

Step 4: Access Swagger UI
Open a browser and go to:
http://localhost:7199/swagger/index.html
This will allow you to test API endpoints interactively.

API Endpoints
Get All Drivers (Sorted Alphabetically by First Name)
GET /api/Driver/GetAllDrivers

Add a New Driver
POST /api/Driver/AddDriver
{
  "firstName": "Oliver",
  "lastName": "Johnson",
  "email": ""oliver.Johnson@driver.com",
  "phoneNumber": "123-456"
}

Get a Driver by ID
GET /api/Driver/GetDriver/{id}

Update a Driver
PUT /api/Driver/UpdateDriver/{id}

{
"firstName": "Oliver",
  "lastName": "Johnson",
  "email": ""oliver.Johnson@driver.com",
  "phoneNumber": "123-456889"
}
Delete a Driver
DELETE /api/Driver/DeleteDriver/{id}

Insert Random Drivers
POST /api/Driver/InsertRandomDriverNames

Get All Drivers Alphabetized
GET /api/Driver/GetAllDriversAlphabetized

Get Alphabetize Driver's Full Name
GET /api/Driver/GetAlphabetizedDriverNamee/{driverFullName}

Database Setup (SQLite)
The application uses SQLite as the database. The database file is created in the project file.

To manually open and inspect the database, you can use:

sqlite3 DriverDatabase.db
Then inside the SQLite shell:
SELECT * FROM Drivers;

Running Unit Tests
Prerequisites
Ensure xUnit and Moq are installed in the test project.

Run Tests in Visual Studio
Open Test Explorer (Test → Test Explorer).
Click Run All Tests.
Check the results for pass/fail status.

Run Tests via CLI
dotnet test
Logging & Error Handling
ILogger is used for logging errors and tracking application flow.

Exceptions are caught and logged to ensure the application does not crash unexpectedly.

Future Improvements
✅ Add authentication & authorization using JWT tokens instead of using Basic Authentication.
✅ Implement pagination for driver listing
✅ Improve error handling & logging
