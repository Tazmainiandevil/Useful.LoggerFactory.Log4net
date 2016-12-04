using Microsoft.Extensions.Logging;
using System;
using System.IO;
using log4net.Config;

namespace Useful.LoggerFactory.Log4net
{
    /// <summary>
    /// Extension class to add log4net to the ILoggerFactory
    /// </summary>
    public static class Log4NetFactoryExtensions
    {
        /// <summary>
        /// Add the log4net provider to the ILoggerFactory
        /// </summary>
        /// <param name="factory">The factory to add to</param>
        /// <param name="log4NetConfiguration">The log4net configuration file to use</param>
        /// <returns>The factory as a fluent interface</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfiguration)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (string.IsNullOrWhiteSpace(log4NetConfiguration))
            {
                throw new ArgumentNullException(nameof(log4NetConfiguration));
            }

            var filePath = new FileInfo(log4NetConfiguration);
            XmlConfigurator.Configure(filePath);            

            factory.AddProvider(new Log4NetProvider());

            return factory;
        }
    }
}