using System;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.ValidatorListProfiles.Definitions;
using NUnit.Framework;
using NUnit.Framework.Internal;

#nullable enable

namespace Netum.Truugo.ValidatorListProfiles.Tests;

[TestFixture]
internal class FunctionalTests : TestBase
{
    private Connection? connection;
    private Options? options;

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
    }

    [Test]
    public async Task ResultContainsAllProfileKeys()
    {
        var result = await Truugo.ValidatorListProfiles(connection, options, CancellationToken.None);

        for (int i = 0; i < (result.Profiles?.Count ?? 0); i++)
        {
            var profile = result.Profiles![i];
            TestContext.WriteLine($"ResultContainsAllProfileKeys() Profile [{i}]: Key={profile.ProfileKey}");
        }

        TestContext.WriteLine($"ResultContainsAllProfileKeys() Total profiles: {result.Profiles?.Count ?? 0}");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Profiles, Is.Not.Null);
        Assert.That(result.Profiles, Is.Not.Empty);
        Assert.That(result.Profiles![0].ProfileKey, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task CancellationTokenShouldCancelRequest()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(100); // Cancel after 100ms

        Exception? ex = null;
        try
        {
            await Truugo.ValidatorListProfiles(connection, options, cts.Token);
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
        var result = await Truugo.ValidatorListProfiles(connection, options, CancellationToken.None);

        TestContext.WriteLine($"SuccessfulRequestShouldHaveStatusCode200() Status Code: {result.StatusCode}");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}