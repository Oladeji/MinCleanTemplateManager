

namespace MinCleanTemplateManager.Api;

public static class EndpointRegistration
{
    public static void RegisterMinCleanTemplateManagerEndpoints(this WebApplication app)
    {
        app.MapSampleModelEndpoint();

      //  app.MapTransmitterModelsEndpoints();
    }
}





