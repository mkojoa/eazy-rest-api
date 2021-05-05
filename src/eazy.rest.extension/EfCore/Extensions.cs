using eazy.rest.extension.EfCore.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.rest.extension.EfCore
{
    public static class Extensions
    {
        public static IServiceCollection AddEfCore<TContext>(this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var option = new EfCoreOptions();
            configuration.GetSection(nameof(EfCoreOptions)).Bind(option);

            services.AddDbContext<TContext>(
                options =>
                    options.UseSqlServer(
                        option.ConnectionString
                    ));

            return services;
        }
    }
}