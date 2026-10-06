using Properties.Application.Exceptions;
using Properties.Application.Utilities.Results;
using Properties.Domain.Exceptions;

namespace Properties.Application.UseCases.Common
{
    public abstract class BaseHandler
    {
        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                T? value = await action();
                return Result<T>.Success(value);
            }
            catch (CustomValidationException ex)
            {
                IEnumerable<Error> errors = ex.Errors.Select(error => new Error(Code: $"Validation.{error.PropertyName}",
                                                                                Description: error.ErrorMessage,
                                                                                Type: ErrorType.Validation));

                return Result<T>.Failure(errors);
            }
            catch (BussinesRuleException ex)
            {
                return Result<T>.Failure(new Error(Code: "BusinessRule.Violation",
                                                   Description: ex.Message,
                                                   Type: ErrorType.Business));
            }
            catch(Exception ex)
            {
                return Result<T>.Failure(new Error(Code: "Unexpected",
                                                   Description: "Unexpected error occurred",
                                                   Type: ErrorType.Unexpected));
            }
        }

        protected async Task<Result> ExecuteAsync(Func<Task> action)
        {
            try
            {
                return Result.Success();
            }
            catch (CustomValidationException ex)
            {
                IEnumerable<Error> errors = ex.Errors.Select(error => new Error(Code: $"Validation.{error.PropertyName}",
                                                                                Description: error.ErrorMessage,
                                                                                Type: ErrorType.Validation));

                return Result.Failure(errors);
            }
            catch (BussinesRuleException ex)
            {
                return Result.Failure(new Error(Code: "BusinessRule.Violation",
                                                   Description: ex.Message,
                                                   Type: ErrorType.Business));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(Code: "Unexpected",
                                                   Description: "Unexpected error occurred",
                                                   Type: ErrorType.Unexpected));
            }
        }
    }
}
