using KafkaCurator.Configuration;
using LSports.Kafka.Curator.Constants;
using LSports.Kafka.Curator.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;

var env = Environment.GetEnvironmentVariable(EnvironmentVariableName.HostEnvironment);
var services = new ServiceCollection();

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", false)
    .AddAwsSsm()
    .Build();

CuratorClusterCreator.CreateBasedOnEvn(env, services, config);

var runConfig = new RunConfiguration
{
    DryRun = args.Contains("--dry-run")
};

var provider = services.BuildServiceProvider();
var curator = provider.CreateCurator();

return await curator.RunAsync(runConfig);