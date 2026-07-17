using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Schematron.Definitions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

#nullable enable

namespace Netum.Truugo.Schematron.Tests;

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
    public async Task ValidateShouldReturnSVRLReportWithNoFailures()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FilePath,
            FilePath = pathToTestFile,
            FileKey = TruugoAPIFileKey,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ValidateShouldReturnSVRLReportWithNoFailures() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("svrl:schematron-output").IgnoreCase);
        Assert.That(result.Data.ToString(), Does.Not.Contain("svrl:failed-assert").IgnoreCase);
    }

    [Test]
    public async Task ListItemsShouldReturnItemKey()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.ListItems,
            GroupKey = TruugoAPIGroupKey,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ListItemsShouldReturnItemKey() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("item_key").IgnoreCase);
    }

    [Test]
    public async Task ListItemVersionsShouldReturnFileKey()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.ListItemVersions,
            ItemKey = TruugoAPIItemKey,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ListItemVersionsShouldReturnFileKey() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("file_key").IgnoreCase);
    }

    [Test]
    public async Task FileImportTypeShouldBeRespected()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FileContent,
            FileKey = TruugoAPIFileKey,
            FilePath = pathToTestFile,
            FileName = "valid_finvoice.xml",
            Content = validContent,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"FileImportTypeShouldBeRespected() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("svrl:schematron-output").IgnoreCase);
        Assert.That(result.Data.ToString(), Does.Not.Contain("svrl:failed-assert").IgnoreCase); // If invalid XML was used, there would be failed assertions in the report
    }

    [Test]
    public async Task InvalidFilePathShouldReturnError()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FilePath,
            FileKey = TruugoAPIFileKey,
            FilePath = "./TestFiles/non_existent_file.xml", // Invalid file path
        };

        // Set ThrowErrorOnFailure to false to get error details in the response instead of an exception
        var options = new Options
        {
            ThrowErrorOnFailure = false,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"InvalidFilePathShouldReturnError() response: {result.Data}, error: {result.Error?.Message}");

        ClassicAssert.IsFalse(result.Success);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Message, Does.Contain("Could not find file").IgnoreCase);
    }

    [Test]
    public async Task InvalidFileContentShouldReturnSVRLReportWithFailures()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FileContent,
            FileKey = TruugoAPIFileKey,
            FileName = "invalid_finvoice.xml",
            Content = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><?xml-stylesheet href=\"Finvoice.xsl\" type=\"text/xsl\"?><!-- Generated by Truugo (https://www.truugo.com) 2018-12-10 --><Finvoice Version=\"3.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"Finvoice3.0.xsd\"></Finvoice>",
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"InvalidFileContentShouldReturnSVRLReportWithFailures() response: {result.Data}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("svrl:schematron-output").IgnoreCase);
        Assert.That(result.Data.ToString(), Does.Contain("svrl:failed-assert").IgnoreCase);
    }

    [Test]
    public async Task CancellationTokenShouldCancelRequest()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.xml",
            Content = validContent,
        };

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100); // Cancel after 100ms

        Exception? ex = null;
        try
        {
            await Truugo.Schematron(input, connection, options, cts.Token);
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
            Endpoint = EndpointPath.Validate,
            FileImportType = FileImport.FileContent,
            FileKey = TruugoAPIFileKey,
            FileName = "testfile.xml",
            Content = validContent,
        };

        var result = await Truugo.Schematron(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"SuccessfulRequestShouldHaveStatusCode200() status code: {result.StatusCode}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}