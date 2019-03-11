using System;
using System.IO;

namespace lox1.Core
{
    public static class Runner
    {
        public static int RunFile(string path)
        {
            string input = File.ReadAllText(path);
            return Run(input);
        }

        public static int RunPrompt()
        {
            string input = Console.ReadLine();
            return Run(input);
        }

        public static int Run(string input)
        {
            Console.WriteLine(input);
            return 0;
        }

    }
}
