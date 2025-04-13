namespace GoAnyway.Framework.Tests.DependencyInjection.Helpers;

[TestFeature]
internal class TestService : ITestService
{
    public string GetValue() => "test";
}