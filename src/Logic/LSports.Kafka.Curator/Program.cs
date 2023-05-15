using KafkaCurator.Configuration;
using LSports.Kafka.Curator.Constants;
using LSports.Kafka.Curator.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;

var env = Environment.GetEnvironmentVariable(EnvironmentVariableName.HostEnvironment);

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", false)
    .AddAwsSsm()
    .Build();

var services = new ServiceCollection();
services.AddKafkaCuratorBasedOnEnv(env,config);

var provider = services.BuildServiceProvider();
var curator = provider.CreateCurator();

var runConfig = new RunConfiguration
{
    DryRun = args.Contains("--dry-run")
};

return await curator.RunAsync(runConfig);