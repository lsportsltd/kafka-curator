using AutoFixture;
using FluentAssertions;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KafkaCurator.UnitTests.ConfigurationBuilders;

[TestClass]
public class StateManagerConfigurationBuilderTests
{
    private readonly Fixture _fixture = new(); 
        
    private StateManagerConfigurationBuilder _builder;
    private Mock<IDependencyConfigurator> _dependencyConfiguratorMock;
    
    [TestInitialize]
    public void Setup()
    {
        _dependencyConfiguratorMock = new Mock<IDependencyConfigurator>();
        _builder = new StateManagerConfigurationBuilder(_dependencyConfiguratorMock.Object);
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
        //Act
        var stateManagerConfiguration = _builder.Build();
        
        //Assert
        stateManagerConfiguration.Name.Should().NotBeEmpty().Should().NotBeNull();
        stateManagerConfiguration.Timeout.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(30));
        stateManagerConfiguration.Type.Should().BeNull();
    }

    [TestMethod]
    public void Build_AllCalls_ReturnPassedValues()
    {
        //Arrange
        var name = _fixture.Create<string>();
        var timeout = _fixture.Create<TimeSpan>();

        _builder.WithName(name)
            .WithTimeout(timeout);

        //Act
        var stateManagerConfiguration = _builder.Build();

        //Assert
        stateManagerConfiguration.Name.Should().BeEquivalentTo(name);
        stateManagerConfiguration.Timeout.Should().BeGreaterOrEqualTo(timeout);
    }
}