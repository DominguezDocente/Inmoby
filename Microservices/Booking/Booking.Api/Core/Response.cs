using System.Text.Json.Serialization;

namespace Booking.Api.Core
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public static Response Success(string message = "Tarea realizada con éxito")
        {
            return new Response
            {
                IsSuccess = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static Response Failure(string message,
                                       List<string>? errors = null,
                                       int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Response
            {
                IsSuccess = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }

        public static Response Failure(Exception ex,
                                       string message = "Ha ocurrido un error al generar la solicitud",
                                       int statusCode = StatusCodes.Status500InternalServerError)
        {
            return new Response
            {
                IsSuccess = false,
                Message = message,

                // TODO: Remove in production, only for debugging purposes
                Errors = new List<string> { ex.Message },
                StatusCode = statusCode
            };
        }

        public static Response NotFound(string message)
        {
            return Failure(message, statusCode: StatusCodes.Status404NotFound);
        }

        public static Response Forbidden(string message = "No tiene permisos para realizar esta acción.")
        {
            return Failure(message, statusCode: StatusCodes.Status403Forbidden);
        }
    }

    public class Response<TResult> : Response
    {
        public TResult? Result { get; set; }

        public static Response<TResult> Success(TResult result, string message = "Tarea realizada con éxito")
        {
            return new Response<TResult>
            {
                IsSuccess = true,
                Message = message,
                Result = result,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static new Response<TResult> Failure(string message,
                                                    List<string>? errors = null,
                                                    int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Response<TResult>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }

        public static new Response<TResult> Failure(Exception ex,
                                                    string message = "Ha ocurrido un error al generar la solicitud",
                                                    int statusCode = StatusCodes.Status500InternalServerError)
        {
            return new Response<TResult>
            {
                IsSuccess = false,
                Message = message,

                // TODO: Remove in production, only for debugging purposes
                Errors = new List<string> { ex.Message },
                StatusCode = statusCode
            };
        }

        public static new Response<TResult> NotFound(string message)
        {
            return Failure(message, statusCode: StatusCodes.Status404NotFound);
        }

        public static new Response<TResult> Forbidden(string message = "No tiene permisos para realizar esta acción.")
        {
            return Failure(message, statusCode: StatusCodes.Status403Forbidden);
        }
    }
}
