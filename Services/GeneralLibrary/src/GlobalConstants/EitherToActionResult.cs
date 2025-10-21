
using DomainErrors;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace GlobalConstants
{
    public static class EitherToActionResultExtensions
    {
        public static Task<IActionResult> ToActionResult<L, R>(this Task<Either<L, R>> either)
        {
            return either.Map(Match);
        }

        private static IActionResult Match<L, R>(this Either<L, R> either)
        {

            return either.Match<IActionResult>(
                 Left: l => l.ToProblemDetails(),
                 Right: r => new OkObjectResult(r));
        }

        public static Task<IActionResult> ToEitherActionResult<L, R>(this Task<Either<L, R>> either)
        {
            return either.Map(MatchEitherActionResult);
        }

        private static IActionResult MatchEitherActionResult<L, R>(this Either<L, R> either)
        {
            return either.Match<IActionResult>(
                Left: l => l.ToProblemDetails(),
                Right: r => new OkObjectResult(r));
        }

        public static Task<IActionResult> ToActionResultCreated<L, R>(this Task<Either<L, R>> either, string endPoint, object data)
        {
            return either.Map(x => MatchCreated(x, endPoint, data));
        }

        private static IActionResult MatchCreated<L, R>(this Either<L, R> either, string endPoint, object data)
        {
            return either.Match(
                Left: l => l.ToProblemDetails(),
                Right: r => new CreatedResult($"{endPoint}/{r}", data));
        }
    }



    public static class ResultExtensions
    {
        public static IActionResult ToProblemDetails(this Object theerror)
        {
            // return new BadRequestObjectResult(new ApiBadRequestResponse(status, error).ProblemDetails);
            var error = theerror as GeneralFailure;
            var Problems = new ProblemDetails
            {
                Detail = $"{error.OriginalError} {error.Code}",
                Title = error.ErrorDescription,
                Status = (int)error.FailureType,
                Type = error.Code,
                Instance = error.OriginalError

            };

            return new ObjectResult(Problems);

        }
    }
}
