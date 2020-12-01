using Blackdot.InvestigationSearch.Core;
using NUnit.Framework;

namespace Blackdot.InvestigationSearch.Tests
{
    public class QueryStringEncoderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("hello", "hello")]
        [TestCase("SpaceX", "SpaceX")]
        [TestCase("quantum physics", "quantum+physics")]
        [TestCase("support vector machines", "support+vector+machines")]
        [TestCase("To Be or Not To Be", "To+Be+or+Not+To+Be")]
        [TestCase("Hello, how are you?", "Hello%2c+how+are+you%3f")]
        [TestCase("1+2+3", "1%2b2%2b3")]
        [TestCase("1+2+3*5", "1%2b2%2b3*5")]
        [TestCase("1+2+3*5/2", "1%2b2%2b3*5%2f2")]
        [TestCase("1+2+(2/2)", "1%2b2%2b(2%2f2)")]
        public void QueryStringEncoder_Encode_HappyPath(string searchTerm, string expectedResult)
        {
            // arrange
            var queryStringEncoder = new QueryStringEncoder();

            // act
            var result = queryStringEncoder.Encode(searchTerm);

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}