using System;
using System.Windows;

namespace Toolkit.Base.Log
{
    public static class Logger
    {
        public static void Trace(Exception exception, string message, string? position)
        {
            GetLogger().Trace(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Trace");
#endif
        }

        public static void Debug(Exception exception, string message, string? position)
        {
            GetLogger().Debug(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Debug");
#endif
        }

        public static void Info(Exception exception, string message, string? position)
        {
            GetLogger().Info(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Info");
#endif
        }

        public static void Warn(Exception exception, string message, string? position)
        {
            GetLogger().Warn(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Warn");
#endif
        }

        public static void Error(Exception exception, string message, string? position)
        {
            GetLogger().Error(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Error");
#endif
        }

        public static void Fatal(Exception exception, string message, string? position)
        {
            GetLogger().Fatal(exception, $"{position}\n{message}");
#if DEBUG
            MessageBox.Show($"{position}\n{message}\n{exception.Message}", "Toolkit - Logger.Fatal");
#endif
        }

        private static NLog.Logger GetLogger()
        {
            if (logger == null)
            {
                string name = "LogFile";
                var config = new NLog.Config.LoggingConfiguration();
                var logfile = new NLog.Targets.FileTarget(name)
                {
                    FileName = $"{name}/{DateTime.Now:yyyy-MM-dd}.txt",
                    Layout = "[${level:uppercase=true}]\n${date}\n${message}\n${exception}\n",
                };
                config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);
                NLog.LogManager.Configuration = config;
                logger = NLog.LogManager.GetCurrentClassLogger();
            }
            return logger;
        }

        private static NLog.Logger? logger = null;
    }
}
