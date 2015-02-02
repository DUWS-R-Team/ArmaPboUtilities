using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using PboBuilder;

namespace PboBuilderTests
{
    [TestFixture]
    public class ProgramTests
    {

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
            }
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
