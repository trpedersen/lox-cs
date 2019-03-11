using System;
using System.IO;

using static lox1.Core.Runner;

namespace lox1
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length > 1)
            {
                Console.WriteLine("Usage: lox1 [script]");
                return 1;
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            } else
            {
                RunPrompt();
            }
            return 0;
        }




    }
}
