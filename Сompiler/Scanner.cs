    using System;
    using System.Collections.Generic;
    using System.Linq;

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
                        Advance();
                        continue;
                    }

                    if (c == '\t')
                    {
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

                    if (char.IsLetter(c) || c == '_')
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

                if (!(char.IsLetter(Peek()) || Peek() == '_'))
                    return Token.Error(Peek().ToString(), _line, start, _col);

                Advance();

                while (!EOF() && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
                    Advance();
            
                string lex = _text.Substring(startPos, _pos - startPos);

                if (lex == "while")
                    return new Token(1, "ключевое слово", lex, _line, start, _col - 1);

                // ДОБАВИТЬ ЭТО:
                if (lex == "and")
                    return new Token(30, "логическое И", lex, _line, start, _col - 1);

                if (lex == "or")
                    return new Token(31, "логическое ИЛИ", lex, _line, start, _col - 1);

                if (lex == "not")
                    return new Token(32, "логическое НЕ", lex, _line, start, _col - 1);
                // КОНЕЦ ДОБАВКИ

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

                return new Token(10, "целое без знака", lex, _line, start, _col - 1);
            }

            private Token ReadSymbol()
            {
                int start = _col;
                char c = Peek();


                if (c == '+' && PeekNext() == '=')
                {
                    Advance(); 
                    Advance();
                    return new Token(12, "оператор присваивания", "+=", _line, start, _col - 1);
                }

                if (c == '-' && PeekNext() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(13, "оператор присваивания", "-=", _line, start, _col - 1);
                }

                if (c == '*' && PeekNext() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(14, "оператор присваивания", "*=", _line, start, _col - 1);
                }

                if (c == '/' && PeekNext() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(15, "оператор присваивания", "/=", _line, start, _col - 1);
                }


                if (c == '+')
                {
                    Advance();
                    return new Token(5, "арифметический оператор", "+", _line, start, _col - 1);
                }

                if (c == '-')
                {
                    Advance();
                    return new Token(5, "арифметический оператор", "-", _line, start, _col - 1);
                }

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

                if (c == '(' || c == ')')
                {
                    Advance();
                    return new Token(13, "скобка", c.ToString(), _line, start, _col - 1);
                }

                if (c == '.')
                {
                    Advance();
                    return new Token(14, "точка", ".", _line, start, _col - 1);
                }

                if (c == '&' || c == '|')
                    return ReadLogical(start);

                if ("=<>!".Contains(c))
                    return ReadOperator(start);

                Advance();
                return Token.Error(c.ToString(), _line, start, _col - 1);
            }
            private char PeekNext()
            {
                return _pos + 1 < _text.Length ? _text[_pos + 1] : '\0';
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
                    '!' => new Token(17, "логическое отрицание", "!", _line, start, _col - 1),
                    _ => Token.Error(first.ToString(), _line, start, _col - 1)
                };
            }

            private Token ReadLogical(int start)
            {
                char first = Peek();
                Advance();

                if (!EOF() && Peek() == first)
                {
                    Advance();
                    string op = $"{first}{first}";

                    return op switch
                    {
                        "&&" => new Token(15, "логическое И", "&&", _line, start, _col - 1),
                        "||" => new Token(16, "логическое ИЛИ", "||", _line, start, _col - 1),
                        _ => Token.Error(op, _line, start, _col - 1)
                    };
                }

                return Token.Error(first.ToString(), _line, start, _col - 1);
            }

            private bool EOF() => _pos >= _text.Length;
            private char Peek() => _text[_pos];
            private void Advance() { _pos++; _col++; }
            private void AdvanceLine() { _pos++; _line++; _col = 1; }
        }
    }
