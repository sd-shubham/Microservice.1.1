using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Timeout;
using Sample.Common.Service.DI;
using Sample.Inventory.Client;
using Sample.Inventory.Entity;
using Sample.Inventory.Mapper;

namespace Sample.Inventory
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
            services.AddMongoDb()
            .AddRepository<InventoryItem>("InventoryItems");
            services.AddAutoMapper(typeof(MapperConfig));
            services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new System.Uri("https://localhost:5001");
            })
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)),
                onRetry: (OutcomeType, timeSpan, retryAttemp) =>
                {
                    Console.WriteLine($"delaying for {timeSpan.TotalSeconds} seconds, then making retry {retryAttemp}");
                }
            ))
            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                2,
                TimeSpan.FromSeconds(7),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"opening the circuit for {timespan.Milliseconds}");
                },
                onReset: () =>
                {
                    System.Console.WriteLine("closing the circuit");
                }
            ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
