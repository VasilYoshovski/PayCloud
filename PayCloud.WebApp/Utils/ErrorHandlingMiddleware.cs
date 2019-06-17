using Microsoft.AspNetCore.Http;
using PayCloud.Services.Utils;
using System;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Utils
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(ServiceErrorException ex)
            {
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ex.Message);
                }
                else
                {
                    context.Response.Redirect("/Error?errorMessage=" +ex.Message);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(Constants.NotAuthorized);
                }
                else
                {
                    context.Response.Redirect("/Error?errorMessage=" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(Constants.CommonError);
                }
                else
                {
                    context.Response.Redirect("/Error?errorMessage="+ Constants.CommonError);
                }
            }
        }
    }
}
