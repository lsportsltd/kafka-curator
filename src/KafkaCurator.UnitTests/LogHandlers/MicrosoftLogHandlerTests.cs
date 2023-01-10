using Moq;
using KafkaCurator.LogHandler.Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KafkaCurator.UnitTests.LogHandlers;

[TestClass]
public class MicrosoftLogHandlerTests
{
    [TestMethod]
    public void Constructor_CreatesNamedLogger()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(x => x.CreateLogger("KafkaCurator")).Returns(loggerMock.Object);

        // Act
        new MicrosoftLogHandler(loggerFactoryMock.Object);

        // Assert
        loggerFactoryMock.Verify(x => x.CreateLogger("KafkaCurator"), Times.Once);
    }
}