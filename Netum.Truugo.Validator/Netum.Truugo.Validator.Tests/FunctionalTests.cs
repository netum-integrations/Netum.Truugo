using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Validator.Definitions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

#nullable enable

namespace Netum.Truugo.Validator.Tests;

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
            ProfileKey = TruugoAPIProfileKey,
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
    public async Task InvalidContentShouldReturnFailedResult()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = "this is not valid content",
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"InvalidContentShouldReturnFailedResult() API response: {result.Data}");
        TestContext.WriteLine($"InvalidContentShouldReturnFailedResult() Success result: {result.Success}");

        Assert.That(result.Data.ToString(), Does.Contain("FAILED"));
    }

    [Test]
    public async Task GetStatusShouldReturnValid_withContent()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"GetStatusShouldReturnValid_withContent() API response: {result.Data}");
        TestContext.WriteLine($"GetStatusShouldReturnValid_withContent() Success result: {result.Success}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data.ToString(), Does.Contain("VALID"));
    }

    [Test]
    public async Task GetStatusShouldReturnValid_withFilePath()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FilePath,
            FilePath = pathToTestFile,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"GetStatusShouldReturnValid_withFilePath() API response: {result.Data}");
        TestContext.WriteLine($"GetStatusShouldReturnValid_withFilePath() Success result: {result.Success}");

        ClassicAssert.IsTrue(result.Success);
        Assert.That(result.Data.ToString(), Does.Contain("VALID"));
    }

    [Test]
    public async Task InstanceNameShouldOverrideFileName()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetFeedback,
            FileImportType = FileImport.FilePath,
            FilePath = pathToTestFile,
            FileName = "testitiedosto123",
            InstanceName = "TestiNimi",
            ErrorsOnly = false,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"InstanceNameShouldOverrideFileName() API response: {result.Data}");
        TestContext.WriteLine($"InstanceNameShouldOverrideFileName() Success result: {result.Success}");

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data.ToString(), Does.Contain("VALID"));

        Assert.That(result.Data.ToString(), Does.Contain("TestiNimi"));
        Assert.That(result.Data.ToString(), Does.Not.Contain("testitiedosto123"));
    }

    [Test]
    public async Task ErrorsOnlyShouldNotReturnSequenceElements()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetFeedback,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
            ErrorsOnly = true,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"ErrorsOnlyShouldNotReturnSequenceElements() API response: {result.Data}");
        TestContext.WriteLine($"ErrorsOnlyShouldNotReturnSequenceElements() Success result: {result.Success}");

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data.ToString(), Does.Not.Contain("sequence")); // When ErrorsOnly is true, the response does not contain any sequence elements. Sequence 1, Sequence 2 etc. are only present when ErrorsOnly is false.
    }

    [Test]
    public async Task GetReportStorageTimeShouldBeRespected()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetReport,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
            StorageTimeInHours = 5,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"GetReportStorageTimeShouldBeRespected() API response: {result.Data}");

        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Success, Is.True);

        var metaExpiry = DateTimeOffset.Parse(result.Data["metaExpiry"].ToString());
        var fileExpiry = DateTimeOffset.Parse(result.Data["fileExpiry"].ToString());
        var expectedExpiry = metaExpiry.AddHours(input.StorageTimeInHours);

        Assert.That(
            fileExpiry.TimeOfDay,
            Is.EqualTo(expectedExpiry.TimeOfDay),
            $"Expected fileExpiry time-of-day to be ~{expectedExpiry.TimeOfDay:hh\\:mm\\:ss}, but was {fileExpiry.TimeOfDay:hh\\:mm\\:ss}");
    }

    [Test]
    public async Task InvalidStorageTimeShouldReturnError()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetReport,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
            StorageTimeInHours = 25, // Invalid storage time, should be between 1 and 24
        };

        var options = new Options
        {
            ThrowErrorOnFailure = false, // Set to false to get error details in the response instead of an exception
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"InvalidStorageTimeShouldReturnError() Success result: {result.Success}");
        TestContext.WriteLine($"InvalidStorageTimeShouldReturnError() API response: {result.Data}");

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(input.StorageTimeInHours, Is.TypeOf<int>());
        Assert.That(result.Error.Message, Does.Contain("Storage time must be greater than 0 and less than or equal to 24").IgnoreCase);
    }

    [Test]
    public async Task StoreStatus30dShouldReturnValid()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.StoreStatus30d,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"StoreStatus30dShouldReturnValid() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
    }

    [Test]
    public async Task StoreStatus60dShouldReturnValid()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.StoreStatus60d,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"StoreStatus60dShouldReturnValid() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
    }

    [Test]
    public async Task StoreReport7dShouldReturnValid()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.StoreReport7d,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"StoreReport7dShouldReturnValid() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
    }

    [Test]
    public async Task StoreReport14dShouldReturnValid()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.StoreReport14d,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"StoreReport14dShouldReturnValid() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
    }

    [Test]
    public async Task StoreReport30dShouldReturnValid()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.StoreReport30d,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"StoreReport30dShouldReturnValid() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
    }

    [Test]
    public async Task FileImportTypeShouldBeRespected_wFilePath()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FilePath,
            FilePath = pathToTestFile,
            Content = "invalid content", // This content should be ignored since FileImportType is FilePath
        };

        var options = new Options
        {
            ThrowErrorOnFailure = false, // Set to false to get error details in the response instead of an exception
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"FileImportTypeShouldBeRespected_wFilePath() Success result: {result.Success}");
        TestContext.WriteLine($"FileImportTypeShouldBeRespected_wFilePath() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("VALID"));
    }

    [Test]
    public async Task FileImportTypeShouldBeRespected_wContent()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetFeedback,
            FileImportType = FileImport.FileContent,
            Content = validContent,
            FileName = "testfile.txt",
            FilePath = pathToTestFile, // This file path should be ignored since FileImportType is FileContent
            ErrorsOnly = true,
        };

        var options = new Options
        {
            ThrowErrorOnFailure = false, // Set to false to get error details in the response instead of an exception
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"FileImportTypeShouldBeRespected_wContent() Success result: {result.Success}");
        TestContext.WriteLine($"FileImportTypeShouldBeRespected_wContent() API response: {result.Data}");

        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.ToString(), Does.Contain("VALID"));
        Assert.That(result.Data.ToString(), Does.Contain("testfile.txt"));
        Assert.That(result.Data.ToString(), Does.Not.Contain($"{TestFileName}")); // The file path should be ignored, so the response should not contain the file name from the file path.
    }

    [Test]
    public async Task CancellationTokenShouldCancelRequest()
    {
        var input = new Input
        {
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100); // Cancel after 100ms

        Exception? ex = null;
        try
        {
            await Truugo.Validator(input, connection, options, cts.Token);
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
            Endpoint = EndpointPath.GetStatus,
            FileImportType = FileImport.FileContent,
            FileName = "testfile.txt",
            Content = validContent,
        };

        var result = await Truugo.Validator(input, connection, options, CancellationToken.None);
        TestContext.WriteLine($"SuccessfulRequestShouldHaveStatusCode200() Status code: {result.StatusCode}");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}
