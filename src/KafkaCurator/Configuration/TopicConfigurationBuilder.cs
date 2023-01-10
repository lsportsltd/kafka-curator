using System;
using KafkaCurator.Abstractions;

namespace KafkaCurator.Configuration
{
    public class TopicConfigurationBuilder : ITopicConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private string _topicName;
        private int? _numberOfPartitions;
        private short? _replicationFactor;
        private string _cleanupPolicy;
        private string _compressionType;
        private bool? _messageDownConversionEnable;
        private int? _minInSyncReplicas;
        private long? _segmentJitterMs;
        private long? _flushMs;
        private string _followerReplicationThrottledReplicas;
        private int? _segmentBytes;
        private long? _retentionMs;
        private long? _flushMessages;
        private string _messageFormatVersion;
        private long? _maxCompactionLagMs;
        private long? _fileDeleteDelayMs;
        private int? _maxMessageBytes;
        private long? _minCompactionLagMs;
        private string _messageTimestampType;
        private bool? _preallocate;
        private double? _minCleanableDirtyRation;
        private int? _indexIntervalBytes;
        private bool? _uncleanLeaderElectionEnable;
        private long? _retentionBytes;
        private long? _deleteRetentionMs;
        private long? _segmentMs;
        private int? _segmentIndexBytes;
        private long? _messageTimestampDifferenceMaxMs;

        public TopicConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public ITopicConfigurationBuilder Name(string topicName)
        {
            _topicName = topicName;
            return this;
        }

        public ITopicConfigurationBuilder WithNumberOfPartitions(int numberOfPartitions)
        {
            _numberOfPartitions = numberOfPartitions;
            return this;
        }

        public ITopicConfigurationBuilder WithReplicationFactory(short replicationFactor)
        {
            _replicationFactor = replicationFactor;
            return this;
        }

        public ITopicConfigurationBuilder WithCleanupPolicy(string policy)
        {
            _cleanupPolicy = policy;
            return this;
        }

        public ITopicConfigurationBuilder WithCompression(string compression)
        {
            _compressionType = compression;
            return this;
        }

        public ITopicConfigurationBuilder WithMessageDownConversionEnable(bool enable)
        {
            _messageDownConversionEnable = enable;
            return this;
        }

        public ITopicConfigurationBuilder WithMinInSyncReplicas(int inSyncReplicas)
        {
            _minInSyncReplicas = inSyncReplicas;
            return this;
        }

        public ITopicConfigurationBuilder WithSegmentJitterMs(long segmentJitterMs)
        {
            _segmentJitterMs = segmentJitterMs;
            return this;
        }

        public ITopicConfigurationBuilder WithFlushMs(long flushMs)
        {
            _flushMs = flushMs;
            return this;
        }

        public ITopicConfigurationBuilder WithFollowerReplicationThrottledReplicas(string list)
        {
            _followerReplicationThrottledReplicas = list;
            return this;
        }

        public ITopicConfigurationBuilder WithSegmentBytes(int segmentBytes)
        {
            _segmentBytes = segmentBytes;
            return this;
        }

        public ITopicConfigurationBuilder WithRetentionMs(long retentionMs)
        {
            _retentionMs = retentionMs;
            return this;
        }

        public ITopicConfigurationBuilder WithFlushMessages(long flushMessages)
        {
            _flushMessages = flushMessages;
            return this;
        }

        public ITopicConfigurationBuilder WithMessageFormatVersion(string messageFormatVersion)
        {
            _messageFormatVersion = messageFormatVersion;
            return this;
        }

        public ITopicConfigurationBuilder WithMaxCompactionLagMs(long maxCompactionLagMs)
        {
            _maxCompactionLagMs = maxCompactionLagMs;
            return this;
        }

        public ITopicConfigurationBuilder WithFileDeleteDelayMs(long fileDeleteDelayMs)
        {
            _fileDeleteDelayMs = fileDeleteDelayMs;
            return this;
        }

        public ITopicConfigurationBuilder WithMaxMessageBytes(int maxMessageBytes)
        {
            _maxMessageBytes = maxMessageBytes;
            return this;
        }

        public ITopicConfigurationBuilder WithMinCompactionLagMs(long minCompactionLagMs)
        {
            _minCompactionLagMs = minCompactionLagMs;
            return this;
        }

        public ITopicConfigurationBuilder WithMessageTimestampType(string messageTimestampType)
        {
            _messageTimestampType = messageTimestampType;
            return this;
        }

        public ITopicConfigurationBuilder WithPreallocate(bool preallocate)
        {
            _preallocate = preallocate;
            return this;
        }

        public ITopicConfigurationBuilder WithMinCleanableDirtyRation(double minCleanableDirtyRation)
        {
            _minCleanableDirtyRation = minCleanableDirtyRation;
            return this;
        }

        public ITopicConfigurationBuilder WithIndexIntervalBytes(int indexIntervalBytes)
        {
            _indexIntervalBytes = indexIntervalBytes;
            return this;
        }

        public ITopicConfigurationBuilder WithUncleanLeaderElectionEnable(bool uncleanLeaderElectionEnable)
        {
            _uncleanLeaderElectionEnable = uncleanLeaderElectionEnable;
            return this;
        }

        public ITopicConfigurationBuilder WithRetentionBytes(long retentionBytes)
        {
            _retentionBytes = retentionBytes;
            return this;
        }

        public ITopicConfigurationBuilder WithDeleteRetentionMs(long deleteRetentionMs)
        {
            _deleteRetentionMs = deleteRetentionMs;
            return this;
        }

        public ITopicConfigurationBuilder WithSegmentMs(long segmentMs)
        {
            _segmentMs = segmentMs;
            return this;
        }

        public ITopicConfigurationBuilder WithMessageTimestampDifferenceMaxMs(long messageTimestampDifferenceMaxMs)
        {
            _messageTimestampDifferenceMaxMs = messageTimestampDifferenceMaxMs;
            return this;
        }

        public ITopicConfigurationBuilder WithSegmentIndexBytes(int segmentIndexBytes)
        {
            _segmentIndexBytes = segmentIndexBytes;
            return this;
        }

        public ITopicConfiguration Build(ClusterConfiguration clusterConfiguration)
        {
            SanityCheck();
            
            return new TopicConfiguration(_topicName)
            {
                Partitions = _numberOfPartitions,
                ReplicationFactor = _replicationFactor,
                CleanupPolicy = _cleanupPolicy,
                CompressionType = _compressionType,
                Preallocate = _preallocate?.ToString(),
                FlushMessages = _flushMessages?.ToString(),
                FlushMs = _flushMs?.ToString(),
                RetentionBytes = _retentionBytes?.ToString(),
                RetentionMs = _retentionMs?.ToString(),
                SegmentBytes = _segmentBytes?.ToString(),
                SegmentMs = _segmentMs?.ToString(),
                DeleteRetentionMs = _deleteRetentionMs?.ToString(),
                IndexIntervalBytes = _indexIntervalBytes?.ToString(),
                MaxMessageBytes = _maxMessageBytes?.ToString(),
                MessageFormatVersion = _messageFormatVersion,
                MessageTimestampType = _messageTimestampType,
                SegmentIndexBytes = _segmentIndexBytes?.ToString(),
                SegmentJitterMs = _segmentJitterMs?.ToString(),
                FileDeleteDelayMs = _fileDeleteDelayMs?.ToString(),
                FollowerReplicationThrottledReplicas = _followerReplicationThrottledReplicas,
                MaxCompactionLagMs = _maxCompactionLagMs?.ToString(),
                MessageDownConversionEnable = _messageDownConversionEnable?.ToString(),
                MinCleanableDirtyRation = _minCleanableDirtyRation?.ToString(),
                MinCompactionLagMs = _minCompactionLagMs?.ToString(),
                MinInSyncReplicas = _minInSyncReplicas?.ToString(),
                UncleanLeaderElectionEnable = _uncleanLeaderElectionEnable?.ToString(),
                MessageTimestampDifferenceMaxMs = _messageTimestampDifferenceMaxMs?.ToString()
            };
        }

        private void SanityCheck()
        {
            if (string.IsNullOrEmpty(_topicName)) throw new ArgumentNullException(nameof(_topicName));
            if (_numberOfPartitions <= 0) throw new ArgumentOutOfRangeException(nameof(_numberOfPartitions));
        }
    }
}