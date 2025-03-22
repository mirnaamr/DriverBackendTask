using System.Collections.Generic;
using System.Linq;
using DriverBackendTask.Controllers;
using DriverBackendTask.Interfaces;
using DriverBackendTask.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DriverTest.Tests.Controllers
{
    public class DriverControllerTests
    {
        private readonly Mock<IDriver> _mockDriver;
        private readonly Mock<ILogger<DriverController>> _mockLogger;
        private readonly DriverController _controller;

        public DriverControllerTests()
        {
            _mockDriver = new Mock<IDriver>();
            _mockLogger = new Mock<ILogger<DriverController>>();
            _controller = new DriverController(_mockDriver.Object, _mockLogger.Object);
            DefaultHttpContext httpContext = new DefaultHttpContext();
            ControllerContext controllerContext = new ControllerContext() { HttpContext = httpContext };
            _controller.ControllerContext = controllerContext;
        }

        [Fact]
        public void AddDriver_ReturnsCreatedResult_WhenDriverIsValid()
        {
            // Arrange
            DriverModel driverModel = new DriverModel
            {
                FirstName = "Oliver",
                LastName = "Johnson",
                Email = "oliver.Johnson@driver.com",
                PhoneNumber = "555677"
            };

            _mockDriver.Setup(x => x.AddDriver(It.IsAny<DriverModel>())).Returns(1);

            // Act
            IActionResult result = _controller.AddDriver(driverModel);

            // Assert
            CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            _mockDriver.Verify(x => x.AddDriver(It.IsAny<DriverModel>()), Times.Once);
        }

        [Fact]
        public void GetAllDrivers_ReturnsOkResult_WhenDriversExist()
        {
            // Arrange
            List<DriverModel> drivers = new List<DriverModel>
            {
            new DriverModel { Id = 1, FirstName = "Oliver", LastName = "Johnson" },
            new DriverModel { Id = 2, FirstName = "Peter", LastName = "John" }
            };

            _mockDriver.Setup(x => x.GetAllDrivers()).Returns(drivers);

            // Act
            IActionResult result = _controller.GetAllDrivers();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<DriverModel> returnValue = Assert.IsType<List<DriverModel>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void DeleteDriver_ReturnsNoContentResult_WhenDriverIsDeleted()
        {
            // Arrange
            int driverId = 1;

            _mockDriver.Setup(x => x.IsDriverExists(driverId)).Returns(true);
            _mockDriver.Setup(x => x.DeleteDriver(driverId)).Verifiable();

            // Act
            IActionResult result = _controller.DeleteDriver(driverId);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void GetDriver_ReturnsNotFound_WhenDriverDoesNotExist()
        {
            // Arrange
            _mockDriver.Setup(x => x.GetDriverById(9999)).Returns((DriverModel)null);

            // Act
            IActionResult result = _controller.GetDriver(9999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateDriver_ReturnsOk_WhenDriverIsUpdated()
        {
            // Arrange
            DriverModel existingDriver = new DriverModel { Id = 1, FirstName = "Oliver", LastName = "Johnson" };
            _mockDriver.Setup(x => x.GetDriverById(1)).Returns(existingDriver);
            _mockDriver.Setup(x => x.IsDriverExists(existingDriver.Id)).Returns(true);

            // Act
            IActionResult result = _controller.UpdateDriver(existingDriver);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllDriversAlphabetized_ReturnsAlphabetizedList()
        {

            // Arrange
            List<DriverModel> drivers = new List<DriverModel>
            {
                new DriverModel { Id = 1, FirstName = "Oliver", LastName = "Johnson", Email = "oliver.Johnson@driver.com", PhoneNumber = "123-456" },
                new DriverModel { Id = 2, FirstName = "Peter", LastName = "John", Email = "peter.john@driver.com", PhoneNumber = "789-101" }
            };

            List<AlphabetizedDriverModel> alphabetizedDrivers = drivers.Select(d => new AlphabetizedDriverModel
            {
                Id = d.Id,
                AlphabetizedFullName = $"{new string(d.FirstName.OrderBy(c => c).ToArray())} {new string(d.LastName.OrderBy(c => c).ToArray())}",
                Email = d.Email,
                PhoneNumber = d.PhoneNumber
            }).ToList();

            _mockDriver.Setup(x => x.GetAllDriversAlphabetized()).Returns(alphabetizedDrivers);

            // Act
            IActionResult result = _controller.GetAllDriversAlphabetized();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<AlphabetizedDriverModel> returnedDrivers = Assert.IsType<List<AlphabetizedDriverModel>>(okResult.Value);
            Assert.Equal(alphabetizedDrivers.Count, returnedDrivers.Count);
            Assert.Equal(alphabetizedDrivers[0].AlphabetizedFullName, returnedDrivers[0].AlphabetizedFullName);
        }

    }
}
