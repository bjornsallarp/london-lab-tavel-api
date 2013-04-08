namespace ValtechLondonLab.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Nancy;
    using Nancy.Helpers;

    using ValtechLondonLab.DAL;
    using ValtechLondonLab.DAL.Entities;

    public class OfferSearchModule : NancyModule
    {
        public OfferSearchModule(ITravelOffersRepository offersRepository)
            : base("/offers")
        {
            this.Get["/from/{airport}"] = parameters =>
            {
                var airport = HttpUtility.UrlDecode(parameters.airport);
                IEnumerable<Offer> offers = offersRepository.GetOffers(airport);

                return offers.OrderBy(offer => offer.Date);
            };

            this.Get["/from/{airport}/on/{departuredate}"] = parameters =>
            {
                var airport = HttpUtility.UrlDecode(parameters.airport);
                DateTime departureDate;

                if (!DateTime.TryParse(HttpUtility.UrlDecode(parameters.departuredate), out departureDate))
                {
                    return ErrorResponse(HttpStatusCode.BadRequest, "Departure date format is invalid");
                }

                IEnumerable<Offer> offers = offersRepository.GetOffers(airport, departureDate);

                return offers.OrderBy(offer => offer.Date);
            };

            this.Get["/from/{airport}/to/{destination}"] = parameters =>
                {
                    var airport = HttpUtility.UrlDecode(parameters.airport);
                    var destination = HttpUtility.UrlDecode(parameters.destination);

                    IEnumerable<Offer> offers = offersRepository.GetOffers(airport, destination);
                    return offers.OrderBy(offer => offer.Date);
                };

            this.Get["/from/{airport}/to/{destination}/on/{departuredate}"] = parameters =>
                {
                    var airport = HttpUtility.UrlDecode(parameters.airport);
                    var destination = HttpUtility.UrlDecode(parameters.destination);
                    DateTime departure;
                    
                    if (!DateTime.TryParse(HttpUtility.UrlDecode(parameters.departuredate), out departure))
                    {
                        return ErrorResponse(HttpStatusCode.BadRequest, "Departure date format is invalid");
                    }

                    IEnumerable<Offer> offers = offersRepository.GetOffers(airport, destination, departure);
                    return offers.OrderBy(offer => offer.Date);
                };
        }

        private static Response ErrorResponse(HttpStatusCode statusCode, string message)
        {
            return new Response
            {
                StatusCode = statusCode,
                ContentType = "text/plain",
                Contents = stream => (new StreamWriter(stream) { AutoFlush = true }).Write(message)
            };
        }
    }
}