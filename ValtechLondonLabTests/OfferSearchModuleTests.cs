namespace ValtechLondonLabTests
{
    using System;

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
    public class OfferSearchModuleTests
    {
        [TestMethod]
        public void ShouldReturnOffersForAirport()
        {
            // Arrange
            var offer = new Offer
                            {
                                City = "Stockholm",
                                BookingUrl = "http://fake.com",
                                Date = DateTime.Now,
                                Days = 7,
                                Departure = "Borlänge",
                                Destination = "Sverige",
                                HotelId = "10",
                                Price = 2000,
                                Remaining = 2,
                                RoomDesc = "Enkelrum"
                            };

            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(repository => repository.GetOffers(It.Is<string>(s => s == "Borlänge")))
                     .Returns(new[] { offer });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferSearchModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offers/from/Borlänge",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("text/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var offers = result.Body.DeserializeJson<Offer[]>();

            offers.Length.Should().Be(1);

            var firstOffer = offers[0];
            firstOffer.City.Should().Be(offer.City);
            firstOffer.BookingUrl.Should().Be(offer.BookingUrl);
            firstOffer.Date.Should().Be(offer.Date);
            firstOffer.Days.Should().Be(offer.Days);
            firstOffer.Departure.Should().Be(offer.Departure);
            firstOffer.Destination.Should().Be(offer.Destination);
            firstOffer.HotelId.Should().Be(offer.HotelId);
            firstOffer.Price.Should().Be(offer.Price);
            firstOffer.Remaining.Should().Be(offer.Remaining);
            firstOffer.RoomDesc.Should().Be(offer.RoomDesc);
        }

        [TestMethod]
        public void ShouldReturnOffersForAirportWithADate()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(
                repository => repository.GetOffers(It.IsAny<string>(), It.IsAny<DateTime>()))
                     .Returns(new[] { new Offer(), new Offer() });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferSearchModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offers/from/Borlänge/on/2013-01-01",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("text/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var offers = result.Body.DeserializeJson<Offer[]>();

            offers.Length.Should().Be(2);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenDateIsInvalid()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(
                repository => repository.GetOffers(It.IsAny<string>(), It.IsAny<DateTime>()))
                     .Returns(new[] { new Offer(), new Offer() });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferSearchModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offers/from/Borlänge/on/2013-01-01asd",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("text/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Body.AsString().Should().Be("Departure date format is invalid");
        }

        [TestMethod]
        public void ShouldReturnOffersForAirporToDestinationt()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(
                repository => repository.GetOffers(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(new[] { new Offer(), new Offer() });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferSearchModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offers/from/Borlänge/to/New York/",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("text/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var offers = result.Body.DeserializeJson<Offer[]>();

            offers.Length.Should().Be(2);
        }

        [TestMethod]
        public void ShouldReturnOffersForAirporToDestinationtWithADate()
        {
            // Arrange
            var offerRepo = new Mock<ITravelOffersRepository>();
            offerRepo.Setup(
                repository => repository.GetOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                     .Returns(new[] { new Offer(), new Offer() });

            var bootstrapper = new TestBootstrapper(with =>
            {
                with.Module<OfferSearchModule>();
                with.Dependency<ITravelOffersRepository>(offerRepo.Object);
            });

            var browser = new Browser(bootstrapper);

            // Act
            var result = browser.Get(
                "/offers/from/Borlänge/to/New York/on/2013-01-01",
                with =>
                {
                    with.HttpRequest();
                    with.Accept("text/json");
                });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var offers = result.Body.DeserializeJson<Offer[]>();

            offers.Length.Should().Be(2);
        }
    }
}
