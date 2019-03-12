using NUnit.Framework;

using static Lox1.Core.Lox;

namespace Lox1.Tests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRunFile()
        {
            int result = RunFile("test_input.lox");
            Assert.Zero(result);
        }
    }
}