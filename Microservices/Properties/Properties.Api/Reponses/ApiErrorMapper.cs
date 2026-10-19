using Properties.Application.Utilities.Results;

namespace Properties.Api.Reponses
{
    internal static class ApiErrorMapper
    {
        public static ApiError From(Error error)
        { 
            return new(error.Code,
                       error.Description,
                       MapType(error.Type));
        }

        public static IReadOnlyList<ApiError> FromMany(IReadOnlyCollection<Error> errors)
        {
            if (errors.Count == 0)
            {
                return [];
            }

            return errors.Select(From).ToList();
        }

        private static string MapType(ErrorType type) => type switch
        {
            ErrorType.None => "none",
            ErrorType.Validation => "validation",
            ErrorType.Business => "business",
            ErrorType.NotFound => "notFound",
            ErrorType.Unexpected => "unexpected",
            _ => "unexpected"
        };
    }
}
