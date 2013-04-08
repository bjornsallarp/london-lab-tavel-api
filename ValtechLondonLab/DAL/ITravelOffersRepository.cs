namespace ValtechLondonLab.DAL
{
    using System;
    using System.Collections.Generic;

    using ValtechLondonLab.DAL.Entities;

    public interface ITravelOffersRepository
    {
        IEnumerable<Offer> GetOffers(string fromAirport);

        IEnumerable<Offer> GetOffers(string fromAirport, DateTime departureDate);

        IEnumerable<Offer> GetOffers(string fromAirport, string toDestination);

        IEnumerable<Offer> GetOffers(string fromAirport, string toDestination, DateTime departureDate);

        IEnumerable<string> GetAllDestinations();

        IEnumerable<string> GetDestinationsForAirport(string airport);

        Hotel GetHotel(string hotelId);
    }
}
