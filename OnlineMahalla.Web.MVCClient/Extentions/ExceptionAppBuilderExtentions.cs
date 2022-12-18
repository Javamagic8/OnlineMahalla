using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UZASBO.Web.MVCClient;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionAppBuilderExtentions
    {
        public static void ConfigureExceptions(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    //var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        var ex = GetInnermostException(error.Error);
                        var problem = new ProblemDetails
                        {
                            Status = 500,
                            Title = ex.Message
                        };

                        problem.Detail = $"StackTrace: {Environment.NewLine}{Environment.NewLine} Full exception: {Environment.NewLine}{error.Error}";
                        //logger.LogError(ex, ex.Message);
                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(problem));
                    }
                });
            });
        }

        public static Exception GetInnermostException(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            return GetInnermostException(ex.InnerException);
        }
    }
}
