using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace apiRESTCine.WebApp.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler (ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync (
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrio un error: {Message}", exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            BadHttpRequestException when exception.Message.Contains("Parameters") => 
                (StatusCodes.Status400BadRequest, "PARAMETRO_FALTANTE_O_INVALIDO", exception.Message),

            FormatException or InvalidCastException =>
                (StatusCodes.Status400BadRequest, "TIPO_DATO_INVALIDO", "Uno de los parametros no coincide con el tipo esperado"),

            _ =>
                (StatusCodes.Status500InternalServerError, "ERROR_INTERNO_SERVIDOR", "Ocurrio un error inesperado en el servidor.")
        };

        await MensajeErrorAsync(httpContext, statusCode, title, detail, cancellationToken);

        return true;
    }

    /* Metodo con formato para errores */
    private static async Task MensajeErrorAsync (
            HttpContext context,
            int statusCode,
            string title,
            string detail,
            CancellationToken cancellationToken)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }


}
