using System;
using System.IO;

using static Lox1.Core.Lox;
using Lox1.Core;

namespace Lox1
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length > 1)
            {
                Console.WriteLine("Usage: lox1 [script]");
                return (int) ExitCode.InvalidArguments;
            }
            else if (args.Length == 1)
            {
                Console.WriteLine($"Running file {args[0]}");
                return (int) RunFile(args[0]);
            } else
            {
                RunPrompt();
            }
            if (HadError)
            {
                return (int) ExitCode.LoxError;
            }
            return 0;
        }

    }
}
