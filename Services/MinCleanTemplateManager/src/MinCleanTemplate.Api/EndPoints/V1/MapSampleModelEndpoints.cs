using CQRSHelper;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Provides endpoints for managing model versions.
/// Includes operations for retrieving, creating, updating, and deleting model versions.
/// </summary>
/// <remarks>
/// ..
/// </remarks>
public static class MapSampleModelEndpoints
{
    /// <summary>
    /// Maps the model version endpoints to the route builder.
    /// </summary>
    /// <param name="routes">The route builder.</param>
    public static void MapSampleModelEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("", Get)
           .Produces(StatusCodes.Status200OK)
            .WithName(""+ "GetAllModelVersions");

    }

    /// <summary>
    /// Retrieves all model versions.
    /// </summary>
    /// <remarks>
    /// Requires authorization.
    /// </remarks>
    private static async Task<IResult> Get(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        //var result = await sender.Send(new GetAllModelVersionQuery(), cancellationToken);
        //return result.Match(
        //    Left: failure => Results.Problem(failure.ErrorDescription, statusCode: 400),
        //    Right: data => Results.Ok(data));
        return Results.Ok("Sample response for Get all model versions");
    }

   
}
