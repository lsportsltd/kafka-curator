using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaCurator.Core.Constants;
using KafkaCurator.Core.Enums;
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
                var errMessage = "Failed to find topics within configuration, aborting";
                _logger.LogInformation(errMessage);
                throw new Exception(errMessage);
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

        private bool ShouldAlterTopic(Topic topic, TopicMetadata topicMetadata)
        {
            if (topic.NumOfPartitions != topicMetadata.Partitions.Count)
            {
                return true;
            }

            if (topic.CleanupPolicy == CleanupPolicy.Delete || topic.CleanupPolicy == CleanupPolicy.Compact)
            {
                return true;
            }

            return false;
        }

        private Task HandleExistingTopics(Topic[] existingTopics, Dictionary<string, TopicMetadata> topicsMetadata)
        {
            try
            {
                if (existingTopics.Length == 0) return Task.CompletedTask;

                var topicsToAlter = new List<Topic>();

                foreach (var existingTopic in existingTopics)
                {
                    if (!ShouldAlterTopic(existingTopic, topicsMetadata[existingTopic.Name])) continue;

                    topicsToAlter.Add(existingTopic);
                }

                if (topicsToAlter.Count == 0)
                {
                    _logger.LogInformation("There are no existing topics to alter.");
                    return Task.CompletedTask;
                }

                _logger.LogInformation($"Found {topicsToAlter.Count} topics to alter.");

                return _kafkaClient.AlterTopicPartitionsAsync(topicsToAlter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred while handling existing topics!");
                return Task.CompletedTask;
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