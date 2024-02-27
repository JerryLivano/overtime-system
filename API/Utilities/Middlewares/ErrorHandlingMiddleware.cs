using System.Net;
using System.Text.Json;
using API.Utilities.ViewModels;

namespace API.Utilities.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var customErrorResponse = new CustomErrorResponseVM(StatusCodes.Status500InternalServerError,
                    HttpStatusCode.InternalServerError.ToString(),
                    "Internal Server Error occured. Please contact the administrator for more information.",
                    e.InnerException?.Message ?? e.Message);

                var serializedErrorResponse = JsonSerializer.Serialize(customErrorResponse); // Dirubah dalam bentuk json
                await context.Response.WriteAsync(serializedErrorResponse); // Ditampilkan pada page
            }
        }
    }
}
