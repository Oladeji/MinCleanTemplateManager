using DomainErrors;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GlobalConstants
{
    public static class EitherToIResultExtensions
    {
        public static Task<IResult> ToIResult<L, R>(this Task<Either<L, R>> either)
        {
            return either.Map(Match);
        }
        private static IResult Match<L, R>(this Either<L, R> either)
        {

            return either.Match<IResult>(
                Left: l => l.ToIResultProblemDetails(),
                Right: r => Results.Ok(r));

        }



        public static IResult ToIResultProblemDetails(this Object theerror)
        {
            if (theerror is GeneralFailure error)
            {
                var problemDetails = new ProblemDetails
                {
                    Detail = $"{error.OriginalError} {error.Code}",
                    Title = error.ErrorDescription,
                    Status = (int)error.FailureType,
                    Type = error.Code,
                    Instance = error.OriginalError
                };

                return Results.Problem(
                    detail: problemDetails.Detail,
                    title: problemDetails.Title,
                    statusCode: problemDetails.Status,
                    type: problemDetails.Type,
                    instance: problemDetails.Instance
                );
            }
            // Handle cases where theerror is null or not of type GeneralFailure
            return Results.Problem(
                detail: "An unknown error occurred.",
                title: "Unknown Error",
                statusCode: 500,
                type: "UnknownError",
                instance: null
            );

        }


        public static Task<IResult> ToIResultCreated<L, R>(this Task<Either<L, R>> either, string endPoint, object data)
        {
            return either.Map(x => MatchCreated(x, endPoint, data));
        }

        private static IResult MatchCreated<L, R>(this Either<L, R> either, string endPoint, object data)
        {
            return either.Match(
                Left: l => l.ToIResultProblemDetails(),
                Right: r => Results.Created($"{endPoint}/{r}", data));
        }

        public static IResult HandleFailure(GeneralFailure failure)
        {
            return failure.FailureType switch
            {
                FailureType.ValidationFailure or FailureType.BadRequestFailure => Results.BadRequest(failure),
                FailureType.NotFoundFailure => Results.NotFound(failure),
                FailureType.DuplicateFailure or FailureType.ConflictFailure => Results.Conflict(failure),
                FailureType.ForbiddenFailure => Results.Forbid(),
                FailureType.UnauthorizedFailure => Results.Unauthorized(),
                FailureType.NotImplementedFailure => Results.StatusCode(StatusCodes.Status501NotImplemented),
                FailureType.ServiceUnavailableFailure => Results.StatusCode(StatusCodes.Status503ServiceUnavailable),
                FailureType.InternalServerErrorFailure => Results.StatusCode(StatusCodes.Status500InternalServerError),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError) // Default to 500 for unknown cases
            };
        }

    }

}
