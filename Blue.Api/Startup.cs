using System;
using System.Reflection;
using AutoMapper;
using Blue.Api.Extensions;
using Blue.Data.IdentityService;
using Blue.DomainService;
using Framework.Common.Middlewares;
using Framework.Constract.Constant;
using Framework.Data.SeedWork;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NSwag.AspNetCore;

namespace Blue.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
			GlobalConfiguration.WebRootPath = _hostingEnvironment.WebRootPath;
			GlobalConfiguration.ContentRootPath = _hostingEnvironment.ContentRootPath;

			services.LoadInstalledModules(_hostingEnvironment.ContentRootPath);

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserResolverService, UserResolverService>();
            services.AddTransient<IEventService, AuditEventService>();

            services.AddHttpContextAccessor();
            services.AddCustomizedDataStore(Configuration);
            services.AddCustomizedIdentity(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = Configuration.GetSection("JWTSettings:Authority").Value;

                // name of the API resource
                options.Audience = Configuration.GetSection("JWTSettings:Audience").Value;

                options.RequireHttpsMetadata = false;
                //options.TokenValidationParameters = tokenValidationParameters;
            });

            //services.AddAuthentication()
            //    .AddOpenIdConnect(options =>
            //    {
            //        options.Authority = "http://localhost:5000";
            //        options.ClientId = "mvc";
            //        options.RequireHttpsMetadata = false;
            //    });

            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Bearer",
                //    policy => policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                //        .RequireAuthenticatedUser()
                //        .Build());

                options.AddPolicy(Policies.Admin, policy =>
                {
                    policy.RequireRole(Roles.SystemAdmin);
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // Enable Cors
            // this defines a CORS policy called "AllowAll"
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                            .AllowCredentials();
                });
            });

            // Add AutoMapper
            services.AddAutoMapper(typeof(Converter.MapBothProfile).GetTypeInfo().Assembly);

            return services.Build(Configuration, _hostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
                settings.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Blue API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "Brian Li",
                        Email = "thangln1003@gmail.com"
                    };
                };
            });

            //TODO: Exeption Handler
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseAuthentication();

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
