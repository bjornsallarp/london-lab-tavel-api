namespace ValtechLondonLab.Modules
{
    using Nancy;

    public class AirportsModule : NancyModule
    {
        public AirportsModule()
        {
            var airports = new[]
                               {
                                   new { Code = "Borlänge", Name = "Borlänge" }, 
                                   new { Code = "Göteborg", Name = "Göteborg/Landvetter" },
                                   new { Code = "Jönköping", Name = "Jönköping" },
                                   new { Code = "Kalmar", Name = "Kalmar" },
                                   new { Code = "Karlstad", Name = "Karlstad" },
                                   new { Code = "Köpenhamn", Name = "Köpenhamn" },
                                   new { Code = "Luleå", Name = "Luleå" },
                                   new { Code = "Malmö Airport", Name = "Malmö" },
                                   new { Code = "Norrköping", Name = "Norrköping" },
                                   new { Code = "Stockholm - Arlanda", Name = "Stockholm/Arlanda" },
                                   new { Code = "Sundsvall", Name = "Sundsvall" },
                                   new { Code = "Umeå", Name = "Umeå" },
                                   new { Code = "Växjö", Name = "Växjö" },
                                   new { Code = "Örebro", Name = "Örebro" }
                               };

            this.Get["/airports"] = parameters => airports;
        }
    }
}