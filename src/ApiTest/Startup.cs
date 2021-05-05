using eazy.rest.data;
using eazy.rest.extension.AntiXss;
using eazy.rest.extension.Cors;
using eazy.rest.extension.EfCore;
using eazy.rest.services.Interface;
using eazy.rest.services.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEfCore<DataContext>(Configuration);
            services.AddHttpContextAccessor();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eazy.Rest.Api", Version = "v1" });
            });

            services.AddScoped<IRestService<Guid>, RestService>();

            services.AddCorsOption(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseCorsOption(Configuration);
            app.UseAntiXssMiddleware(Configuration);

#if DEBUG
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eazy.Rest.Api v1");
                c.OAuthClientId(Configuration["IdpSettings:ClientId"]);
                c.OAuthClientSecret(Configuration["IdpSettings:ClientSecret"]);
                c.OAuthAppName("Eazy Rest Api v1");
                c.OAuthScopeSeparator(" ");
            });
#else
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Configuration["AppSettings:Folder"]}/swagger/v1/swagger.json",
                    "Eazy.Rest.Api v1");
                c.OAuthClientId(Configuration["IdpSettings:ClientId"]);
                c.OAuthClientSecret(Configuration["IdpSettings:ClientSecret"]);
                c.OAuthAppName("Eazy Rest Api v1");
                c.OAuthScopeSeparator(" ");
            });
#endif


            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();




            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
