﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registration.API.Entities;
using Registration.API.Services;

namespace Registration.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();


            var connectionString = Startup.Configuration["connectionStrings:registrationDBConnectionString"];
            services.AddDbContext<RegistrationContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RegistrationContext registrationContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            registrationContext.EnsureSeedDataForContext();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Event, Models.EventDto>();
            });

            app.UseMvc();
        }
    }
}
