using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NCTServices.Application.Common.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationCommonLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }
    }
}
