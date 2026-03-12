using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Token
    {
        public int Code { get; }
        public string Type { get; }
        public string Lexeme { get; }
        public int Line { get; }
        public int Start { get; }
        public int End { get; }
        public string? Message { get; }

        public Token(int code, string type, string lexeme, int line, int start, int end, string? message = null)
        {
            Code = code;
            Type = type;
            Lexeme = lexeme;
            Line = line;
            Start = start;
            End = end;
            Message = message;
        }

        public static Token Error(string lexeme, int line, int start, int end)
            => new Token(11, "ошибка", lexeme, line, start, end, "Недопустимый символ");
    }

}
