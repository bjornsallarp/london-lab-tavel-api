namespace ValtechLondonLabTests
{
    using System.Net.Http;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Nancy;
    using Nancy.Testing;

    using ValtechLondonLab.DAL;
    using ValtechLondonLab.DAL.Entities;
    using ValtechLondonLab.Modules;

    using ValtechLondonLabTests.Helpers;

    [TestClass]
    public class OfferHotelModuleTests
    {
        [TestMethod]
        public void ShouldReturnHotel()
        {
            // Arrange
            var hotel = new Hotel
                            {
                                Name = "Taj Mahal",
                                ImageUrl = "http://www.fake.com/image.jpg",
                                Grade = 4.5m,
                                Html = "<blink>AWESOME</blink>",
                                Id = "1",
                                Url = "http://fake.com"
                            };

            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetHotel(It.Is<string>(s => s == "10")))
                     .Returns(hotel);

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferHotelModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offer/hotel/10",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var jsonObject = result.Body.DeserializeJson<Hotel>();

            jsonObject.Name.Should().Be(hotel.Name);
            jsonObject.ImageUrl.Should().Be(hotel.ImageUrl);
            jsonObject.Grade.Should().Be(hotel.Grade);
            jsonObject.Html.Should().Be(hotel.Html);
            jsonObject.Id.Should().Be(hotel.Id);
            jsonObject.Url.Should().Be(hotel.Url);
        }

        [TestMethod]
        public void ShouldReturnNotFoundForInvalidId()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetHotel(It.Is<string>(s => s == "100")))
                     .Returns((Hotel)null);

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferHotelModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offer/hotel/100",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ShouldReturnNotFoundWhenImageSourceIsNotFound()
        {
            // Arrange
            var hotel = new Hotel { ImageUrl = "http://www.fake.com/image.jpg" };

            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetHotel(It.IsAny<string>()))
                     .Returns(hotel);

            var response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound };
            var httpClient = new HttpClient(new FakeHandler { Response = response });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferHotelModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
                with.Dependency<HttpClient>(httpClient);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/hotel/1/image/width/100/height/100",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ShouldReturnNotFoundWhenHotelDoesNotExist()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetHotel(It.IsAny<string>()))
                     .Returns(default(Hotel));

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferHotelModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/hotel/1/image/width/100/height/100",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("application/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
