namespace NordesteFoodAPI.Shared.Common.Results
{
    public enum ErrorType
    {
        None,
        ValidationError,
        CreateConflict,
        Conflict,
        NotFound,
        Unauthorized,
        UnexpectedFailure,
        Failure,
        DatabaseError
    }
}
