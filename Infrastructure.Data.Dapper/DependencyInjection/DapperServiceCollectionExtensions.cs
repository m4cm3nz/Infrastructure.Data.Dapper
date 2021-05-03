using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperServiceCollectionExtensions
    {
        public static IServiceCollection AddDapper(this IServiceCollection services, Action<IDapperBuilder> configure)
        {
            var builder = new DapperBuilder(services);

            configure(builder);

            builder.Validate();

            return services;
        }
    }
}
