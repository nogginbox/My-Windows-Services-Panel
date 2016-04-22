using System;
using System.Linq;
using WindowsServicePanel.Sevices;
using Xunit;

namespace WindowsServicePanel.Tests.Services
{
    public class WindowsServicesServiceTests
    {
        [Fact]
        public void GetAllServicesReturnsServices()
        {
            // Arrange
            var serviceFixture = new WindowsServicesService();

            // Act
            var services = serviceFixture.GetAllServices().ToList();

            // Assert
            Assert.NotEmpty(services);
            Assert.NotNull(services[0].Name);
            Assert.NotNull(services[0].DisplayName);
            Assert.NotNull(services[0].Description);
        }

        [Fact]
        public void GetServiceByNameReturnsService()
        {
            // Arrange
            const String serviceName = "wuauserv"; // Windows Update better be there
            var serviceFixture = new WindowsServicesService();

            // Act
            var service = serviceFixture.GetServiceByName(serviceName);

            // Assert
            Assert.NotNull(service);
            Assert.Equal(serviceName, service?.Name);
            Assert.NotNull(service?.DisplayName);
            Assert.NotNull(service?.Description);
        }
    }
}
