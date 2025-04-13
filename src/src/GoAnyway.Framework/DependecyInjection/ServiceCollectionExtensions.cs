using GoAnyway.Framework.Assertion;
using Microsoft.Extensions.DependencyInjection;

namespace GoAnyway.Framework.DependecyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddByAttribute<TAttribute>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient) 
        where TAttribute : Attribute
    {
        services.ThrowIfNull();
        lifetime.ThrowIfNotDefined();

        return services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.WithAttribute<TAttribute>(), publicOnly: false)
            .AsImplementedInterfaces()
            .WithLifetime(lifetime)
        );
    }
}
