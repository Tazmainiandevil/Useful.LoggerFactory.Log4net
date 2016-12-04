using Microsoft.Extensions.Logging;
using System;

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
        /// <returns>The factory as a fluent interface</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            factory.AddProvider(new Log4NetProvider());

            return factory;
        }
    }
}