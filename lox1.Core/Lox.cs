using System;
using System.IO;

namespace Lox1.Core
{
    public static class Lox
    {
        public static bool HadError { get; private set;}
        private static readonly TextWriter errorWriter = Console.Error;

        public static int RunFile(string path)
        {
            string input = File.ReadAllText(path);
            return Run(input);
        }

        public static void RunPrompt()
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
            }
         //   return Run(input);
        }

        public static int Run(string input)
        {
            Console.WriteLine(input);
            return 0;
        }

        public static void Error( int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            errorWriter.WriteLine($"[line {line}] Error {where}: {message}");
            HadError = true;
        }

    }
}
