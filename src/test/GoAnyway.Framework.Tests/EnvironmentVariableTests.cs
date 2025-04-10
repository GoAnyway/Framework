

using FluentAssertions;
using GoAnyway.Framework.Configuration;

namespace GoAnyway.Framework.Tests;

public class EnvironmentVariableTests
{
    private const string VariableName = "MY_TEST_ENV_VAR";

    [Fact]
    public void Get_Generic_WithConverter_ShouldReturnConvertedValue()
    {
        Environment.SetEnvironmentVariable(VariableName, "123");

        int result = EnvironmentVariable.Get(VariableName, int.Parse);

        result.Should().Be(123);
    }

    [Fact]
    public void Get_Generic_WithConverterAndDefault_ShouldReturnConvertedValue_IfSet()
    {
        Environment.SetEnvironmentVariable(VariableName, "true");

        bool result = EnvironmentVariable.Get(VariableName, bool.Parse, false);

        result.Should().BeTrue();
    }

    [Fact]
    public void Get_Generic_WithConverterAndDefault_ShouldReturnDefault_IfUnset()
    {
        Environment.SetEnvironmentVariable(VariableName, null);

        int result = EnvironmentVariable.Get(VariableName, int.Parse, 42);

        result.Should().Be(42);
    }

    [Fact]
    public void Get_String_ShouldReturnValue_IfSet()
    {
        Environment.SetEnvironmentVariable(VariableName, "abc");

        string result = EnvironmentVariable.Get(VariableName);

        result.Should().Be("abc");
    }

    [Fact]
    public void Get_String_ShouldThrow_IfNotSet()
    {
        Environment.SetEnvironmentVariable(VariableName, null);

        Action act = () => EnvironmentVariable.Get(VariableName);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage($"Environment variable '{VariableName}' is not set.");
    }

    [Fact]
    public void Get_String_WithDefault_ShouldReturnValue_IfSet()
    {
        Environment.SetEnvironmentVariable(VariableName, "value");

        string result = EnvironmentVariable.Get(VariableName, "fallback");

        result.Should().Be("value");
    }

    [Fact]
    public void Get_String_WithDefault_ShouldReturnDefault_IfUnset()
    {
        Environment.SetEnvironmentVariable(VariableName, null);

        string result = EnvironmentVariable.Get(VariableName, "fallback");

        result.Should().Be("fallback");
    }

    [Fact]
    public void Find_Generic_ShouldReturnConvertedValue_IfSet()
    {
        Environment.SetEnvironmentVariable(VariableName, "9.9");

        var result = EnvironmentVariable.Find(VariableName, double.Parse);

        result.Should().Be(9.9);
    }

    [Fact]
    public void Find_Generic_ShouldReturnNull_IfUnset()
    {
        Environment.SetEnvironmentVariable(VariableName, null);

        var result = EnvironmentVariable.Find(VariableName, int.Parse);

        result.Should().Be(null);
    }

    [Fact]
    public void Find_String_ShouldReturnValue_IfSet()
    {
        Environment.SetEnvironmentVariable(VariableName, "yo");

        var result = EnvironmentVariable.Find(VariableName);

        result.Should().Be("yo");
    }

    [Fact]
    public void Find_String_ShouldReturnNull_IfUnset()
    {
        Environment.SetEnvironmentVariable(VariableName, null);

        var result = EnvironmentVariable.Find(VariableName);

        result.Should().BeNull();
    }
}
