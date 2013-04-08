namespace ValtechLondonLabTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Nancy;
    using Nancy.Testing;

    using ValtechLondonLab.DAL;
    using ValtechLondonLab.Modules;

    using ValtechLondonLabTests.Helpers;

    [TestClass]
    public class DestionationModuleTests
    {
        [TestMethod]
        public void ShouldReturnAllDestinations()
        {
            // Arrange
            var destinations = new List<string> { "New York", "Miami", "Tokyo" };

            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetAllDestinations()).Returns(destinations);

            var bootstrapper = new TestBootstrapper(with =>
                { 
                    with.Module<DestinationsModule>();
                    with.Dependency<ITravelOffersRepository>(offerRepo.Object);
                });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/destinations",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResult = result.Body.DeserializeJson<List<string>>();

            jsonResult.Should().BeEquivalentTo(destinations);
        }

        [TestMethod]
        public void ShouldReturnDestinationsForStockholm()
        {
            // Arrange
            var destinations = new List<string> { "New York", "Tokyo" };

            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetDestinationsForAirport(It.Is<string>(s => s == "Borlänge")))
                     .Returns(destinations);

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<DestinationsModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/destinations/Borlänge",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResult = result.Body.DeserializeJson<List<string>>();

            jsonResult.Should().BeEquivalentTo(destinations);
        }
    }
}
