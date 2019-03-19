using Lox1.Core.Ast;
using Lox1.Tool;
using System;
using System.IO;

namespace Lox1.Core
{
    public static class Lox
    {
        public static bool HadError { get; private set;}
        private static readonly TextWriter errorWriter = Console.Error;

        public static ExitCode RunFile(string path)
        {
            string input = File.ReadAllText(path);
            Run(input);
            if (HadError)
            {
                return ExitCode.LoxError;
            }
            return ExitCode.Success;
        }

        public static ExitCode RunPrompt()
        {
            bool keepGoing = true;
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                Console.WriteLine();
                Console.WriteLine("Bye bye!");
                e.Cancel = true;
                keepGoing = false;
            };
            while(keepGoing)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                Run(input);
            }
            if (HadError)
            {
                return ExitCode.LoxError;
            }
            return ExitCode.Success;
        }

        public static int Run(string input)
        {
            if (input != null)
            {
                Scanner scanner = new Scanner(Error, input);
                var tokens = scanner.ScanTokens();
                Parser parser = new Parser(Error, tokens);
                HadError = false;

                Expr expression = parser.Parse();
                if (HadError)
                    return (int)ExitCode.LoxError;

                AstPrinter printer = new AstPrinter(Console.Out);
                printer.WriteLine(expression);

            }
            return 0;
        }

        public static void Error( int line, string message)
        {
            Report(line, "", message);
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void Report(int line, string where, string message)
        {
            errorWriter.WriteLine($"[line {line}] Error {where}: {message}");
            HadError = true;
        }

    }
}
