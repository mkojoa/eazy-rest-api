using eazy.rest.extension.AntiXss.Info;
using eazy.rest.extension.AntiXss.Option;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace eazy.rest.extension.AntiXss
{
    public static class AntiXssMiddlewareExtension
    {
        //app.UseAntiXssMiddleware();
        public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder,
            IConfiguration configuration)
        {
            var options = new AntiXssOptions();
            configuration.GetSection(nameof(AntiXssOptions)).Bind(options);

            var isEnabled = options.Enabled ? true : false;

            if (isEnabled) return builder.UseMiddleware<AntiXssMiddleware>();

            return builder;
        }
    }
}