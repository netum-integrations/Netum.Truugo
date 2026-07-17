using System;
using dotenv.net;
using Netum.Truugo.Validator.Definitions;

namespace Netum.Truugo.Validator.Tests;

internal abstract class TestBase
{
    internal TestBase()
    {
        DotEnv.Load();
        TruugoAPIUsername = GetEnvVar("TRUUGO_API_USERNAME");
        TruugoAPIPassword = GetEnvVar("TRUUGO_API_PASSWORD");
        TruugoAPIProfileKey = GetEnvVar("TRUUGO_API_PROFILE_KEY");
        TestFileName = GetEnvVar("VALIDATOR_TEST_FILE_NAME");
    }

    protected string TruugoAPIUsername { get; set; }

    protected string TruugoAPIPassword { get; set; }

    protected string TruugoAPIProfileKey { get; set; }

    protected string TestFileName { get; set; }

    protected static Input DefaultInput() => new();

    protected static Connection DefaultConnection() => new();

    protected static Options DefaultOptions() => new();

    private static string GetEnvVar(string name) => Environment.GetEnvironmentVariable(name) ??
                                                    throw new InvalidOperationException(
                                                        $"Missing required env var: {name}");
}
