using System;
using System.Collections.Generic;
using System.Text;

namespace Сompiler
{
    public class Scanner
    {
        private string text;
        private int pos = 0;
        private int line = 1;

        public Scanner(string text)
        {
            this.text = text;
        }

        private char Current => pos < text.Length ? text[pos] : '\0';

        private void Next() => pos++;

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (pos < text.Length)
            {
                if (char.IsWhiteSpace(Current))
                {
                    if (Current == '\n') line++;
                    Next();
                    continue;
                }

                if (char.IsDigit(Current))
                {
                    int start = pos;
                    var sb = new StringBuilder();

                    while (char.IsDigit(Current))
                    {
                        sb.Append(Current);
                        Next();
                    }

                    tokens.Add(new Token(1, "num", sb.ToString(), line, start, pos));
                    continue;
                }

                if (char.IsLetter(Current))
                {
                    int start = pos;
                    var sb = new StringBuilder();

                    while (char.IsLetterOrDigit(Current) || Current == '_')
                    {
                        sb.Append(Current);
                        Next();
                    }

                    tokens.Add(new Token(2, "id", sb.ToString(), line, start, pos));
                    continue;
                }

                int opStart = pos;

                if (Current == '*')
                {
                    Next();
                    if (Current == '*')
                    {
                        Next();
                        tokens.Add(new Token(3, "op", "**", line, opStart, pos));
                    }
                    else
                        tokens.Add(new Token(3, "op", "*", line, opStart, pos));
                    continue;
                }

                if (Current == '/')
                {
                    Next();
                    if (Current == '/')
                    {
                        Next();
                        tokens.Add(new Token(3, "op", "//", line, opStart, pos));
                    }
                    else
                        tokens.Add(new Token(3, "op", "/", line, opStart, pos));
                    continue;
                }

                if ("+-()%".Contains(Current))
                {
                    tokens.Add(new Token(3, "op", Current.ToString(), line, pos, pos + 1));
                    Next();
                    continue;
                }

                tokens.Add(Token.Error(Current.ToString(), line, pos, pos + 1));
                Next();
            }

            return tokens;
        }
    }
}