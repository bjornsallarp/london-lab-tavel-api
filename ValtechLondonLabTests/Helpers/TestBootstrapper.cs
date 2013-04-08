namespace ValtechLondonLabTests.Helpers
{
    using System;

    using Nancy;
    using Nancy.Serializers.Json.ServiceStack;
    using Nancy.Testing;

    using ServiceStack.Text;

    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration)
            : base(configuration)
        {
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig.IncludeNullValues = false;

            container.Register<ISerializer>(new ServiceStackJsonSerializer());
        }
    }
}
