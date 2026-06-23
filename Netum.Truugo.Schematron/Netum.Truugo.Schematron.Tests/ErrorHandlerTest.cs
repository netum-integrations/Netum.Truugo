using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Schematron.Definitions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Netum.Truugo.Schematron.Tests;

[TestFixture]
internal class ErrorHandlerTest : TestBase
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
           Truugo.Schematron(DefaultInput(), emptyConnection, DefaultOptions(), CancellationToken.None).GetAwaiter().GetResult()));
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var result = await Truugo.Schematron(DefaultInput(), DefaultConnection(), options, CancellationToken.None);
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var emptyConnection = new Connection { Username = string.Empty, Password = string.Empty };
        var ex = Assert.Throws<Exception>((Action)(() =>
            Truugo.Schematron(DefaultInput(), emptyConnection, options, CancellationToken.None).GetAwaiter().GetResult()));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message, Contains.Substring(CustomErrorMessage));
    }

    [Test]
    public async Task MissingCredentialsShouldReturnError()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FilePath,
            FilePath = "./TestFiles/finvoice_30_example2.xml",
            FileKey = "EN16931/CII/LATEST",
        };

        // Set ThrowErrorOnFailure to false to get error details in the response instead of an exception
        var options = new Options
        {
            ThrowErrorOnFailure = false,
        };

        var emptyConnection = new Connection
        {
            Username = string.Empty,
            Password = string.Empty,
        };

        var result = await Truugo.Schematron(input, emptyConnection, options, CancellationToken.None);
        TestContext.WriteLine($"MissingCredentialsShouldReturnError() response: {result.Data}, error: {result.Error?.Message}");

        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Does.Contain("Username field is required").IgnoreCase);
        Assert.That(result.Error!.Message, Does.Contain("Password field is required").IgnoreCase);
    }
}
