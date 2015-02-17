using System;
using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;
using PboBuilder;
using PboLib;

namespace PboBuilderTests
{
    [TestFixture]
    public class ProgramTests
    {
        [SetUp]
        public void TestSetup()
        {
            Environment.ExitCode = 0;
        }

        [Test]
        public void Main_Should_Return_Help_Output_When_Empty_Args_Provided()
        {
            using (var sw = new StringWriter())
            {
                // Arrange
                Console.SetOut(sw);

                // Act
                Program.Main(new[] {string.Empty});

                // Act
                Assert.AreEqual(BuildExpectedHelpOutput(), sw.ToString());
                Assert.AreEqual(0, Environment.ExitCode);
            }
        }

        [Test]
        public void Main_Should_Return_Help_Output_When_Null_Args_Provided()
        {
            using (var sw = new StringWriter())
            {
                // Arrange
                Console.SetOut(sw);

                // Act
                Program.Main(null);

                // Act
                Assert.AreEqual(BuildExpectedHelpOutput(), sw.ToString());
                Assert.AreEqual(0, Environment.ExitCode);
            }
        }

        [Test]
        public void Main_Should_Return_Exit_Code_Of_1_When_Exception_Raised()
        {
            // Act
            Program.Main(new []{"-o","-f", "c:\\Blah"});

            // Assert
            Assert.AreEqual(1, Environment.ExitCode);
        }

        [Test]
        public void Main_Should_Execute_Pbo_File_Pack_Directory_When_Arguments_Are_Valid()
        {
            // Arrange
            var pboFileMock = new Mock<IPboFile>(MockBehavior.Strict);
            pboFileMock.Setup(x => x.PackDirectory(true, "C:\\Blah", "output.pbo"));
            PboFileFactory.MockPboFile = pboFileMock.Object;

            // Act
            Program.Main(new []{"-o","-f", "arma3", "C:\\Blah", "output.pbo"});

            // Assert
            Assert.AreEqual(0, Environment.ExitCode);
        }

        private static string BuildExpectedHelpOutput()
        {
            var buffer = new StringBuilder();
            buffer.AppendLine("PboBuilder Help");
            buffer.AppendLine(string.Empty);
            buffer.AppendLine("  Packs the supplied folder into a pbo with the supplied name.");
            buffer.AppendLine(string.Empty);
            buffer.AppendLine("Arguments:");
            buffer.AppendLine("  [folder to pack] - Folder to be packed into a pbo object");
            buffer.AppendLine("  [output file name] - The output pbo file name");
            buffer.AppendLine(string.Empty);
            buffer.AppendLine("Options:");
            buffer.AppendLine("  -f, --format [pbo format to use]");
            buffer.AppendLine("    Used with the pack option only.");
            buffer.AppendLine("    * arma3 -- PBO format used by vanilla Arma 3");
            buffer.AppendLine(string.Empty);
            buffer.AppendLine("  -o, --overwrite");
            buffer.AppendLine("    Removes the destination file if it already exists.");
            buffer.AppendLine(string.Empty);
            buffer.AppendLine("example:");
            buffer.AppendLine("   PboBuilder.exe -f arma3 C:\\MyPboFolder C:\\MyPboFile.pbo");
            buffer.AppendLine("   PboBuilder.exe -o -f arma3 C:\\MyPboFolder C:\\MyPboFile.pbo");
            return buffer.ToString();
        }
    }
}
