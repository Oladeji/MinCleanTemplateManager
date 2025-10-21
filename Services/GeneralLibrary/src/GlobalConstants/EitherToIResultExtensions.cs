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
            var problemDetails = new ProblemDetails
            {
                Detail = $"{failure.ErrorDescription}",
                //Title = failure.Code,
                Title = GetTitleForFailureType(failure.FailureType),
                Status = GetStatusCodeForFailureType(failure.FailureType),
                Type = failure.Code,
                Instance = failure.OriginalError
            };

            // Add additional context in extensions for better debugging
            problemDetails.Extensions["errorCode"] = failure.Code;
            problemDetails.Extensions["failureType"] = failure.FailureType.ToString();
            problemDetails.Extensions["timestamp"] = DateTimeOffset.UtcNow;
            problemDetails.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id;

            return Results.Problem(
                detail: problemDetails.Detail,
                title: problemDetails.Title,
                statusCode: problemDetails.Status,
                type: problemDetails.Type,
                instance: problemDetails.Instance,
                extensions: problemDetails.Extensions
            );
        }

        private static string GetTitleForFailureType(FailureType failureType)
        {
            return failureType switch
            {
                FailureType.ValidationFailure => "Validation Error",
               // FailureType.BadRequestFailure => "Bad Request",
                FailureType.NotFoundFailure => "Resource Not Found",
                FailureType.DuplicateFailure => "Duplicate Resource",
               // FailureType.ConflictFailure => "Conflict Error",
                FailureType.ForbiddenFailure => "Access Forbidden",
                FailureType.UnauthorizedFailure => "Unauthorized Access",
                FailureType.NotImplementedFailure => "Feature Not Implemented",
                FailureType.ServiceUnavailableFailure => "Service Unavailable",
                FailureType.InternalServerErrorFailure => "Internal Server Error",
                _ => "Unknown Error"
            };
        }

        private static int GetStatusCodeForFailureType(FailureType failureType)
        {
            return failureType switch
            {
                FailureType.ValidationFailure or FailureType.BadRequestFailure => StatusCodes.Status400BadRequest,
                FailureType.NotFoundFailure => StatusCodes.Status404NotFound,
                FailureType.DuplicateFailure or FailureType.ConflictFailure => StatusCodes.Status409Conflict,
                FailureType.ForbiddenFailure => StatusCodes.Status403Forbidden,
                FailureType.UnauthorizedFailure => StatusCodes.Status401Unauthorized,
                FailureType.NotImplementedFailure => StatusCodes.Status501NotImplemented,
                FailureType.ServiceUnavailableFailure => StatusCodes.Status503ServiceUnavailable,
                FailureType.InternalServerErrorFailure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        public static IResult HandleFailure2(GeneralFailure failure)
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
