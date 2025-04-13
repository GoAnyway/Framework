using GoAnyway.Framework.DependecyInjection;
using GoAnyway.Framework.Tests.DependencyInjection.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace GoAnyway.Framework.Tests.DependencyInjection;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddByAttribute_RegistersServiceWithCorrectLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddByAttribute<TestFeatureAttribute>(ServiceLifetime.Scoped);
        var provider = services.BuildServiceProvider();

        // Assert
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ITestService));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);

        var service = provider.GetService<ITestService>();
        Assert.NotNull(service);
        Assert.IsType<TestService>(service);
    }

    [Fact]
    public void AddByAttribute_DoesNotRegisterClassesWithoutAttribute()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddByAttribute<TestFeatureAttribute>();
        var provider = services.BuildServiceProvider();

        // Assert
        var allServices = services.Where(s => s.ServiceType == typeof(ITestService)).ToList();

        Assert.Single(allServices); // Только TestService
        Assert.IsType<TestService>(provider.GetService<ITestService>());
    }

    [Fact]
    public void AddByAttribute_ThrowsIfServicesNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ((IServiceCollection)null!).AddByAttribute<TestFeatureAttribute>();
        });
    }

    [Fact]
    public void AddByAttribute_RegistersAsTransientByDefault()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddByAttribute<TestFeatureAttribute>();
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ITestService));

        // Assert
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }
}