using Lox1.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lox1.Tests
{
    public static class LoxErrorHandler
    {
        public static void Error(int line, string message)
        {
            throw new NUnit.Framework.AssertionException($"Lox error during test: {line} - {message}");
        }

        public static void Error(Token token, string message)
        {
            throw new NUnit.Framework.AssertionException($"Lox error during test: {token.ToString()} - {message}");
        }
    }
}
