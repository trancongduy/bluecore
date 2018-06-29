using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blue.Data;
using Blue.Api.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Http;
using Blue.Data.IdentityService;

namespace Blue.Api
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<BlueDbContext>
    {
		public BlueDbContext CreateDbContext(string[] args)
		{
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var contentRootPath = Directory.GetCurrentDirectory();

			var builder = new ConfigurationBuilder()
							.SetBasePath(contentRootPath)
							.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
							.AddJsonFile($"appsettings.{environmentName}.json", true);

			builder.AddEnvironmentVariables();
			var configuration = builder.Build();

			//setup DI
			var containerBuilder = new ContainerBuilder();
			IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserResolverService, UserResolverService>();

            services.LoadInstalledModules(contentRootPath);
            services.AddCustomizedDataStore(configuration);
			containerBuilder.Populate(services);
			var serviceProvider = containerBuilder.Build().Resolve<IServiceProvider>();

			return serviceProvider.GetRequiredService<BlueDbContext>();
		}
    }
}
