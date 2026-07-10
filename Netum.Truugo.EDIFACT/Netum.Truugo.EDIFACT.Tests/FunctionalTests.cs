using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.EDIFACT.Definitions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

#nullable enable

namespace Netum.Truugo.EDIFACT.Tests;

[TestFixture]
internal class FunctionalTests : TestBase
{
    private Connection? connection;
    private Options? options;

    private string? pathToTestFile;
    private string? validContent;

    [SetUp]
    public void SetUp()
    {
        connection = new Connection
        {
            Username = TruugoAPIUsername,
            Password = TruugoAPIPassword,
        };
        options = new Options
        {
            ThrowErrorOnFailure = true,
            ErrorMessageOnFailure = "Task failed",
        };

        pathToTestFile = $"./TestFiles/{TestFileName}";
        validContent = System.IO.File.ReadAllText(pathToTestFile);
    }

    [Test]
    public async Task ValidContentFromFilePathShouldPass()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.CheckSyntax,
            FileImportType = FileImport.FilePath,
            FilePath = pathToTestFile,
            StorageTimeInHours = 2,
        };

        var result = await Truugo.EDIFACT(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ValidContentFromFilePathShouldPass() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("true").IgnoreCase);
    }

    [Test]
    public async Task ValidContentFromFileContentShouldPass()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetBrowser,
            FileImportType = FileImport.FileContent,
            StorageTimeInHours = 1,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.EDIFACT(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ValidContentFromFileContentShouldPass() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That((string)result.Data["browserUrl"], Is.Not.Null.And.Not.Empty);
        Assert.That((string)result.Data["fileExpiry"], Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task ToXmlShouldReturnDecodedContent()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.ToXML,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.EDIFACT(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ToXmlShouldReturnDecodedContent() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That((string)result.Data["Body"][0], Does.Contain("INVOIC").IgnoreCase);
        Assert.That((string)result.Data["Body"][0], Does.Contain("<?xml version").IgnoreCase);
    }

    [Test]
    public async Task MultipleFilesShouldBeConvertedToXmlCorrectly()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.ToXML,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent + validContent,
        };

        var result = await Truugo.EDIFACT(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"MultipleFilesShouldBeConvertedToXmlCorrectly() response: {result.Data}");

        var decodedContent = result.Data["Body"];

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(decodedContent.Count, Is.EqualTo(2), "Expected 2 decoded messages in the response.");
        Assert.That((string)decodedContent[0], Does.Contain("INVOIC").IgnoreCase);
        Assert.That((string)decodedContent[1], Does.Contain("INVOIC").IgnoreCase);
        Assert.That((string)decodedContent[0], Does.Contain("<?xml version").IgnoreCase);
        Assert.That((string)decodedContent[1], Does.Contain("<?xml version").IgnoreCase);
    }

    [Test]
    public async Task CancellationTokenShouldCancelRequest()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.CheckSyntax,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100); // Cancel after 100ms

        Exception? ex = null;
        try
        {
            await Truugo.EDIFACT(input, connection, options, cts.Token);
        }
        catch (Exception e)
        {
            ex = e;
        }

        TestContext.WriteLine($"CancellationTokenShouldCancelRequest() exception type: {ex?.GetType().FullName}");
        TestContext.WriteLine($"CancellationTokenShouldCancelRequest() exception message: {ex?.Message}");
        Assert.That(ex, Is.Not.Null, "Expected an exception to be thrown");
        Assert.That(ex, Is.TypeOf<TaskCanceledException>());
    }

    [Test]
    public async Task SuccessfulRequestShouldHaveStatusCode200()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.CheckSyntax,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.EDIFACT(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"SuccessfulRequestShouldHaveStatusCode200() response: {result.StatusCode}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}
