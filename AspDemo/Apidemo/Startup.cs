using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Apidemo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;

namespace Apidemo
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
            InMemConfiguation inMemoryConfiguration = new InMemConfiguation();
            InMemoryCacheRevocatonHandler cacheRevocatonHandler = new InMemoryCacheRevocatonHandler(inMemoryConfiguration);
            HttpClient httpClient = new HttpClient();
            // add the required services
            services.AddRevocation(cacheRevocatonHandler, httpClient);
            AddHealthChecks(services);
            


            // add text reader support
            services.AddMvc(o => o.AddRevocationSupport())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        public void AddHealthChecks(IServiceCollection services)
        {
            services.AddSingleton<IHealthCheck, SampleHealthCheck>();
            services.AddSingleton<IHealthCheck, SampleHealthCheck2>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UsePathBase("/store/abc");
            app.UseHealthCheck();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void AddRevocationSupport(this MvcOptions opts)
        {
            opts.InputFormatters.Insert(0, new TextRequestBodyFormatter());
            
        }
    }
    public static class RevocationServiceCollectionsExtensions
    {
        public static void AddRevocation(this IServiceCollection services,
            IRevocationMessageHandler revocationMessageHandler, HttpClient httpClient)
        {
            services.AddSingleton<HttpClient>(httpClient);
            services.AddSingleton<IRevocationMessageHandler>(revocationMessageHandler);

        }
    }

}
