namespace DomainErrors
{
    public enum FailureType
    {
        Failure = 400,
        ValidationFailure = 400,// 400 Bad Request
        NotFoundFailure = 404, //404 Not Found
        DuplicateFailure = 409,// 409 Conflict
        ForbiddenFailure = 403,// 403 Forbidden
        UnauthorizedFailure = 401,// 401 Unauthorized
        NotImplementedFailure = 501,// 501 Not Implemented
        ServiceUnavailableFailure = 503,// 503 Service Unavailable
        ConflictFailure = 409,// 409 Conflict
        BadRequestFailure = 400,// 400 Bad Request
        InternalServerErrorFailure = 500,// 500 Internal Server Error

    }

    public interface IGeneralFailure
    {
        string Code { get; init; }
        string ErrorDescription { get; init; }
        string OriginalError { get; init; }
        FailureType FailureType { get; init; }
        //void Deconstruct(out string Code, out string ErrorType, out string ErrorDescription);
        //bool Equals(GeneralFailure? other);
        //bool Equals(object? obj);
        //int GetHashCode();
        //string ToString();
    }
    public record GeneralFailure(string Code, string OriginalError, string ErrorDescription, FailureType FailureType) : IGeneralFailure
    {
        public static GeneralFailure None => new(string.Empty, "NONE", string.Empty, FailureType.Failure);
    }
    public static class GeneralFailures
    {
        public static GeneralFailure DuplicateEntity(string? value) => new("A01", $"{value} ", "Data  already Exist in Repository", FailureType.DuplicateFailure);
        public static GeneralFailure ErrorRetrievingListDataFromRepository(string? value) => new("A02", $"{value} ", "Error Retrieving  List From  Repository", FailureType.InternalServerErrorFailure);
        public static GeneralFailure ErrorRetrievingSingleDataFromRepository(string? value) => new("A03", $"{value} ", "Error Retrieving  Single Entity From  Repository/Null Result", FailureType.InternalServerErrorFailure);
        public static GeneralFailure ProblemAddingEntityIntoDbContext(string? value) => new("A04", $"{value} ", "Error Adding entity  into to Repository", FailureType.InternalServerErrorFailure);
        public static GeneralFailure ProblemDeletingEntityFromRepository(string? value) => new("A05", $"{value}", "Error Deleting entity  in Repository", FailureType.InternalServerErrorFailure);
        public static GeneralFailure ProblemUpdatingEntityInRepository(string? value) => new("A06", $"{value} ", "Error Updating entity  in Repository", FailureType.InternalServerErrorFailure);
        public static GeneralFailure DataNotFoundInRepository(string? value) => new("A07", $"{value} ", "Data Not Found  in Repository", FailureType.NotFoundFailure);
        public static GeneralFailure ExceptionThrown(string where, string? summary, string details) => new("A08", $": Exception Thrown : {summary}", $"{details} ", FailureType.InternalServerErrorFailure);
        public static GeneralFailure ErrorThrown(string where, string? summary, string details, FailureType failure) => new("A09", $": Exception Thrown : {summary}", $"{details} ", failure);

    }
}
