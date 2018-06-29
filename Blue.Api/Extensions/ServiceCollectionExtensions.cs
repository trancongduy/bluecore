using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blue.Api.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Framework.Data;
using Framework.Common.Custom;
using Framework.Constract.Interfaces;
using Framework.Data.Models;
using Framework.Data.SeedWork;
using Blue.Data;
using Blue.Data.IdentityModel;
using Blue.Data.IdentityService;
using Framework.Data.Interfaces;
using IdentityServer4.Services;
using IMapper = Framework.Constract.Interfaces.IMapper;

namespace Blue.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
		public static IServiceCollection LoadInstalledModules(this IServiceCollection services, string contentRootPath)
		{
			var modules = new List<ModuleInfo>();
			var moduleRootFolder = new DirectoryInfo(Path.Combine(contentRootPath, "Modules"));
            //var moduleRootFolder = new DirectoryInfo(contentRootPath);
			var moduleFolders = moduleRootFolder.GetDirectories();

			foreach (var moduleFolder in moduleFolders)
			{
				var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                //var binFolder = new DirectoryInfo(moduleFolder.FullName);
                if (!binFolder.Exists)
				{
					continue;
				}

				foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
				{
					Assembly assembly;
					try
					{
						assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
					}
					catch (FileLoadException)
					{
						// Get loaded assembly
						assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

						if (assembly == null)
						{
							throw;
						}
					}

					if (assembly.FullName.Contains(moduleFolder.Name))
					{
						modules.Add(new ModuleInfo
						{
							Name = moduleFolder.Name,
							Assembly = assembly,
							Path = moduleFolder.FullName
						});
					}
				}
			}

			GlobalConfiguration.Modules = modules;
			return services;
		}

		public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfigurationRoot configuration)
		{
		    var connectionString = configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<BlueDbContext>(options =>
		        options.UseLazyLoadingProxies()
		            .UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        }));

            //services.AddDbContext<BlueDbContext>(options =>
            //               options.UseNpgsql(connectionString,
            //      b => b.MigrationsAssembly(migrationsAssembly));

            return services;
		}

		public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services, IConfigurationRoot configuration)
		{
		    var connectionString = configuration.GetConnectionString("Configuration");
		    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentity<User, Role>()
                    .AddRoleStore<RoleStore>()
                    .AddUserStore<UserStore>()
                    .AddRoleManager<ApplicationRoleManager>()
                    .AddUserManager<ApplicationUserManager>()
                    .AddSignInManager<ApplicationSignInManager>()
                    .AddEntityFrameworkStores<BlueDbContext>()
                    .AddDefaultTokenProviders();

		    services.AddIdentityServer(options =>
		        {
		            options.Events.RaiseSuccessEvents = true;
		            options.Events.RaiseFailureEvents = true;
		            options.Events.RaiseInformationEvents = true;
		            options.Events.RaiseErrorEvents = true;
		        })
		        .AddDeveloperSigningCredential(filename: "tempkey.rsa")
		        // this adds the config data from DB (clients, resources, CORS)
		        .AddConfigurationStore(options =>
		        {
		            options.ConfigureDbContext = db =>
		                db.UseSqlServer(connectionString,
		                    sqlServerOptionsAction: sqlOptions =>
		                    {
		                        sqlOptions.MigrationsAssembly(migrationsAssembly);
		                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
		                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
		        })
		        // this adds the operational data from DB (codes, tokens, consents)
		        .AddOperationalStore(options =>
		        {
		            options.ConfigureDbContext = db =>
		                db.UseSqlServer(connectionString,
		                    sqlServerOptionsAction: sqlOptions =>
		                    {
		                        sqlOptions.MigrationsAssembly(migrationsAssembly);
		                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
		                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
		                    });

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
		            // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
		        })
		        .AddAspNetIdentity<User>()
		        .Services.AddTransient<IProfileService, CustomProfileService>();

            return services;
		}

		public static IServiceProvider Build(this IServiceCollection services,
			IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
		{
			var builder = new ContainerBuilder();
            // Setup DI
		    builder.RegisterType<BlueDbContext>().As<IBaseContext>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>));
            //builder.RegisterModule(new IdentityConfigModule());
            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
		    builder.RegisterType<AuditAttribute>().InstancePerLifetimeScope();

            foreach (var module in GlobalConfiguration.Modules)
			{
				builder.RegisterAssemblyTypes(module.Assembly).AsImplementedInterfaces();
            }

            //Register AutoMapper
            builder.RegisterType<CustomMapper>().As<IMapper>();

            builder.RegisterInstance(configuration);
			builder.RegisterInstance(hostingEnvironment);
			builder.Populate(services);
			var container = builder.Build();
			return container.Resolve<IServiceProvider>();
		}
    }
}

