using System;
using System.Runtime.Remoting.Messaging;
using Common.Logging;
using Moq;
using NUnit.Framework;
using PboLib;

namespace PboLibTests
{
    [TestFixture]
    public class PboFileFactoryTests
    {
        [Test]
        public void CreatePboFile_Should_Return_Mock_Instance_When_Mock_Is_Provided()
        {
            // Arrange
            var mockLogger = new Mock<ILog>(MockBehavior.Strict);
            var mockFile = new Mock<IPboFile>(MockBehavior.Strict);
            PboFileFactory.MockPboFile = mockFile.Object;

            // Act
            var pboFile = PboFileFactory.CreatePboFile(PboFormat.Unknown, mockLogger.Object);

            // Assert
            Assert.AreSame(mockFile.Object, pboFile);
        }

        [Test]
        public void CreatePboFile_Should_Return_Instance_When_Mock_Is_Not_Provided()
        {
            // Arrange
            var mockLogger = new Mock<ILog>(MockBehavior.Strict);

            // Act
            var pboFile = PboFileFactory.CreatePboFile(PboFormat.Arma3, mockLogger.Object);

            // Assert
            Assert.IsNotNull(pboFile, "Did not get instance of pbo file.");
        }

        [Test]
        public void CreatePboFile_Should_Throw_Exception_When_Format_Is_Unknown()
        {
            // Arrange
            var mockLogger = new Mock<ILog>(MockBehavior.Strict);

            // Act
            try
            {
                PboFileFactory.CreatePboFile(PboFormat.Unknown, mockLogger.Object);
            }
            catch (ArgumentException ex)
            {
                // Assert
                Assert.AreEqual("The format \"Unknown\" could not be created.", ex.Message);
            }
        }
    }
}