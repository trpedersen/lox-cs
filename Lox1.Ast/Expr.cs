using Lox1.Core;

namespace Lox1.Ast
{
    public abstract class Expr
    {

        public interface IExprVisitor<T>
        {
            T VisitBinaryExpr( Binary expr);
            T VisitGroupingExpr( Grouping expr);
            T VisitLiteralExpr( Literal expr);
            T VisitUnaryExpr( Unary expr);
        }

        public abstract T Accept<T>( IExprVisitor<T> visitor);

        public class Binary: Expr
        {
            readonly Expr left;
            readonly Token operatorToken;
            readonly Expr right;

            Binary(Expr left, Token operatorToken, Expr right)
            {
                this.left = left;
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Grouping: Expr
        {
            readonly Expr expression;

            Grouping(Expr expression)
            {
                this.expression = expression;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }

        public class Literal: Expr
        {
            readonly object value;

            Literal(object value)
            {
                this.value = value;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public class Unary: Expr
        {
            readonly Token operatorToken;
            readonly Expr right;

            Unary(Token operatorToken, Expr right)
            {
                this.operatorToken = operatorToken;
                this.right = right;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

}

}
