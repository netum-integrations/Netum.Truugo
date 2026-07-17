using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.ValidatorListProfiles.Definitions;
using NUnit.Framework;

namespace Netum.Truugo.ValidatorListProfiles.Tests;

[TestFixture]
internal class ErrorHandlerTest : TestBase
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
           Truugo.ValidatorListProfiles(emptyConnection, DefaultOptions(), CancellationToken.None).GetAwaiter().GetResult()));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var result = await Truugo.ValidatorListProfiles(emptyConnection, options, CancellationToken.None);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
            Truugo.ValidatorListProfiles(emptyConnection, options, CancellationToken.None).GetAwaiter().GetResult()));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message, Contains.Substring(CustomErrorMessage));
    }

    [Test]
    public async Task MissingCredentialsShouldReturnError()
    {
        var connection = new Connection
        {
            Username = string.Empty,
            Password = string.Empty,
        };

        var options = new Options
        {
            ThrowErrorOnFailure = false,
        };

        var result = await Truugo.ValidatorListProfiles(connection, options, CancellationToken.None);

        TestContext.WriteLine($"MissingCredentialsShouldReturnError() Success: {result.Success}");
        TestContext.WriteLine($"MissingCredentialsShouldReturnError() Error Message: {result.Error?.Message}");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Does.Contain("Username field is required").IgnoreCase);
        Assert.That(result.Error!.Message, Does.Contain("Password field is required").IgnoreCase);
    }
}
