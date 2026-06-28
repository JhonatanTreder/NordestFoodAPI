namespace NordesteFoodAPI.Shared.Common.Results
{
    public enum ErrorType
    {
        None = 0,
        ValidationError = 1,
        CreateConflict = 2,
        Conflict = 3,
        NotFound = 4,
        Unauthorized = 5,
        UnexpectedFailure = 6
    }
}
