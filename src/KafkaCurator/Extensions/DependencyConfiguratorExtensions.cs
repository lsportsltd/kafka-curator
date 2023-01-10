using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Extensions;
using KafkaCurator.Changes.TopicAltering;
using KafkaCurator.Changes.TopicAltering.Handlers;

namespace KafkaCurator.Extensions
{
    internal static class DependencyConfiguratorExtensions
    {
        internal static IDependencyConfigurator AddTopicAlterServices(this IDependencyConfigurator configurator)
        {
            configurator.AddSingleton<ITopicAlterFactory, TopicAlterFactory>();

            configurator.AddSingleton<ITopicAlterHandler, CleanupPolicyAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, CompressionTypeAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MessageDownConversionEnableAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MinInSyncReplicasAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, SegmentJitterMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, FlushMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, FollowerReplicationThrottledReplicasAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, SegmentBytesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, RetentionMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, FlushMessagesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MessageFormatVersionAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MaxCompactionLagMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MaxMessageBytesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MinCompactionLagMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MessageTimestampTypeAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, PreallocateAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MinCleanableDirtyRationAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, IndexIntervalBytesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, UncleanLeaderElectionEnableAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, RetentionBytesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, DeleteRetentionMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, SegmentMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, MessageTimestampDifferenceMaxMsAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, SegmentIndexBytesAlterHandler>();
            configurator.AddSingleton<ITopicAlterHandler, FileDeleteDelayMsAlterHandler>();
            
            return configurator;
        }
    }
}