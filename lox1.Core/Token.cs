﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lox1.Core
{
    public class Token
    {
        TokenType type;
        string lexeme;
        object literal;
        int line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            return $"{type.ToString()} {lexeme} {literal}";
        }
    }
}
