using System;
using dotenv.net;
using Netum.Truugo.Schematron.Definitions;

namespace Netum.Truugo.Schematron.Tests;

internal abstract class TestBase
{
    internal TestBase()
    {
        DotEnv.Load();
        TruugoAPIUsername = GetEnvVar("TRUUGO_API_USERNAME");
        TruugoAPIPassword = GetEnvVar("TRUUGO_API_PASSWORD");
        TruugoAPIGroupKey = GetEnvVar("TRUUGO_API_GROUP_KEY");
        TruugoAPIItemKey = GetEnvVar("TRUUGO_API_ITEM_KEY");
        TruugoAPIFileKey = GetEnvVar("TRUUGO_API_FILE_KEY");
        TestFileName = GetEnvVar("SCHEMATRON_TEST_FILE_NAME");
    }

    protected string TruugoAPIUsername { get; set; }

    protected string TruugoAPIPassword { get; set; }

    protected string TruugoAPIGroupKey { get; set; }

    protected string TruugoAPIItemKey { get; set; }

    protected string TruugoAPIFileKey { get; set; }

    protected string TestFileName { get; set; }

    protected static Input DefaultInput() => new();

    protected static Connection DefaultConnection() => new();

    protected static Options DefaultOptions() => new();

    private static string GetEnvVar(string name) => Environment.GetEnvironmentVariable(name) ??
                                                    throw new InvalidOperationException(
                                                        $"Missing required env var: {name}");
}
