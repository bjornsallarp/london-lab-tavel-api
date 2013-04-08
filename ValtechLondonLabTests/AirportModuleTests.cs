namespace ValtechLondonLabTests
{
    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Nancy;
    using Nancy.Testing;

    using Newtonsoft.Json;

    using ValtechLondonLab.Modules;

    using ValtechLondonLabTests.Helpers;

    [TestClass]
    public class AirportModuleTests
    {
        [TestMethod]
        public void ShouldReturnAvailableAirports()
        {
            // Arrange
            var bootstrapper = new TestBootstrapper(with => with.Module<AirportsModule>());

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/airports",
                with =>
                    {
                        with.HttpRequest();
                        with.Accept("application/json");
                    });


            // Asser
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonObject = new[] { new { Code = string.Empty, Name = string.Empty } };

            jsonObject = JsonConvert.DeserializeAnonymousType(result.Body.AsString(), jsonObject);
            jsonObject.Should().Contain(arg => arg.Name == "Borlänge" && arg.Code == "Borlänge");
        }
    }
}
