using System.Text.Json.Serialization;

namespace Booking.Api.Core
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public List<string> Errors { get; set; } = [];

        [JsonIgnore]
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        public static Result Success(string message = "Tarea realizada con éxito", int statusCode = StatusCodes.Status200OK)
        {
            return new Result
            {
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result Failure(string message = "Ha ocurrido un error", 
                                     List<string>? errors = null, 
                                     int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? [],
                StatusCode = statusCode
            };
        }

        public static Result Failure(Exception ex,
                                     string message = "Ha ocurrido un error",
                                     int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result
            {
                IsSuccess = false,
                Message = message,
                Errors = [ ex.Message ], // TODO: Just for development, remove in production
                StatusCode = statusCode
            };
        }
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public static Result<T> Success(T data, string message = "Tarea realizada con éxito", int statusCode = StatusCodes.Status200OK)
        {
            return new Result<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result<T> Failure(string message = "Ha ocurrido un error",
                                        List<string>? errors = null,
                                        int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? [],
                StatusCode = statusCode
            };
        }

        public static Result<T> Failure(Exception ex,
                                     string message = "Ha ocurrido un error",
                                     int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = [ex.Message], // TODO: Just for development, remove in production
                StatusCode = statusCode
            };
        }

    }
}
