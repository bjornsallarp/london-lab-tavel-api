namespace ValtechLondonLab.DAL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using ValtechLondonLab.DAL.Entities;

    public class TravelOffersRepository : ITravelOffersRepository
    {
        public TravelOffersRepository(string dataFilePath)
            : this(File.OpenRead(dataFilePath))
        {
        }

        public TravelOffersRepository(Stream stream)
        {
            var jsonObject = new { Hotels = new List<Hotel>(), Offers = new List<Offer>() };

            using (var reader = new StreamReader(stream))
            {
                var data = JsonConvert.DeserializeAnonymousType(reader.ReadToEnd(), jsonObject);

                this.Offers = data.Offers;
                this.Hotels = data.Hotels;
            }
        }

        private List<Offer> Offers { get; set; }

        private List<Hotel> Hotels { get; set; }

        public IEnumerable<Offer> GetOffers(string fromAirport)
        {
            var lowerAirport = fromAirport.ToLower();

            return this.Offers.Where(offer => offer.Departure.ToLower() == lowerAirport);
        }

        public IEnumerable<Offer> GetOffers(string fromAirport, DateTime departureDate)
        {
            return this.GetOffers(fromAirport).Where(offer => offer.Date >= departureDate);
        }

        public IEnumerable<Offer> GetOffers(string fromAirport, string toDestination)
        {
            var lowerDestination = toDestination.ToLower();

            return this.GetOffers(fromAirport).Where(offer => offer.Destination.ToLower() == lowerDestination);
        }

        public IEnumerable<Offer> GetOffers(string fromAirport, string toDestination, DateTime departureDate)
        {
            return this.GetOffers(fromAirport, toDestination).Where(offer => offer.Date >= departureDate);
        }

        public IEnumerable<string> GetAllDestinations()
        {
            return this.Offers.Select(offer => offer.Destination)
                              .Distinct();
        }

        public IEnumerable<string> GetDestinationsForAirport(string airport)
        {
            var lowerAirport = airport.ToLower();
            
            return
                this.Offers.Where(offer => offer.Departure.ToLower() == lowerAirport)
                    .Select(offer => offer.Destination)
                    .Distinct();
        }

        public Hotel GetHotel(string hotelId)
        {
            return this.Hotels.FirstOrDefault(hotel => hotel.Id == hotelId);
        }
    }
}