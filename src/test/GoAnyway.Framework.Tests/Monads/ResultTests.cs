using GoAnyway.Framework.Monads;

namespace GoAnyway.Framework.Tests.Monads;

public class ResultTests
{
    [Fact]
    public void Success_ShouldHaveValueAndBeMarkedSuccess()
    {
        var result = Result<string>.Success("hello");

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("hello", result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldHaveErrorAndBeMarkedFailure()
    {
        var error = new Error("Something went wrong");
        var result = Result<string>.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitSuccessOperator_ShouldWrapValue()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitFailureOperator_ShouldWrapError()
    {
        Error error = new("fail");
        Result<string> result = error;

        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ToString_ShouldDisplaySuccessValue()
    {
        Result<int> result = 5;
        var str = result.ToString();

        Assert.Equal("Success(5)", str);
    }

    [Fact]
    public void ToString_ShouldDisplayFailureMessage()
    {
        var result = Result<int>.Failure(new Error("fail"));

        Assert.Equal("Failure(fail)", result.ToString());
    }
}