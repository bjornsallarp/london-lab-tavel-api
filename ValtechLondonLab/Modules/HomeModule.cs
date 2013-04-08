namespace ValtechLondonLab.Modules
{
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            this.Get["/"] = parameters => "Welcome to the travel API!";
        }
    }
}