using Lox1.Core;
using NUnit.Framework;

using static Lox1.Core.Lox;
using System.Linq;

namespace Lox1.Tests
{
    public class ScannerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRunFile()
        {
            ExitCode result = RunFile("test_input.lox");
            Assert.AreEqual(result, ExitCode.Success);
        }

        [Test]
        public void TestScanString()
        {
            string source = " + - * / 123 456 \"abc\" \"def\"";
            int expectedTokens = 9; // include EOF
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            Assert.AreEqual(tokens.Count, expectedTokens);
        }
    }
}