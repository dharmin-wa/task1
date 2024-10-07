using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace user_GDPR.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		/// <summary>
		/// Constructor of global exception middleware
		/// </summary>
		/// <param name="next"></param>
		/// <param name="logger"></param>
		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_logger = logger;
			_next = next;
		}

		/// <summary>
		/// Execute method when a exception occurs
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError($"InvokeAsync went wrong: {ex}");
				await HandleExceptionAsync(httpContext, ex, httpContext.User.Identity.Name);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception, string userName)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			_logger.LogError($"Error from custom middleware :{exception.Message}");
			return context.Response.WriteAsync($"Error from custom middleware :{userName} :{exception.Message}");
		}
	}
}
