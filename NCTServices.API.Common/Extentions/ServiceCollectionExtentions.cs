using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.OpenApi.Models;
using NCTServices.Shared.Constants;
using NCTServices.Shared.Configurations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Infrastructure.Responsitories;
using NCTServices.Infrastructure.Contexts;
using System.Reflection;

namespace NCTServices.API.Common.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddCommonCors(this IServiceCollection services, IConfiguration configuration)
        {
            var isAllowedOriginEnabled = configuration.GetValue<bool>("AllowedOrigins:Enable");
            var stringUrls = configuration.GetSection("AllowedOrigins:Urls").Get<List<string>>();

            if (isAllowedOriginEnabled && stringUrls.Count > 0)
            {
                var corsOriginAllowed = stringUrls.ToArray();
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.SetIsOriginAllowed((host) =>
                        {
                            var result = corsOriginAllowed.Any(origin =>
                                Regex.IsMatch(host, origin, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100)));
                            return result;
                        })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                        //.SetIsOriginAllowed((host) => true));
            });
            }
        }
        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NCT.Services.API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DatabaseConnectionConfiguration>().Bind(configuration.GetSection(StartupConstant.DBConnection))
             .Validate(config =>
             {
                 config.DBConnection = configuration[StartupConstant.DBConnection];
                 return true;
             });
            services.AddDbContext<NCTServicesDBReadContext>(options => options
                    .UseSqlServer(configuration[StartupConstant.DBConnection]));

            services.AddDbContext<NCTServicesDBWriteContext>(options => options
                    .UseSqlServer(configuration[StartupConstant.DBConnection]));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<NCTServicesDBReadContext>());
            services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
            services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
            return services;
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(ResponsitoryAsync<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
        public static void ConfigureKeyVault(this WebApplicationBuilder builder)
        {
            try
            {
                builder.Host.ConfigureAppConfiguration((context, config) =>
                {
                    var secretClient = new SecretClient(
                        new Uri(builder.Configuration[StartupConstant.KeyVaultURL]),
                        new DefaultAzureCredential());
                    config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
