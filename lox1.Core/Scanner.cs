using System;
using System.Collections.Generic;
using System.Text;
using static Lox1.Core.TokenType;

namespace Lox1.Core
{
    public class Scanner
    {
        private char[] source;
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;


        private static Dictionary<String, TokenType> keywords = new Dictionary<string, TokenType>();

        static Scanner()
        {
            keywords.Add("and", AND);
            keywords.Add("class", CLASS);
            keywords.Add("else", ELSE);
            keywords.Add("false", FALSE);
            keywords.Add("for", FOR);
            keywords.Add("fun", FUN);
            keywords.Add("if", IF);
            keywords.Add("nil", NIL);
            keywords.Add("or", OR);
            keywords.Add("print", PRINT);
            keywords.Add("return", RETURN);
            keywords.Add("super", SUPER);
            keywords.Add("this", THIS);
            keywords.Add("true", TRUE);
            keywords.Add("var", VAR);
            keywords.Add("while", WHILE);
        }

        public Scanner(string source)
        {
            this.source = source.ToCharArray();
        }

        private List<Token> ScanTokens()
        {


            while (!IsAtEnd())
            {
                // At beginning of next lexeme
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '{': AddToken(LEFT_BRACE); break;
                case '}': AddToken(RIGHT_BRACE); break;
                case ',': AddToken(COMMA); break;
                case '.': AddToken(DOT); break;
                case '-': AddToken(MINUS); break;
                case '+': AddToken(PLUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;

                case '!': AddToken(Match('=') ? BANG_EQUAL : BANG); break;
                case '=': AddToken(Match('=') ? EQUAL_EQUAL : EQUAL); break;
                case '<': AddToken(Match('=') ? LESS_EQUAL : LESS); break;
                case '>': AddToken(Match('=') ? GREATER_EQUAL : GREATER); break;

                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.                
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.                      
                    break;

                case '\n':
                    line++;
                    break;

                case '"':
                    String();
                    break;

                default:

                    if (IsDigit(c))
                        Number();
                    else if (IsAlpha(c))
                        Identifier();
                    else
                        Lox.Error(line, "Unexpected character.");
                    break;
            }
        }

        bool IsAtEnd()
        {
            return current >= source.Length;
        }

        private char Advance()
        {
            current++;
            return (source[current - 1]);
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string lexeme = new String(source.Slice(start, current));
            tokens.Add(new Token(type, lexeme, literal, line));
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
                return false;
            if (source[current] != expected)
                return false;
            current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd())
                return '\0';
            else
                return source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length)
                return '\0';
            return source[current + 1];
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                    line++;
                Advance();
            }

            // Unterminated string...
            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            // The closing "
            Advance();

            // Trim the surrounding quotes
            string lexeme = new string(source.Slice(start + 1, current - 1));
            AddToken(STRING, lexeme);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void Number()
        {
            while (IsDigit(Peek()))
                Advance();

            // Look for a fractional part
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // consume the '.'
                Advance();
                while (IsDigit(Peek()))
                    Advance();
            }
            string lexeme = new string(source.Slice(start, current));
            Double.TryParse(lexeme, out double value);
            AddToken(NUMBER, value);
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
                Advance();
            string lexeme = new string(source.Slice(start, current));
            if (!keywords.TryGetValue(lexeme, out TokenType type))
                type = IDENTIFIER;
            AddToken(type);
        }

    }
}
