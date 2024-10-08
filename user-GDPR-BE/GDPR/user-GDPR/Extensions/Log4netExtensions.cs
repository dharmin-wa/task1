using log4net.Config;
using log4net;
using System.Reflection;
using user_GDPR.Middlewares;

namespace user_GDPR.Extensions
{
    public static class Log4netExtensions
    {
        public static IServiceCollection AddLog4Net(this IServiceCollection services, string configFilePath)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(configFilePath));

            return services;
        }
    }
}