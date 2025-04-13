namespace GoAnyway.Framework.Tests.DependencyInjection.Helpers;

internal class NonAttributedService : ITestService
{
    public string GetValue() => "should not be registered";
}