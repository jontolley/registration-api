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
using Registration.API.Services.Email;

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
                options.Audience = "https://sunrise2018.org/api";
                options.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.Requirements.Add(new UserRoleRequirement()));
                options.AddPolicy("Admin", policy => policy.Requirements.Add(new AdminRoleRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, UserRoleHandler>();
            services.AddSingleton<IAuthorizationHandler, AdminRoleHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins(new[]{
                        "http://authdemo.dev:4200",
                        "https://sunrise2018.org",
                        "http://sunrise2018.org:4200",
                        "http://encampment.dev:4200",
                        "https://encampment.azurewebsites.net" })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );
            });

            var connectionString = Startup.Configuration["connectionStrings:registrationDBConnectionString"];
            services.AddDbContext<RegistrationContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IEmailService, SendGridService>();
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

                cfg.CreateMap<Models.AccommodationDto, Entities.Accommodation>();
                cfg.CreateMap<Models.MeritBadgeDto, Entities.MeritBadge>();
                cfg.CreateMap<Models.ShirtSizeDto, Entities.ShirtSize>();

                cfg.CreateMap<Entities.Attendee, Models.AttendeeStubDto>();
                cfg.CreateMap<Entities.Attendee, Models.AttendeeDto>().ConvertUsing<AttendeeToAttendeeDtoConverter>();
                cfg.CreateMap<Models.AttendeeForCreationDto, Entities.Attendee>().ConvertUsing<AttendeeForCreationDtoToAttendeeConverter>();
                //cfg.CreateMap<Models.AttendeeForUpdateDto, Entities.Attendee>().ConvertUsing<AttendeeForUpdateDtoToAttendeeConverter>();

                cfg.CreateMap<Entities.User, Models.UserDto>();
                cfg.CreateMap<Entities.User, Models.UserWithRolesDto>().ConvertUsing<UserToUserWithRolesDtoConverter>();
                cfg.CreateMap<Entities.User, Models.UserWithSubgroupsDto>().ConvertUsing<UserToUserWithSubgroupsDtoConverter>();
                cfg.CreateMap<Entities.User, Models.UserForUpdateDto>();
                cfg.CreateMap<Models.UserForCreationDto, Entities.User>();
                cfg.CreateMap<Models.UserForUpdateDto, Entities.User>();
            });
            
            app.UseMvc();
        }
    }
}
