using Lox1.Core.Ast;
using Lox1.Tool;
using System;
using System.Diagnostics;
using System.IO;

namespace Lox1.Core
{
    public static class Lox
    {
        public static bool HadError { get; private set;}
        private static readonly TextWriter errorWriter = Console.Error;
        private static readonly Interpreter interpreter = new Interpreter(Error);

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
        //(1+3-(345/(345/(34+234-23*23232))))
        public static int Run(string input)
        {
            if (input != null)
            {
                Scanner scanner = new Scanner(Error, input);
                Stopwatch timer = Stopwatch.StartNew();
                var tokens = scanner.ScanTokens();
                Debug.WriteLine($"scan timer: {timer.ElapsedMilliseconds} ms, {timer.ElapsedTicks} ticks");

                Parser parser = new Parser(Error, tokens);
                HadError = false;
                timer.Reset();
                Expr expression = parser.Parse();
                Debug.WriteLine($"parse timer: {timer.ElapsedMilliseconds} ms, {timer.ElapsedTicks} ticks");
                if (HadError)
                    return (int)ExitCode.LoxError;

                AstPrinter printer = new AstPrinter(Console.Out);
                printer.WriteLine(expression);

                timer.Reset();
                var result = interpreter.Interpret(expression);
                Debug.WriteLine($"interpreter timer: {timer.ElapsedMilliseconds} ms, {timer.ElapsedTicks} ticks");
                if (HadError)
                    return (int)ExitCode.LoxError;

                Console.WriteLine($"Result> {result}");

            }
            return 0;
        }

        public static void Error( int line, string message)
        {
            ReportError(line, "", message);
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                ReportError(token.Line, " at end", message);
            }
            else
            {
                ReportError(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void ReportError(int line, string where, string message)
        {
            errorWriter.WriteLine($"[line {line}] Error {where}: {message}");
            HadError = true;
        }

    }
}
