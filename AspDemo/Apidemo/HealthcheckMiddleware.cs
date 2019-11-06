using Apidemo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apidemo
{
    public static class HealthcheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseHealthCheck(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HealthcheckMiddleware>();
        }
    }

    public class HealthcheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IHealthCheck> _healthChecks;

        public HealthcheckMiddleware(RequestDelegate next, IEnumerable<IHealthCheck> healthChecks)
        {
            _next = next;
            _healthChecks = healthChecks;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // https://github.com/ycrumeyrolle/HealthCheck/blob/master/src/AspNetCore.HealthCheck/HealthCheckMiddleware.cs
            var path = context.Request.Path;
            if (!string.IsNullOrEmpty(path) &&
                path.Equals("/healthcheck", StringComparison.InvariantCultureIgnoreCase))
            {
                // do all health checks with timeouts
                // var results = _healthChecks.Select((hc) => hc.Check());

                context.Response.StatusCode = StatusCodes.Status200OK;
                var headers = context.Response.Headers;
                headers[HeaderNames.CacheControl] = "no-store, no-cache";
                headers[HeaderNames.Pragma] = "no-cache";
                headers[HeaderNames.Expires] = "Thu, 01 Jan 1970 00:00:00 GMT";
                //await _healthCheckOptions.ResponseWriter(context, result);
                context.Response.WriteAsync("write json here");

            }
            else
            {

                // check if the request is for /healthcheck, if yes terminaite the request
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
        }
    }
    
}
