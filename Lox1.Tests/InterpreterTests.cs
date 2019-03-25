using Lox1.Core;
using NUnit.Framework;

using System.Linq;
using Lox1.Core.Ast;
using Lox1.Tool;
using System;
using System.IO;

namespace Lox1.Tests
{
    class InterpreterTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIntegerExpression()
        {
            string input = "(1+3-(345/(345/(34+234-23*23232))))";
            string expected = "534072";

            Scanner scanner = new Scanner(LoxErrorHandler.Error, input);
            var tokens = scanner.ScanTokens();
            Parser parser = new Parser(LoxErrorHandler.Error, tokens);
            Expr expression = parser.Parse();
            Interpreter interpreter = new Interpreter(LoxErrorHandler.Error);
            var result = interpreter.Interpret(expression);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestDoubleExpression()
        {
            string input = "1.4 + 4.5 - (23.8*99.8/(3.8-8.3))";
            string expected = "533.731111111111";

            Scanner scanner = new Scanner(LoxErrorHandler.Error, input);
            var tokens = scanner.ScanTokens();
            Parser parser = new Parser(LoxErrorHandler.Error, tokens);
            Expr expression = parser.Parse();
            Interpreter interpreter = new Interpreter(LoxErrorHandler.Error);
            var result = interpreter.Interpret(expression);

            Assert.AreEqual(expected, result);
        }

    }
}