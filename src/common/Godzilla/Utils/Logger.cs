using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Godzilla.Utils
{
    public static class Logger
    {
        private static ILoggerFactory _Factory = new LoggerFactory();

        internal static void ConfigureLogging(ILoggerFactory factory)
        {
            _Factory = factory;
        }

        public static ILogger Application => CreateLogger();

        private static ILogger CreateLogger()
        {
            var category = GetCallerCategory();
            return _Factory.CreateLogger(category);
        }

        private static string GetCallerCategory()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(4);
            return $"Godzilla | {frame.GetMethod().DeclaringType.Name} | {frame.GetMethod().Name}";
        }
    }
}
