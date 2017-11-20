using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Registration.API.CustomDtoMapper;
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
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = "https://tolleyfam.auth0.com/";
                options.Audience = "http://localhost:19671";
                options.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Over21", policy => policy.Requirements.Add(new MinimumAgeRequirement(21)));
            });

            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://authdemo.dev:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );
            });

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
            
            app.UseCors("AllowSpecificOrigin");

            registrationContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            app.UseAuthentication();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Event, Models.EventDto>();
                cfg.CreateMap<Entities.Event, Models.EventForUpdateDto>();
                cfg.CreateMap<Models.EventForCreationDto, Entities.Event>();
                cfg.CreateMap<Models.EventForUpdateDto, Entities.Event>();

                cfg.CreateMap<Entities.Group, Models.GroupDto>();
                cfg.CreateMap<Entities.Group, Models.GroupWithSubgroupsDto>();
                cfg.CreateMap<Entities.Group, Models.GroupForUpdateDto>();
                cfg.CreateMap<Models.GroupForCreationDto, Entities.Group>();
                cfg.CreateMap<Models.GroupForUpdateDto, Entities.Group>();

                cfg.CreateMap<Entities.Subgroup, Models.SubgroupDto>();
                cfg.CreateMap<Entities.Subgroup, Models.SubgroupForUpdateDto>();
                cfg.CreateMap<Models.SubgroupForCreationDto, Entities.Subgroup>();
                cfg.CreateMap<Models.SubgroupForUpdateDto, Entities.Subgroup>();

                cfg.CreateMap<Entities.User, Models.UserDto>();
                cfg.CreateMap<Entities.User, Models.UserWithRolesDto>().ConvertUsing<UserToUserWithRolesDtoConverter>();

                cfg.CreateMap<Entities.User, Models.UserForUpdateDto>();
                cfg.CreateMap<Models.UserForCreationDto, Entities.User>();
                cfg.CreateMap<Models.UserForUpdateDto, Entities.User>();
            });
            
            app.UseMvc();
        }
    }
}
