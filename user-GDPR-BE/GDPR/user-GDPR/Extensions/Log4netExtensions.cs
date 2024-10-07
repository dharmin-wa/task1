using user_GDPR.Middlewares;

namespace user_GDPR.Extensions
{
	public static class Log4netExtensions
	{
		public static void AddLog4net(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
