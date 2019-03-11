using NUnit.Framework;

using static lox1.Core.Runner;

namespace Tests
{
    public class Tests
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