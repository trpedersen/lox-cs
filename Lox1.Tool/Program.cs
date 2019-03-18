using CommandLine;
using Lox1.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lox1.Tool
{
    class Program
    {
        [Verb("generate-ast", HelpText = "Generate AST.")]
        public class AstOptions
        {
            [Option('o', "output-director", Required = true, HelpText = "Output directory")]
            public string OutputDirectory { get; set; }

        }
        static int Main(string[] args)
        {
            ExitCode exitCode = ExitCode.Success;
            TextWriter errorWriter = Console.Error;

            try
            {
                CommandLine.Parser.Default.ParseArguments<AstOptions>(args)
                       .WithParsed<AstOptions>(options => { exitCode = AstGenerator.Generate(options.OutputDirectory); })
                       .WithNotParsed(errors => { errorWriter.WriteLine($"Bad arguments"); exitCode = ExitCode.InvalidArguments; });
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Program exiting abnormally, exception encountered: {e.Message}");
                errorWriter.WriteLine($"Program exiting abnormally, exception encountered: {e.Message}");
                exitCode = ExitCode.UnknownError;
            }

            return (int)exitCode;

        }
    }
}
