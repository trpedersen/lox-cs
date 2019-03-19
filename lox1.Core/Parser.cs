using System;
using System.Collections.Generic;
using System.Text;

using Lox1.Core.Ast;
using static Lox1.Core.TokenType;

namespace Lox1.Core
{
    /*
        expression     → equality ;
        equality       → comparison ( ( "!=" | "==" ) comparison )* ;
        comparison     → addition ( ( ">" | ">=" | "<" | "<=" ) addition )* ;
        addition       → multiplication ( ( "-" | "+" ) multiplication )* ;
        multiplication → unary ( ( "/" | "*" ) unary )* ;
        unary          → ( "!" | "-" ) unary
                       | primary ;
        primary        → NUMBER | STRING | "false" | "true" | "nil"
                       | "(" expression ")" ;
    */

    public class Parser
    {
        private class ParseException : Exception
        {
            public ParseException(string message) : base(message)
            {
            }
        }

        private readonly List<Token> Tokens;
        private int current = 0;
        private Action<Token, string> ParseErrorHandler;

        public Parser(Action<Token, string> parseErrorHandler, List<Token> tokens)
        {
            ParseErrorHandler = parseErrorHandler;
            Tokens = tokens;
        }

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch(ParseException e)
            {
                return null;
            }
        }

        private Expr Expression()
        {
            return Equality();
        }

        private Expr Equality()
        {
            // equality → comparison ( ( "!=" | "==" ) comparison )* ;

            Expr expr = Comparison();
            while(Match(BANG_EQUAL, EQUAL_EQUAL))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Comparison()
        {
            // comparison → addition ( ( ">" | ">=" | "<" | "<=" ) addition )* ;
            Expr expr = Addition();

            while(Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Addition();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Addition()
        {
            // addition → multiplication ( ( "-" | "+" ) multiplication )* ;

            Expr expr = Multiplication();

            while(Match(MINUS, PLUS))
            {
                Token op = Previous();
                Expr right = Multiplication();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Multiplication()
        {
            // multiplication → unary(("/" | "*") unary) * ;
            Expr expr = Unary();

            while (Match(SLASH, STAR))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }
            return expr;
        }

        private Expr Unary()
        {
            // unary → ( "!" | "-" ) unary
            if (Match(BANG, MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }
            return Primary();
        }

        private Expr Primary()
        {
            // primary → NUMBER | STRING | "false" | "true" | "nil"
            //         | "(" expression ")";

            if (Match(FALSE)) return new Expr.Literal(false);
            if (Match(TRUE)) return new Expr.Literal(true);
            if (Match(NIL)) return new Expr.Literal(null);

            if (Match(NUMBER, STRING)) { return new Expr.Literal(Previous().Literal); };

            if (Match(LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }
            throw Error(Peek(), "Expecting an expression.");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private ParseException Error(Token token, string message)
        {
            ParseErrorHandler(token, message);
            return new ParseException(message);
        }

        private void Synchronise()
        {
            Addition();
            while (!IsAtEnd())
            {
                if (Previous().Type == SEMICOLON)
                    return;

                switch (Peek().Type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }
                Advance();
            }
        }

        private bool Match(params TokenType[] tokenTypes)
        {
            foreach(var type in tokenTypes)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
                return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
                current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == EOF;
        }

        private Token Peek()
        {
            return Tokens[current];
        }

        private Token Previous()
        {
            return Tokens[current - 1];
        }
    }
}
