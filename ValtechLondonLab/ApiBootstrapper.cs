namespace ValtechLondonLab
{
    using System.IO;

    using Nancy;
    using Nancy.Bootstrapper;

    using ServiceStack.Text;

    using ValtechLondonLab.DAL;

    public class ApiBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig.IncludeNullValues = false;

            var pathProvider = container.Resolve<IRootPathProvider>();
            var travelRepoJsonPath = Path.Combine(pathProvider.GetRootPath(), "Resources", "offers.json");

            var travelRepo = new TravelOffersRepository(travelRepoJsonPath);

            container.Register<ITravelOffersRepository>(travelRepo);
        }
    }
}