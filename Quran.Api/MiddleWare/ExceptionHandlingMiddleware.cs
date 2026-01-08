namespace Quran.Api.MiddleWare
{
    using global::Quran.Services.Loggs;
    using System.Net;
    using System.Text.Json;

    namespace Quran.API.Middlewares
    {
        public class ExceptionHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IWebHostEnvironment _env;

            public ExceptionHandlingMiddleware(
                RequestDelegate next,
                IWebHostEnvironment env)
            {
                _next = next;
                _env = env;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private async Task HandleExceptionAsync(
                HttpContext context,
                Exception exception)
            {
                // Log Exception (Serilog)
                LogException.Logs(
                    exception,
                    $"HTTP {context.Request.Method} {context.Request.Path}"
                );

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    success = false,
                    message = _env.IsDevelopment()
                        ? exception.Message
                        : "An unexpected error occurred.",
                    statusCode = context.Response.StatusCode
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }
        }
    }

}
