using System;
using System.Collections.Generic;
using System.Linq;
using eazy.rest.extension.Logging.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace eazy.rest.extension.Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseEasyLog(this IHostBuilder hostBuilder,
            Action<LoggerConfiguration> configure = null)
        {
            return hostBuilder.UseSerilog((context, loggerConfig) =>
            {
                var options = new LoggerOptions();
                context.Configuration.GetSection(nameof(LoggerOptions)).Bind(options);

                //get logging level
                var level = FetchLogEventLevel(options.Level);

                loggerConfig.Enrich.FromLogContext()
                    .MinimumLevel.Is(level)
                    .Enrich
                    .WithProperty("Environment",
                        context.HostingEnvironment.EnvironmentName) //development ? production
                    .Enrich.WithProperty("Application", options.ApplicationName) //App Name
                    .Enrich.WithProperty("Instance", "Instance")
                    .Enrich.WithProperty("Version", "v1");

                foreach (var (key, value)
                    in options.Tags
                       ??
                       new Dictionary<string, object>())
                    loggerConfig.Enrich.WithProperty(key, value);


                foreach (var (key, value)
                    in options.MinimumLevelOverrides
                       ??
                       new Dictionary<string, string>())
                    loggerConfig.MinimumLevel.Override(key, FetchLogEventLevel(value));


                options.ExcludePaths?.ToList().ForEach(p
                    => loggerConfig.Filter
                        .ByExcluding(
                            Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

                options.ExcludeProperties?.ToList().ForEach(p
                    => loggerConfig.Filter
                        .ByExcluding(Matching.WithProperty(p)));

                var fileOptions = options.File ?? new FileOptions();
                var seqOptions = options.Seq ?? new SeqOptions();

                if (fileOptions.Enabled)
                {
                    var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "Logs/logs.txt" : fileOptions.Path;

                    if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                        interval = RollingInterval.Day;

                    loggerConfig.WriteTo.File(path, rollingInterval: interval);
                }

                if (seqOptions.Enabled)
                    loggerConfig.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
            });
        }

        private static void MapOptions(LoggerOptions loggerOptions, string appOptions,
            LoggerConfiguration loggerConfig, string environmentName)
        {
            var level = FetchLogEventLevel(loggerOptions.Level);

            loggerConfig.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", loggerOptions.ApplicationName)
                .Enrich.WithProperty("Instance", "Instance")
                .Enrich.WithProperty("Version", "v1");

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
                loggerConfig.Enrich.WithProperty(key, value);

            foreach (var (key, value) in loggerOptions.MinimumLevelOverrides ?? new Dictionary<string, string>())
            {
                var logLevel = FetchLogEventLevel(value);
                loggerConfig.MinimumLevel.Override(key, logLevel);
            }

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfig.Filter
                .ByExcluding(
                    Matching.WithProperty<string>(
                        "RequestPath", n => n.EndsWith(p))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfig.Filter
                .ByExcluding(Matching.WithProperty(p)));
        }

        private static LogEventLevel FetchLogEventLevel(string level)
        {
            return Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
                ? logLevel
                : LogEventLevel.Information;
        }
    }
}