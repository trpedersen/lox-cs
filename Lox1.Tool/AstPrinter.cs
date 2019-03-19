using Lox1.Core.Ast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Lox1.Core.Ast.Expr;

namespace Lox1.Tool
{
    public class AstPrinter : IExprVisitor<string>, IDisposable
    {
        private TextWriter writer;

        public AstPrinter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Write(Expr expr)
        {
            currentIndentLevel = 0;
            writer.Write(expr.Accept<string>(this).Trim());
        }
        public void WriteLine(Expr expr)
        {
            currentIndentLevel = 0;
            writer.WriteLine(expr.Accept<string>(this).Trim());
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesise(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Parenthesise("group", expr.Expression);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null)
                return "nil";
            else
            {
                //StringBuilder sb = new StringBuilder();
                //sb.Append("\n");
                //for (int i = 0; i < stackDepth + 1; i++)
                //{
                //    sb.Append(standardIndent);
                //}
                //sb.Append(expr.Value.ToString());
                //return sb.ToString();
                return expr.Value.ToString();
            }
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesise(expr.Operator.Lexeme, expr.Right);
        }

        private int currentIndentLevel = 0;
        private string standardIndent = "  ";
        static string currentIndent = "";

        private void PushIndent()
        {
            currentIndentLevel++;
            currentIndent += standardIndent;
        }

        private void PopIndent()
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

        private string Parenthesise(string name, params Expr[] expressions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(")
                .Append(name);

            bool allLiterals = true;
            foreach(var expr in expressions)
            {
                if(!(expr is Expr.Literal))
                {
                    allLiterals = false;
                    break;
                }
            }

            if (allLiterals)
            {
                foreach (var expr in expressions)
                {
                    sb.Append(" ")
                        .Append(expr.Accept<string>(this));
                }
            }
            else
            {
                PushIndent();
                foreach (Expr expr in expressions)
                {
                    sb.Append("\n")
                        .Append(currentIndent)
                        .Append(expr.Accept<string>(this));
                }
                PopIndent();
            }
            sb.Append(")");
            return sb.ToString();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AstPrinter() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
