using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaCurator.Core.Constants;
using KafkaCurator.Kafka;
using KafkaCurator.Kafka.Interfaces;
using KafkaCurator.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaCurator
{
    public class KafkaCuratorHostedService : IHostedService
    {
        private readonly ILogger<KafkaCuratorHostedService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly IKafkaClient _kafkaClient;

        public KafkaCuratorHostedService(ILogger<KafkaCuratorHostedService> logger, IHostApplicationLifetime applicationLifetime, IConfiguration configuration)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _configuration = configuration;

            var bootstrapServers = _configuration[Endpoints.KafkaBootstrapServers];
            _kafkaClient = new KafkaClient(bootstrapServers);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var configTopics = _configuration.GetSection("topics").Get<Topic[]>();
            var topicMetadata = _kafkaClient.GetKafkaTopics();

            if (configTopics == null)
            {
                configTopics = Array.Empty<Topic>();
            }

            _logger.LogInformation($"Found {configTopics.Length} topics within configuration...");
            _logger.LogInformation($"Found {topicMetadata.Count} topics within Kafka...");

            var existingTopics = configTopics.Where(ct => topicMetadata.ContainsKey(ct.Name));
            await HandleExistingTopics(existingTopics.ToArray(), topicMetadata);

            var newTopics = configTopics.Where(ct => !topicMetadata.ContainsKey(ct.Name));
            await HandleNewTopics(newTopics.ToArray());

            var topicsToDelete = topicMetadata.Keys.Except(configTopics.Select(ct => ct.Name));
            await HandleTopicsToDelete(topicsToDelete.ToArray());

            _logger.LogInformation("Finished successfully, stopping kafka curator...");
            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<(bool, AlterTopicInfo)> ShouldAlterTopic(Topic topic, TopicMetadata topicMetadata)
        {
            var result = false;
            var alterInfo = new AlterTopicInfo();

            if (topic.NumOfPartitions != topicMetadata.Partitions.Count)
            {
                result = true;
                alterInfo.ShouldAlterNumOfPartitions = true;
            }

            var configs = await _kafkaClient.DescribeTopicConfigAsync(topic.Name);
            if (configs == null) return (result, alterInfo);

            var cleanupPolicy = topic.CleanupPolicy.ToString().ToLower();
            if (configs.Entries.TryGetValue("cleanup.policy", out var entry))
            {
                if (entry.Value != cleanupPolicy)
                {
                    result = true;
                    alterInfo.ShouldAlterCleanupPolicy = true;
                }
            }

            return (result, alterInfo);
        }

        private async Task HandleExistingTopics(Topic[] existingTopics, Dictionary<string, TopicMetadata> topicsMetadata)
        {
            try
            {
                if (existingTopics.Length == 0) return;

                var topicsToAlter = new List<(Topic, AlterTopicInfo)>();

                foreach (var existingTopic in existingTopics)
                {
                    var (result, alterInfo) = await ShouldAlterTopic(existingTopic, topicsMetadata[existingTopic.Name]);
                    if (!result) continue;

                    topicsToAlter.Add((existingTopic, alterInfo));
                }

                if (topicsToAlter.Count == 0)
                {
                    _logger.LogInformation("There are no existing topics to alter.");
                    return;
                }

                _logger.LogInformation($"Found {topicsToAlter.Count} topics to alter.");

                await _kafkaClient.AlterTopics(topicsToAlter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred while handling existing topics!");
            }
        }

        private Task HandleNewTopics(Topic[] newTopics)
        {
            try
            {
                if (newTopics.Length == 0)
                {
                    _logger.LogInformation("There are no new topics to create.");
                    return Task.CompletedTask;
                }

                return _kafkaClient.CreateTopicsAsync(newTopics);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred while handling new topics!");
                return Task.CompletedTask;
            }
        }

        private Task HandleTopicsToDelete(string[] topics)
        {
            try
            {
                if (topics.Length == 0)
                {
                    _logger.LogInformation("There are no topics to delete.");
                    return Task.CompletedTask;
                }

                return _kafkaClient.DeleteTopicAsync(topics);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred while handling topics to delete!");
                return Task.CompletedTask;
            }
        }
    }
}