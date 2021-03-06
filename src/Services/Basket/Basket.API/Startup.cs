using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Eventbus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API
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
            //Redis Configuration
            RedisConfiguration(services);

            //General Configuration
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));

            //gRPC Configuration
            RegisterGrpcClientService(services);

            //Masstransit-RabbitMQ Configuration
            RegisterMassTransit(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
        }

        private void RedisConfiguration(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString"));
        }

        private void RegisterGrpcClientService(IServiceCollection services)
        {
            services.AddGrpcClient<DiscountProtoServiceClient>(options =>
                options.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

            services.AddScoped<DiscountGrpcService>();
        }

        private void RegisterMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);

                    // Add this line to change exchange type to topic (default is fanout)
                    cfg.Publish<BasketCheckoutEvent>(c =>
                    {
                        c.ExchangeType = ExchangeType.Topic;
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}