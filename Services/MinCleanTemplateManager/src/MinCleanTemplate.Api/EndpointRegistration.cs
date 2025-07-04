

namespace MinCleanTemplateManager.Api;

public static class EndpointRegistration
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapSampleModelEndpoint();

      //  app.MapTransmitterModelsEndpoints();
    }
}





