using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using PboBuilder;

namespace PboBuilderTests
{
    [TestFixture]
    public class ParseExceptionTests
    {
        [Test]
        public void ParseException_Constructors_All_Exist()
        {
            StaticUtilities.AssertExceptionConstructorsExist(typeof (ParseException));
        }
    }
}