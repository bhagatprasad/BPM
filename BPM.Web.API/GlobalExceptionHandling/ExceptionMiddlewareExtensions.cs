using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json; 

namespace BPM.Web.API.GlobalExceptionHandling
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                       
                        var errorResponse = new 
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please try again later."
                        };

                        
                        var jsonString = JsonSerializer.Serialize(errorResponse);
                        await context.Response.WriteAsync(jsonString);
                    }
                });
            });
        }
    }
}