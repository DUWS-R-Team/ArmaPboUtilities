using System.Linq.Expressions;
using NUnit.Framework;
using PboBuilder;
using PboLib;

namespace PboBuilderTests
{
    [TestFixture]
    public class RunParametersTest
    {
        [Test]
        public void PopulateFromCommandLineArguments_Should_Throw_ParseException_When_Args_Null()
        {
            // Arrange
            var runParameters = new RunParameters();
            ParseException thrownException = null;

            try
            {
                // Act
                runParameters.PopulateFromCommandLineArguments(null);
            }
            catch (ParseException ex)
            {
                thrownException = ex;
            }

            // Assert
            Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
            Assert.AreEqual("Cannot parse null arguments", thrownException.Message);
        }

        [Test]
        public void PopulateFromCommandLineArguments_Should_Throw_ParseException_When_Format_Arg_Cannot_Be_Understood()
        {
            // Arrange
            var runParameters = new RunParameters();
            ParseException thrownException = null;

            try
            {
                // Act
                runParameters.PopulateFromCommandLineArguments(new[] {"-f", "foo"});
            }
            catch (ParseException ex)
            {
                thrownException = ex;
            }

            // Assert
            Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
            Assert.AreEqual("It appears the format argument was not supplied or is not a valid format. Please verify the argument and try again.", thrownException.Message);
        }

        [Test]
        public void PopulateFromCommandLineArguments_Should_Populate_Object_When_All_Properties_Specified_Correctly()
        {
            // Arrange
            var runParameters = new RunParameters();
            const string expectedFolderToPack = "C:\\Some\\Path\\";
            const string expectedDestinationFilePath = "output.pbo";

            // Act
            runParameters.PopulateFromCommandLineArguments(new[] {"-o", "-f", "ArmA3", expectedFolderToPack, expectedDestinationFilePath});

            // Assert
            Assert.AreEqual(true, runParameters.IsOverwriteEnabled);
            Assert.AreEqual(PboFormat.Arma3, runParameters.PboFormat);
            Assert.AreEqual(expectedFolderToPack, runParameters.FolderToPack);
            Assert.AreEqual(expectedDestinationFilePath, runParameters.DestinationFilePath);
        }

        [Test]
        public void PopulateFromCommandLineArguments_Should_Throw_Exception_When_Unknown_Argument_Specified()
        {
            // Arrange
            var runParameters = new RunParameters();
            const string expectedFolderToPack = "C:\\Some\\Path\\";
            const string expectedDestinationFilePath = "output.pbo";
            ParseException thrownException = null;

            try
            {
                // Act
                runParameters.PopulateFromCommandLineArguments(new[]
                {"-o", "-f", "ArmA3", expectedFolderToPack, expectedDestinationFilePath, "unknownArg"});
            }
            catch (ParseException ex)
            {
                thrownException = ex;
            }

            // Assert
            Assert.IsNotNull(thrownException, "Expected exception was not thrown.");
            Assert.AreEqual("Unknown argument: unknownArg", thrownException.Message);
        }

        [Test]
        public void GetHashCode_Returns_Hash_Code_Based_On_The_Current_Object_Status_When_Using_Default_Object()
        {
            // Arrange
            var runParameters = new RunParameters();

            // Act
            var result = runParameters.GetHashCode();

            // Assert
            Assert.AreEqual(8993, result);
        }

        [Test]
        public void GetHashCode_Returns_Hash_Code_Based_On_The_Current_Object_Status_When_Using_Populated_Object()
        {
            // Arrange
            const string expectedFolderToPack = "C:\\Some\\Path\\";
            const string expectedDestinationFilePath = "output.pbo";
            var runParameters = new RunParameters();
            runParameters.PopulateFromCommandLineArguments(new[] {"-o", "-f", "ArmA3", expectedFolderToPack, expectedDestinationFilePath});

            // Act
            var result = runParameters.GetHashCode();

            // Assert
            Assert.AreEqual(1932353595, result);
        }

        [Test]
        public void ToString_Returns_String_Based_On_The_Current_Object_Status_When_Using_Default_Object()
        {
            // Arrange
            var runParameters = new RunParameters();

            // Act
            var result = runParameters.ToString();

            // Assert
            Assert.AreEqual("PboBuilder.RunParameters with values IsOverwriteEnabled:False, PboFormat:Unknown, FolderToPack:, DestinationFilePath:", result);
        }

        [Test]
        public void Equals_Returns_False_When_Using_Null_Object()
        {
            // Arrange
            var runParameters = new RunParameters();

            // Act
            var result = runParameters.Equals(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Returns_True_When_Using_Two_Default_Objects()
        {
            // Arrange
            var runParameters = new RunParameters();
            var runParameters2 = new RunParameters();

            // Act
            var result = runParameters.Equals(runParameters2);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_Operator_Returns_True_When_Using_Two_Default_Objects()
        {
            // Arrange
            var runParameters = new RunParameters();
            var runParameters2 = new RunParameters();

            // Act
            var result = runParameters == runParameters2;

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void NotEquals_Operator_Returns_True_When_Using_Two_Default_Objects()
        {
            // Arrange
            var runParameters = new RunParameters();
            var runParameters2 = new RunParameters();

            // Act
            var result = runParameters != runParameters2;

            // Assert
            Assert.AreEqual(false, result);
        }
    }
}