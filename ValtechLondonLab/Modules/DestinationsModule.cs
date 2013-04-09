namespace ValtechLondonLab.Modules
{
    using System.Collections.Generic;
    using System.Linq;

    using Nancy;
    using Nancy.Helpers;

    using ValtechLondonLab.DAL;

    public class DestinationsModule : NancyModule
    {
        public DestinationsModule(ITravelOffersRepository offersRepository)
        {
            this.Get["/destinations"] = _ => offersRepository.GetAllDestinations().OrderBy(s => s);

            this.Get["/destinations/{airport}"] = parameters =>
                {
                    var airport = HttpUtility.UrlDecode(parameters.airport);
                    IEnumerable<string> destinations = offersRepository.GetDestinationsForAirport(airport);

                    return destinations.OrderBy(s => s);
                };
        }
    }
}