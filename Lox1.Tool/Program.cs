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
                Parser.Default.ParseArguments<AstOptions>(args)
                       .WithParsed<AstOptions>(options => CreateAst(options))
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

        private static void CreateAst(AstOptions options)
        {
            List<string> ExprTypes = new List<string>()
            {
                "Binary     : Expr left, Token operatorToken, Expr right",
                "Grouping   : Expr expression",
                "Literal    : object value",
                "Unary      : Token operatorToken, Expr right",
            };

            DefineAst(options.OutputDirectory, "Expr", ExprTypes);


        }

        static string standardIndent = "    ";
        static int currentIndentLevel = 0;
        static string currentIndent = "";

        private static void PushIndent()
        {
            currentIndentLevel++;
            currentIndent += standardIndent;
        }
        private static void IndentSpecific(int level)
        {
            currentIndentLevel = level;
            if (currentIndentLevel < 0)
                currentIndentLevel = 0;
            currentIndent = "";
            for (int i = 0; i < currentIndentLevel; i++)
            {
                currentIndent += standardIndent;
            }
        }

        private static void PopIndent()
        {
            currentIndentLevel--;
            if (currentIndentLevel < 0)
                currentIndentLevel = 0;
            currentIndent = "";
            for (int i = 0; i < currentIndentLevel; i++)
            {
                currentIndent += standardIndent;
            }
        }

        private static void DefineAst(string outputDirectory, string baseName, List<String> types)
        {
            String outputPath = outputDirectory + "/" + baseName + ".cs";

            using(StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("using Lox1.Core;");
                writer.WriteLine();
                writer.WriteLine("namespace Lox1.Ast");
                writer.WriteLine("{");

                PushIndent();

                writer.WriteLine($"{currentIndent}public abstract class {baseName}");
                writer.WriteLine($"{currentIndent}{{");

                // Visitor interface(s)
                DefineVisitorInterfaces(writer, baseName, types);
                writer.WriteLine();

                // Base abstract functions
                PushIndent();
                writer.WriteLine($"{currentIndent}public abstract T Accept<T>( IExprVisitor<T> visitor);");
                PopIndent();
                writer.WriteLine();


                // AST classes
                foreach (string type in types)
                {
                    string[] defn = type.Split(new char[] { ':' });
                    string className = defn[0].Trim();
                    string fieldList = defn[1].Trim();
                    DefineType(writer, baseName, className, fieldList);
                    writer.WriteLine();
                }
                PopIndent();
                writer.WriteLine(($"{currentIndent}}}")); // end abstract class

                IndentSpecific(0);
                writer.WriteLine();
                writer.WriteLine("}"); // end namespace

            }

        }

        private static void DefineVisitorInterfaces(StreamWriter writer, string baseName, List<string> types)
        {
            PushIndent();
            writer.WriteLine();
            writer.WriteLine($"{currentIndent}public interface IExprVisitor<T>");
            writer.WriteLine($"{currentIndent}{{");
            PushIndent();
            foreach(string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine($"{currentIndent}T Visit{typeName}{baseName}( {typeName} {baseName.ToLower()});");
            }
            PopIndent();
            writer.WriteLine($"{currentIndent}}}"); // end interface Visitor
            PopIndent();
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            try
            {
                PushIndent();
                // nested class defn.
                writer.WriteLine($"{currentIndent}public class {className}: {baseName}");
                writer.WriteLine($"{currentIndent}{{");

                PushIndent();

                var fields = fieldList.Split(", ");
                // properties
                foreach(var field in fields)
                {
                    writer.WriteLine($"{currentIndent}readonly {field};");
                }

                writer.WriteLine();

                // constructor
                writer.WriteLine($"{currentIndent}{className}({fieldList})");
                writer.WriteLine($"{currentIndent}{{");
                PushIndent();
                foreach (var field in fields)
                {
                    string name = field.Split(" ")[1];
                    writer.WriteLine($"{currentIndent}this.{name} = {name};");
                }
                PopIndent();
                writer.WriteLine($"{currentIndent}}}"); // end constructor

                writer.WriteLine();
                writer.WriteLine($"{currentIndent}public override T Accept<T>(IExprVisitor<T> visitor)");
                writer.WriteLine($"{currentIndent}{{");
                PushIndent();
                writer.WriteLine($"{currentIndent}return visitor.Visit{className}{baseName}(this);");
                PopIndent();
                writer.WriteLine($"{currentIndent}}}");

                PopIndent();
                writer.WriteLine($"{currentIndent}}}"); // end class

                PopIndent();

            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
            }
        }
    }





    //public abstract class Base
    //{
    //    public abstract void DoSomething();

    //    public class Child : Base
    //    {
    //        public override void DoSomething()
    //        {
    //            Console.WriteLine("Child DoSomething()");
    //        }

    //    }
    //}
}
