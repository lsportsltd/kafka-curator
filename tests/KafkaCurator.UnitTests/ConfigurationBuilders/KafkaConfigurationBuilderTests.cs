using KafkaCurator.Configuration;
using KafkaCurator.LogHandler.Microsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KafkaCurator.UnitTests.ConfigurationBuilders;

[TestClass]
public class KafkaConfigurationBuilderTests
{
    [TestMethod]
    public void ExtensionMethod_UseMicrosoftLog_ConfigureMicrosoftLogHandler()
    {
        //Arrange
        var builder = new Mock<IKafkaConfigurationBuilder>();
        
        //Act
        builder.Object.UseMicrosoftLog();
        
        //Assert
        builder.Verify(x => x.UseLogHandler<MicrosoftLogHandler>(), Times.Once);
    }
}