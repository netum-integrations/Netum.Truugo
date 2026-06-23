using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Validator.Definitions;
using NUnit.Framework;

namespace Netum.Truugo.Validator.Tests;

[TestFixture]
internal class ErrorHandlerTest : TestBase
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
           Truugo.Validator(DefaultInput(), emptyConnection, DefaultOptions(), CancellationToken.None).GetAwaiter().GetResult()));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var result = await Truugo.Validator(DefaultInput(), DefaultConnection(), options, CancellationToken.None);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
            Truugo.Validator(DefaultInput(), emptyConnection, options, CancellationToken.None).GetAwaiter().GetResult()));
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
            ProfileKey = string.Empty,
        };

        // Set ThrowErrorOnFailure to false to get error details in the response instead of an exception
        var options = new Options
        {
            ThrowErrorOnFailure = false,
        };

        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FilePath,
            FilePath = "./TestFiles/finvoice_30_example.xml",
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"MissingCredentialsShouldReturnError() Success result: {result.Success}");
        TestContext.WriteLine($"MissingCredentialsShouldReturnError() Error Message: {result.Error?.Message}");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Does.Contain("Username field is required").IgnoreCase);
        Assert.That(result.Error!.Message, Does.Contain("Password field is required").IgnoreCase);
        Assert.That(result.Error!.Message, Does.Contain("ProfileKey field is required").IgnoreCase);
    }
}
