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
            _registerLayoutPluginCommandMock.Setup(m => m.Invoke("SomeLocation", Enums.RenderingEngine.Dot));
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

        [Test]
        public void DoesNotCrashWithLargeInput()
        {
            // Arrange
            var getProcessStartInfoQuerty = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuerty, _getStartProcessQuery);

            var wrapper = new GraphGeneration(
                _getStartProcessQuery,
                getProcessStartInfoQuerty,
                registerLayoutPluginCommand);

            // Act

            var diagraph =
                "digraph G {subgraph cluster_T1 { label = \"WORK ORDER\"; S1 [label = \"ACCEPTED\"]; S2 [label = \"DENIED\"]; S5 [label = \"SCHEDULED\"]; S44 [label = \"COMPLETE\"]; S47 [label = \"DELIVERED\"]; S48 [label = \"CANCELLED\"]; } subgraph cluster_T2 { label = \"MILL/DIG CHECK OUT\"; S3 [label = \"PROGRAMMING\"]; S4 [label = \"CUTTER PATH\"]; } subgraph cluster_T24 { label = \"OFFLOAD INTERNAL\"; S83 [label = \"IN PROCESS\"]; S84 [label = \"FINISHED\"]; } subgraph cluster_T23 { label = \"OFFLOAD EXTERNAL\"; S45 [label = \"IN PROCESS\"]; S46 [label = \"FINISHED\"]; S82 [label = \"QUOTE PENDING\"]; } subgraph cluster_T1 { label = \"WORK ORDER\"; S49 [label = \"WAITING 5-AXIS\"]; S50 [label = \"WAITING DATA CHANGE\"]; } subgraph cluster_T8 { label = \"MILL 1\"; S15 [label = \"IN PROCESS\"]; S16 [label = \"FINISHED\"]; } subgraph cluster_T9 { label = \"MILL 2\"; S17 [label = \"IN PROCESS\"]; S18 [label = \"FINISHED\"]; } subgraph cluster_T10 { label = \"MILL 3\"; S20 [label = \"IN PROCESS\"]; S21 [label = \"FINISHED\"]; } subgraph cluster_T11 { label = \"MILL 4\"; S22 [label = \"IN PROCESS\"]; S23 [label = \"FINISHED\"]; } subgraph cluster_T12 { label = \"MILL 5\"; S24 [label = \"IN PROCESS\"]; S25 [label = \"FINISHED\"]; } subgraph cluster_T13 { label = \"MILL 6\"; S26 [label = \"IN PROCESS\"]; S27 [label = \"FINISHED\"]; } subgraph cluster_T14 { label = \"MILL 7\"; S28 [label = \"IN PROCESS\"]; S29 [label = \"FINISHED\"]; } subgraph cluster_T15 { label = \"MILL 8\"; S30 [label = \"IN PROCESS\"]; S31 [label = \"FINISHED\"]; } subgraph cluster_T16 { label = \"HAAS\"; S32 [label = \"IN PROCESS\"]; S33 [label = \"FINISHED\"]; } subgraph cluster_T17 { label = \"FADAL 1\"; S34 [label = \"IN PROCESS\"]; S35 [label = \"FINISHED\"]; } subgraph cluster_T18 { label = \"FADAL 2\"; S36 [label = \"IN PROCESS\"]; S37 [label = \"FINISHED\"]; } subgraph cluster_T19 { label = \"DUPLICATOR\"; S38 [label = \"IN PROCESS\"]; S39 [label = \"FINISHED\"]; } subgraph cluster_T20 { label = \"TWIN RED\"; S40 [label = \"IN PROCESS\"]; S41 [label = \"FINISHED\"]; } subgraph cluster_T21 { label = \"TWIN BLUE\"; S42 [label = \"IN PROCESS\"]; S43 [label = \"FINISHED\"]; } S1->S3;S47->S44;S3->S4;S4->S5;S83->S84;S84->S47;S45->S46;S46->S47;S82->S45;S15->S16;S16->S47;S17->S18;S18->S47;S20->S21;S21->S47;S22->S23;S23->S47;S24->S25;S25->S47;S26->S27;S27->S47;S28->S29;S29->S47;S30->S31;S31->S47;S32->S33;S33->S47;S34->S35;S35->S47;S36->S37;S37->S47;S38->S39;S39->S47;S40->S41;S41->S47;S42->S43;S43->S47;}";

            byte[] output = wrapper.GenerateGraph(diagraph, Enums.GraphReturnType.Png);
        }

        [Test]
        public void AllowsPlainTextOutputType() {
            // Arrange
            var getProcessStartInfoQuerty = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuerty, _getStartProcessQuery);

            var wrapper = new GraphGeneration(
                _getStartProcessQuery,
                getProcessStartInfoQuerty,
                registerLayoutPluginCommand);

            // Act
            byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Plain);

            var graphPortion = System.Text.Encoding.Default.GetString(output).Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

            Assert.AreEqual("graph 1 1.125 2.5", graphPortion[0]);
        }

        [Test]
        public void AllowsPlainExtTextOutputType()
        {
            // Arrange
            var getProcessStartInfoQuerty = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuerty, _getStartProcessQuery);

            var wrapper = new GraphGeneration(
                _getStartProcessQuery,
                getProcessStartInfoQuerty,
                registerLayoutPluginCommand);

            // Act
            byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.PlainExt);

            var graphPortion = System.Text.Encoding.Default.GetString(output).Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

            Assert.AreEqual("graph 1 1.125 2.5", graphPortion[0]);
        }
    }
}
