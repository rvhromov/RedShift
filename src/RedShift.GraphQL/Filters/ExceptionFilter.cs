using RedShift.Domain.Abstractions;
using System.Net;

namespace RedShift.GraphQL.Filters;

public sealed class ExceptionFilter : IErrorFilter
{
    private const string InternalErrorMessage = "Server exception occurred.";

    public IError OnError(IError error)
    {
        var (code, message) = GetErrorInfo(error);

        return ErrorBuilder
           .New()
           .SetMessage(message)
           .SetCode(code)
           .SetPath(error.Path)
           .Build();
    }

    private (string code, string message) GetErrorInfo(IError error)
    {
        var exceptionMessage = $"{InternalErrorMessage} {GetInnerMessage(error.Exception)}";

        return error?.Exception switch
        {
            RedShiftException redShiftException => (redShiftException.Type.ToString(), redShiftException.Message),
            _ => (HttpStatusCode.InternalServerError.ToString(), exceptionMessage)
        };
    }

    private static string GetInnerMessage(Exception exception)
    {
        var currentException = exception;

        while (currentException?.InnerException != null)
        {
            currentException = currentException.InnerException;
        }

        return currentException?.Message;
    }
}
