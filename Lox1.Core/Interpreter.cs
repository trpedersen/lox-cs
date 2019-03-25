using System;
using System.Collections.Generic;
using System.Text;
using Lox1.Core.Ast;
using static Lox1.Core.Ast.Expr;
using static Lox1.Core.TokenType;

namespace Lox1.Core
{
    public class Interpreter : IExprVisitor<object>
    {
        private class InterpreterException : Exception
        {
            public readonly Token Token;
            public InterpreterException(Token token, string message) : base(message)
            {
                Token = token;
            }
        }

        private readonly Action<Token, string> ErrorHandler;

        public Interpreter(Action<Token, string> errorHandler)
        {
            ErrorHandler = errorHandler;
        }

        public string Interpret(Expr expression)
        {
            try
            {
                Object value = Evaluate(expression);
                return Stringify(value);
            }
            catch(InterpreterException ex)
            {
                Error(ex);
            }
            return null;
        }

        private void Error(InterpreterException exception)
        {
            ErrorHandler(exception.Token, exception.Message + $"\n[line {exception.Token.Line}]");
        }

        private String Stringify(Object @object)
        {
            if (@object == null) return "nil";

            // Work around C# adding ".0" to integer-valued doubles. // TODO: check this is the case
            if (@object is double) {
                String text = @object.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return @object.ToString();
        }

        public object VisitBinaryExpr(Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case MINUS:
                    CheckNumberOperands(expr.Operator, right);
                    return (double)left - (double)right;
                case SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case PLUS:
                    if (left is double && right is double)
                        return (double)left + (double)right;
                    if (left is string && right is string)
                        return (string)left + (string)right;
                    if((left is string) && (right is double))
                            return (string)left + right.ToString();
                    return left.ToString() + (string)right;
                    //throw new InterpreterException(expr.Operator, "Operands must be either two numbers or two strings.");
                case GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case BANG_EQUAL:
                    return !IsEqual(left, right);
                case EQUAL_EQUAL:
                    return IsEqual(left, right);
                default:
                    return null;
            }
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case BANG:
                    return !IsTruthy(right);
                case MINUS:
                    return -(double)right;

            }
            return null;
        }

        private object Evaluate(Expr expression)
        {
            return expression.Accept(this);
        }

        private bool IsTruthy(object o)
        {
            if (o == null)
                return false;
            if (o is bool)
                return (bool)o;
            return true;
        }

        private bool IsEqual(object left, object right)
        {
            if (left == null && right == null)
                return true;
            if (left == null)
                return false;
            return left.Equals(right);
        }

        private void CheckNumberOperands(Token @operator, params object[] operands)
        {
            foreach( var operand in operands)
            {
                if(!(operand is double ))
                    throw new InterpreterException(@operator, "Operand must be a number.");
            }
            if(@operator.Type == SLASH)
            {
                if((double)operands[1] == 0.0)
                    throw new InterpreterException(@operator, "RHS operand must not be 0.");
            }
            return;
        }

    }
}
