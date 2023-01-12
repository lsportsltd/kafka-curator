using Amazon.Extensions.Configuration.SystemsManager;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;

namespace LSports.Kafka.Curator.Extensions
{
    public static class SsmExtensions
    {
        public static IConfigurationBuilder AddAwsSsm(this IConfigurationBuilder configBuilder)
        {
            return configBuilder.AddSystemsManager(source =>
            {
                var config = configBuilder.Build();
                var path = config["KafkaSsmPath"];

                source.Path = path;
                source.ParameterProcessor = new KafkaCuratorParameterProcessor();
            });
        }
    }

    public class KafkaCuratorParameterProcessor : DefaultParameterProcessor
    {
        public override string GetKey(Parameter parameter, string path)
        {
            return parameter.Name.TrimStart('/').Replace("/", KeyDelimiter).ToUpper();
        }
    }
}