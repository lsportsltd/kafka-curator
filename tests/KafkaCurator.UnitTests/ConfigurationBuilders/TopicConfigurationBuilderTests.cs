using AutoFixture;
using FluentAssertions;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KafkaCurator.UnitTests.ConfigurationBuilders;

[TestClass]
public class TopicConfigurationBuilderTests
{
    private readonly Fixture _fixture = new(); 
        
    private TopicConfigurationBuilder _builder;
    private Mock<IDependencyConfigurator> _dependencyConfiguratorMock;
    
    [TestInitialize]
    public void Setup()
    {
        _dependencyConfiguratorMock = new Mock<IDependencyConfigurator>();
        _builder = new TopicConfigurationBuilder(_dependencyConfiguratorMock.Object);
    }
    
    [TestMethod]
    public void DependencyConfigurator_SetProperty_ReturnPassedInstance()
    {
        // Assert
        _builder.DependencyConfigurator.Should().Be(_dependencyConfiguratorMock.Object);
    }

    [TestMethod]
    public void Build_RequiredCalls_ReturnDefaultValues()
    {
        //Arrange
        var clusterConfiguration = _fixture.Create<ClusterConfiguration>();

        var topic = _fixture.Create<string>();
        _builder.Name(topic);
        
        //Act
        var configuration = _builder.Build(clusterConfiguration);
        
        //Assert
        configuration.Name.Should().BeEquivalentTo(topic);
        configuration.Partitions.Should().BeNull();
        configuration.Preallocate.Should().BeNull();
        configuration.CleanupPolicy.Should().BeNull();
        configuration.CompressionType.Should().BeNull();
        configuration.FlushMessages.Should().BeNull();
        configuration.FlushMs.Should().BeNull();
        configuration.ReplicationFactor.Should().BeNull();
        configuration.RetentionBytes.Should().BeNull();
        configuration.RetentionMs.Should().BeNull();
        configuration.SegmentBytes.Should().BeNull();
        configuration.SegmentMs.Should().BeNull();
        configuration.DeleteRetentionMs.Should().BeNull();
        configuration.IndexIntervalBytes.Should().BeNull();
        configuration.MaxMessageBytes.Should().BeNull();
        configuration.MessageFormatVersion.Should().BeNull();
        configuration.MessageTimestampType.Should().BeNull();
        configuration.SegmentIndexBytes.Should().BeNull();
        configuration.SegmentJitterMs.Should().BeNull();
        configuration.FileDeleteDelayMs.Should().BeNull();
        configuration.FollowerReplicationThrottledReplicas.Should().BeNull();
        configuration.MaxCompactionLagMs.Should().BeNull();
        configuration.MessageDownConversionEnable.Should().BeNull();
        configuration.MinCleanableDirtyRation.Should().BeNull();
        configuration.MinCompactionLagMs.Should().BeNull();
        configuration.MinInSyncReplicas.Should().BeNull();
        configuration.UncleanLeaderElectionEnable.Should().BeNull();
        configuration.MessageTimestampDifferenceMaxMs.Should().BeNull();
    }

    [TestMethod]
    public void Build_AllCalls_ReturnPassedValues()
    {
        //Arrange
        var clusterConfiguration = _fixture.Create<ClusterConfiguration>();

        var topic = _fixture.Create<string>();
        var partitions = _fixture.Create<int>();
        var replication = _fixture.Create<short>();
        var cleanupPolicy = _fixture.Create<string>();
        var compression = _fixture.Create<string>();
        var messageDownConversion = _fixture.Create<bool>();
        var minInSyncReplicas = _fixture.Create<int>();
        var segmentJitterMs = _fixture.Create<long>();
        var flushMs = _fixture.Create<long>();
        var followerReplicationThrottledReplicas = _fixture.Create<string>();
        var segmentBytes = _fixture.Create<int>();
        var retentionMs = _fixture.Create<long>();
        var flushMessages = _fixture.Create<long>();
        var messageFormatVersion = _fixture.Create<string>();
        var maxCompactionLagMs = _fixture.Create<long>();
        var fileDeleteDelayMs = _fixture.Create<long>();
        var maxMessageBytes = _fixture.Create<int>();
        var minCompactionLagMs = _fixture.Create<long>();
        var messageTimestampType = _fixture.Create<string>();
        var preallocate = _fixture.Create<bool>();
        var minCleanableDirtyRation = _fixture.Create<double>();
        var indexIntervalBytes = _fixture.Create<int>();
        var uncleanLeaderElectionEnable = _fixture.Create<bool>();
        var retentionBytes = _fixture.Create<long>();
        var deleteRetentionMs = _fixture.Create<long>();
        var segmentMs = _fixture.Create<long>();
        var messageTimestampDifferenceMaxMs = _fixture.Create<long>();
        var segmentIndexBytes = _fixture.Create<int>();
        
        _builder.Name(topic);
        _builder.WithNumberOfPartitions(partitions);
        _builder.WithReplicationFactory(replication);
        _builder.WithCleanupPolicy(cleanupPolicy);
        _builder.WithCompression(compression);
        _builder.WithMessageDownConversionEnable(messageDownConversion);
        _builder.WithMinInSyncReplicas(minInSyncReplicas);
        _builder.WithSegmentJitterMs(segmentJitterMs);
        _builder.WithFlushMs(flushMs);
        _builder.WithFollowerReplicationThrottledReplicas(followerReplicationThrottledReplicas);
        _builder.WithSegmentBytes(segmentBytes);
        _builder.WithRetentionMs(retentionMs);
        _builder.WithFlushMessages(flushMessages);
        _builder.WithMessageFormatVersion(messageFormatVersion);
        _builder.WithMaxCompactionLagMs(maxCompactionLagMs);
        _builder.WithFileDeleteDelayMs(fileDeleteDelayMs);
        _builder.WithMaxMessageBytes(maxMessageBytes);
        _builder.WithMinCompactionLagMs(minCompactionLagMs);
        _builder.WithMessageTimestampType(messageTimestampType);
        _builder.WithPreallocate(preallocate);
        _builder.WithMinCleanableDirtyRation(minCleanableDirtyRation);
        _builder.WithIndexIntervalBytes(indexIntervalBytes);
        _builder.WithUncleanLeaderElectionEnable(uncleanLeaderElectionEnable);
        _builder.WithRetentionBytes(retentionBytes);
        _builder.WithDeleteRetentionMs(deleteRetentionMs);
        _builder.WithSegmentMs(segmentMs);
        _builder.WithMessageTimestampDifferenceMaxMs(messageTimestampDifferenceMaxMs);
        _builder.WithSegmentIndexBytes(segmentIndexBytes);
        
        //Act
        var configuration = _builder.Build(clusterConfiguration);
        
        //Assert
        configuration.Name.Should().BeEquivalentTo(topic);
        configuration.Partitions.Should().BeGreaterOrEqualTo(partitions);
        configuration.Preallocate.Should().BeEquivalentTo(preallocate.ToString());
        configuration.CleanupPolicy.Should().BeEquivalentTo(cleanupPolicy);
        configuration.CompressionType.Should().BeEquivalentTo(compression);
        configuration.FlushMessages.Should().BeEquivalentTo(flushMessages.ToString());
        configuration.FlushMs.Should().BeEquivalentTo(flushMs.ToString());
        configuration.ReplicationFactor.Should().BeGreaterOrEqualTo(replication);
        configuration.RetentionBytes.Should().BeEquivalentTo(retentionBytes.ToString());
        configuration.RetentionMs.Should().BeEquivalentTo(retentionMs.ToString());
        configuration.SegmentBytes.Should().BeEquivalentTo(segmentBytes.ToString());
        configuration.SegmentMs.Should().BeEquivalentTo(segmentMs.ToString());
        configuration.DeleteRetentionMs.Should().BeEquivalentTo(deleteRetentionMs.ToString());
        configuration.IndexIntervalBytes.Should().BeEquivalentTo(indexIntervalBytes.ToString());
        configuration.MaxMessageBytes.Should().BeEquivalentTo(maxMessageBytes.ToString());
        configuration.MessageFormatVersion.Should().BeEquivalentTo(messageFormatVersion);
        configuration.MessageTimestampType.Should().BeEquivalentTo(messageTimestampType);
        configuration.SegmentIndexBytes.Should().BeEquivalentTo(segmentIndexBytes.ToString());
        configuration.SegmentJitterMs.Should().BeEquivalentTo(segmentJitterMs.ToString());
        configuration.FileDeleteDelayMs.Should().BeEquivalentTo(fileDeleteDelayMs.ToString());
        configuration.FollowerReplicationThrottledReplicas.Should().BeEquivalentTo(followerReplicationThrottledReplicas);
        configuration.MaxCompactionLagMs.Should().BeEquivalentTo(maxCompactionLagMs.ToString());
        configuration.MessageDownConversionEnable.Should().BeEquivalentTo(messageDownConversion.ToString());
        configuration.MinCleanableDirtyRation.Should().BeEquivalentTo(minCleanableDirtyRation.ToString());
        configuration.MinCompactionLagMs.Should().BeEquivalentTo(minCompactionLagMs.ToString());
        configuration.MinInSyncReplicas.Should().BeEquivalentTo(minInSyncReplicas.ToString());
        configuration.UncleanLeaderElectionEnable.Should().BeEquivalentTo(uncleanLeaderElectionEnable.ToString());
        configuration.MessageTimestampDifferenceMaxMs.Should().BeEquivalentTo(messageTimestampDifferenceMaxMs.ToString());
    }
}