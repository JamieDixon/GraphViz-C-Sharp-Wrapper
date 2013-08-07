using System.Diagnostics;
using GraphVizWrapper.Queries;
using Moq;
using NUnit.Framework;

namespace GraphVizWrapper.Tests
{
    using GraphVizWrapper.Commands;

    [TestFixture]
    public class Tests
    {
        private Mock<IRegisterLayoutPluginCommand> _registerLayoutPluginCommandMock;
        private Mock<IGetProcessStartInfoQuery> _getProcessStartInfoQuery;
        private IGetStartProcessQuery _getStartProcessQuery;
        
        [SetUp]
        public void Init()
        {
            _getProcessStartInfoQuery = new Mock<IGetProcessStartInfoQuery>();
            _registerLayoutPluginCommandMock = new Mock<IRegisterLayoutPluginCommand>();
            _getStartProcessQuery = new GetStartProcessQuery();
        }

        [Test]
        public void GenerateGraphReturnsByteArrayWithLengthGreaterOrEqualZero()
        {
            // Arrange
            _registerLayoutPluginCommandMock.Setup(m => m.Invoke());
            _getProcessStartInfoQuery.Setup(m => m.Invoke(It.IsAny<IProcessStartInfoWrapper>())).Returns(
                new ProcessStartInfo
                    {
                        FileName = "cmd",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });

            var wrapper = new GraphGeneration(
                _getStartProcessQuery,
                _getProcessStartInfoQuery.Object,
                _registerLayoutPluginCommandMock.Object);

            // Act
            byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Png);

            // Assert
            Assert.That(output.Length, Is.GreaterThanOrEqualTo(0));
        }
    }
}
