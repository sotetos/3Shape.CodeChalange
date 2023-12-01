using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Internals;
using Services.Ports;

namespace Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddTransient<IDataImporter, DataService>();
            services.TryAddTransient<IImportDataParser, ImportDataParser>();
            services.TryAddTransient<ISearchStringParser, SearchStringParser>();

            services.AddSingleton<PretendBookDataSource>();

            return services;
        }
    }
}
