using Lox1.Core;
using NUnit.Framework;

using System.Linq;
using Lox1.Core.Ast;
using Lox1.Tool;
using System;
using System.IO;

namespace Lox1.Tests
{
    class AstTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAstPrinter()
        {
            string result;
            using(StringWriter writer = new StringWriter())
            using(AstPrinter printer = new AstPrinter(writer))
            {

                Expr expression = new Expr.Binary(
                    new Expr.Unary(
                        new Token(TokenType.MINUS, "-", null, 1),
                        new Expr.Literal(123)),
                    new Token(TokenType.STAR, "*", null, 1),
                    new Expr.Grouping(
                        new Expr.Literal(45.67)));
                printer.Write(expression);
                result = writer.ToString();
            }
            string expected = "(*\n  (- 123)\n  (group 45.67))";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestAstRPNPrinter()
        {
            string result;
            using (StringWriter writer = new StringWriter())
            using (AstRPNPrinter printer = new AstRPNPrinter(writer))
            {

                Expr expression =
                    new Expr.Binary(
                        new Expr.Grouping(
                            new Expr.Binary(
                                new Expr.Literal(1),
                                new Token(TokenType.PLUS, "+", null, 1),
                                new Expr.Literal(2))),
                        new Token(TokenType.STAR, "*", null, 1),
                        new Expr.Grouping(
                            new Expr.Binary(
                                new Expr.Literal(4),
                                new Token(TokenType.MINUS, "-", null, 1),
                                new Expr.Literal(3)))
                     );

                printer.Write(expression);
                result = writer.ToString();
            }
            string expected = "1 2 + 4 3 - *";

            Assert.AreEqual(expected, result);
        }

    }
}