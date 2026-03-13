using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Scanner
    {
        private readonly string _text;
        private int _pos;
        private int _line = 1;
        private int _col = 1;

        private static readonly string[] Forbidden =
         {
            "int", "float", "double", "string", "char", "bool",
            "if", "else", "for", "return", "break", "continue",
            "class", "public", "private", "protected", "static",
            "void", "new", "using", "namespace"
        };
        public Scanner(string text)
        {
            _text = text ?? string.Empty;
        }

        public List<Token> Analyze()
        {
            var tokens = new List<Token>();

            while (!EOF())
            {
                char c = Peek();
                if (c == ' ')
                {
                    tokens.Add(new Token(3, "разделитель", " ", _line, _col, _col));
                    Advance();
                    continue;
                }

                if (c == '\t')
                {
                    tokens.Add(new Token(3, "разделитель", "    ", _line, _col, _col + 3));
                    _pos++;
                    _col += 4;
                    continue;
                }

                if (c == '\r')
                {
                    Advance();
                    continue;
                }

                if (c == '\n')
                {
                    AdvanceLine();
                    continue;
                }

                if (char.IsLetter(c))
                {
                    tokens.Add(ReadIdentifier());
                    continue;
                }

                if (char.IsDigit(c))
                {
                    tokens.Add(ReadNumber());
                    continue;
                }

                tokens.Add(ReadSymbol());
            }

            return tokens;
        }
        private Token ReadIdentifier()
        {
            int start = _col;
            int startPos = _pos;

            while (!EOF() && char.IsLetterOrDigit(Peek()))
                Advance();

            string lex = _text.Substring(startPos, _pos - startPos);

            if (lex == "while")
                return new Token(1, "ключевое слово", lex, _line, start, _col - 1);

            if (Forbidden.Contains(lex))
                return new Token(11, "ошибка", lex, _line, start, _col - 1);

            return new Token(2, "идентификатор", lex, _line, start, _col - 1);
        }


        private Token ReadNumber()
        {
            int start = _col;
            int startPos = _pos;

            while (!EOF() && char.IsDigit(Peek()))
                Advance();

            string lex = _text.Substring(startPos, _pos - startPos);

            return new Token(10, "число", lex, _line, start, _col - 1);
        }

        private Token ReadSymbol()
        {
            int start = _col;
            char c = Peek();

            if (c == ';')
            {
                Advance();
                return new Token(9, "конец конструкции", ";", _line, start, _col - 1);
            }

            if (c == ':')
            {
                Advance();
                return new Token(8, "начало блока", ":", _line, start, _col - 1);
            }
            
            if ("=<>!".Contains(c))
                return ReadOperator(start);

            Advance();
            return Token.Error(c.ToString(), _line, start, _col - 1);
        }

        private Token ReadOperator(int start)
        {
            char first = Peek();
            Advance();

            if (!EOF() && Peek() == '=')
            {
                char second = Peek();
                Advance();

                string op = $"{first}{second}";

                return op switch
                {
                    "<=" => new Token(6, "оператор сравнения", "<=", _line, start, _col - 1),
                    ">=" => new Token(6, "оператор сравнения", ">=", _line, start, _col - 1),
                    "==" => new Token(7, "оператор равенства", "==", _line, start, _col - 1),
                    "!=" => new Token(7, "оператор равенства", "!=", _line, start, _col - 1),
                    _ => Token.Error(op, _line, start, _col - 1)
                };
            }

            return first switch
            {
                '=' => new Token(4, "присваивание", "=", _line, start, _col - 1),
                '<' => new Token(5, "оператор сравнения", "<", _line, start, _col - 1),
                '>' => new Token(5, "оператор сравнения", ">", _line, start, _col - 1),
                _ => Token.Error(first.ToString(), _line, start, _col - 1)
            };
        }

        private bool EOF() => _pos >= _text.Length;
        private char Peek() => _text[_pos];
        private void Advance() { _pos++; _col++; }
        private void AdvanceLine() { _pos++; _line++; _col = 1; }
    }
}