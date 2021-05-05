using System.Linq;
using eazy.rest.extension.Cors.Option;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.rest.extension.Cors
{
    public static class CorsExtension
    {
        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsOption(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new CorsOptions();
            configuration.GetSection(nameof(CorsOptions)).Bind(options);

            //if Links=null, set links array to empty array
            var linksOption = options.Links ?? new string[] { };

            var policyName = options.Name;

            if (options.Enabled)
            {
                var clientUrls = linksOption.ToArray();

                services.AddCors(opt =>
                {
                    opt.AddPolicy(policyName,
                        fbuilder => fbuilder.WithOrigins(clientUrls)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
            }

            return services;
        }


        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsOption(this IApplicationBuilder app, IConfiguration configuration)
        {
            var options = new CorsOptions();
            configuration.GetSection(nameof(CorsOptions)).Bind(options);

            if (options.Enabled) app.UseCors(options.Name);

            return app;
        }
    }
}