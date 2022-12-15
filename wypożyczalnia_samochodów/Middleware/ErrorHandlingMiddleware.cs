using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UI.Middleware
{
    public class ErrorHandlingMiddleware: IMiddleware
    {
        public async Task InvokeAsync(HttpContext context,RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}	
            catch(BadRequestException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            //catch(InvalidOperationException e)
            //{
            //    await context.Response.WriteAsync("Złe Id");
            //}
			catch(InvalidEnumArgumentException e)
			{
                await context.Response.WriteAsync("Zła klasa");
            }
        }
    }
}
