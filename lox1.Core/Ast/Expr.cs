using Lox1.Core;

namespace Lox1.Core.Ast
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
            public readonly Expr Left;
            public readonly Token Operator;
            public readonly Expr Right;

            public Binary(Expr Left, Token Operator, Expr Right)
            {
                this.Left = Left;
                this.Operator = Operator;
                this.Right = Right;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Grouping: Expr
        {
            public readonly Expr Expression;

            public Grouping(Expr Expression)
            {
                this.Expression = Expression;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }

        public class Literal: Expr
        {
            public readonly object Value;

            public Literal(object Value)
            {
                this.Value = Value;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public class Unary: Expr
        {
            public readonly Token Operator;
            public readonly Expr Right;

            public Unary(Token Operator, Expr Right)
            {
                this.Operator = Operator;
                this.Right = Right;
            }

            public override T Accept<T>(IExprVisitor<T> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

}

}
