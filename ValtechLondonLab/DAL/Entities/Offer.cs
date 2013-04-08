namespace ValtechLondonLab.DAL.Entities
{
    using System;

    public class Offer
    {
        public string BookingUrl { get; set; }

        public string City { get; set; }

        public DateTimeOffset Date { get; set; }

        public int Days { get; set; }

        public string Departure { get; set; }

        public string Destination { get; set; }

        public string HotelId { get; set; }

        public decimal Price { get; set; }

        public int Remaining { get; set; }

        public string RoomDesc { get; set; }
    }
}