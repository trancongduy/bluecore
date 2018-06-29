using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Framework.Common.Middlewares;
using Framework.Constract.SeedWork;
using Framework.Constract.Constant;
using Blue.Data;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;

namespace Blue.IdentityServer
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

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlueDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Blue.Api")));

            //services.AddDbContext<BlueDbContext>(options =>
            //        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly("Blue.Api")));

            //Add Customize Identity
            services.AddIdentity<User, Role>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserStore>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddUserManager<ApplicationUserManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddEntityFrameworkStores<BlueDbContext>()
                .AddDefaultTokenProviders();
                    //.AddIdentityServer();


            // Add Identity Server
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential(filename: "tempkey.rsa")
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<User>();

            // Still working fine with IdentityServer4
            services.AddMvc();

            //services.AddMvcCore()
            //        .AddAuthorization()
            //        .AddJsonFormatters(j =>
            //        {
            //            j.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //            j.Formatting = Formatting.Indented;
            //        });

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

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            // Handle Error for API
            #region Exception Handler
            //app.UseExceptionHandler(appBuilder =>
            //{
            //    appBuilder.Use(async (context, next) =>
            //    {
            //        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            //        //when authorization has failed, should retrun a json message to client
            //        if (error != null && error.Error is SecurityTokenExpiredException)
            //        {
            //            var result = new RequestResult();
            //            result.State = RequestState.NotAuth;
            //            result.Messages.Add(new ValidationError
            //            {
            //                Error = "Token expired"
            //            });

            //            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //            context.Response.ContentType = HttpContentType.ApplicationJson;

            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            //        }
            //        //when orther error, retrun a error message json to client
            //        else if (error != null && error.Error != null)
            //        {
            //            var result = new RequestResult();
            //            result.State = RequestState.Failed;
            //            result.Messages.Add(new ValidationError {
            //                Error = error.Error.Message,
            //                StackTrace = error.Error.StackTrace
            //            });

            //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //            context.Response.ContentType = HttpContentType.ApplicationJson;
            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            //        }
            //        //when no error, do next.
            //        else await next();
            //    });
            //});

            //app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            #endregion

            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
