using Lox1.Core.Ast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Lox1.Core.Ast.Expr;

namespace Lox1.Tool
{
    public class AstRPNPrinter : IExprVisitor<string>, IDisposable
    {
        private TextWriter writer;

        public AstRPNPrinter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Write(Expr expr)
        {
            writer.Write(expr.Accept<string>(this));
        }
        public void WriteLine(Expr expr)
        {
            writer.WriteLine(expr.Accept<string>(this));
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Stack(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Stack("", expr.Expression);
        }


        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null)
                return "nil";
            else
                return expr.Value.ToString();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Stack(expr.Operator.Lexeme, expr.Right);
        }

        private string Stack(string name, params Expr[] expressions)
        {
            StringBuilder sb = new StringBuilder();

            foreach(Expr expr in expressions)
            {
                sb.Append(expr.Accept<string>(this));
                sb.Append(" ");
            }
            string result = null;
            if(string.IsNullOrEmpty(name))
                result = sb.ToString().Trim();
            else
                result = sb.ToString().Trim() + " " + name.Trim();
            //sb.Append(name);

            //return sb.ToString();
            return result;
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
