using System;
using System.Collections.Generic;
using System.Text;

namespace Lox1.Core
{
    public class Token
    {
        public TokenType Type;
        public string Lexeme;
        public object Literal;
        public int Line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.Type = type;
            this.Lexeme = lexeme;
            this.Literal = literal;
            this.Line = line;
        }

        public override string ToString()
        {
            return $"{Type.ToString()} {Lexeme} {Literal}";
        }
    }
}
