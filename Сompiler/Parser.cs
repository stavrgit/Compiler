using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;

namespace Сompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;

        public List<ParseError> Errors { get; } = new();

        private Token Current =>
        _pos < _tokens.Count
        ? _tokens[_pos]
        : new Token(0, "EOF", "", 0, 0, 0);

        private enum ParseResult { Ok, Error }

        private readonly Stack<string> _context = new();

        private static readonly string[] SyncTokens = { ";", ")", ":", "while" };
        private bool _errorInCurrentNode = false;


        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private void InsertQ(string lexeme)
        {
            var fake = new Token(
                2,
                "исправление",
                lexeme,
                Current.Line,
                Current.Start,
                Current.Start
            );

            _tokens.Insert(_pos, fake);
        }

        private string BuildQ(string broken)
        {
            return broken switch
            {
                "Factor" => "i",
                "MulExpr" => "0",
                "AddExpr" => "0",
                "RelExpr" => "0",
                "AndExpr" => "0",
                "OrExpr" => "0",
                "LogicExpr" => "0",
                "Stmt" => ";",        
                "While" => "while",    
                "Colon" => ":",       
                "Semicolon" => ";",       
                _ => "0"
            };
        }


        private void SyncTo(params string[] lexemes)
        {
            while (_pos < _tokens.Count &&
                   Array.IndexOf(lexemes, Current.Lexeme) == -1)
                _pos++;
        }

        private ParseResult IronError(string expected)
        {
            int startPos = _pos;

            // 1. Пропускаем плохой токен (выкидываем whil или лишнюю скобку)
            if (_pos < _tokens.Count) _pos++;

            // 2. Вставляем заглушку
            string broken = _context.Count > 0 ? _context.Peek() : "Factor";
            string q = BuildQ(broken);
            InsertQ(q);

            // 3. Сдвигаем pos ЗА вставленный токен, чтобы парсер считал его "пройденным"
            _pos++;

            ErrorRange(startPos, startPos, $"Ожидался {expected}");
            return ParseResult.Ok;
        }
        private ParseResult ParseFactor()
        {
            _context.Push("Factor");
            var t = Current;

            if (t.Lexeme == "not" || t.Lexeme == "!" || t.Code == 32)
            {
                _pos++;
                var r = ParseFactor();
                _context.Pop();
                return r;
            }

            if (t.Lexeme == "(")
            {
                int start = _pos;
                _pos++;

                var r = ParseLogicExpr();
                if (r == ParseResult.Error)
                {
                    SyncTo(")");
                    if (Current.Lexeme == ")") _pos++;
                    _context.Pop();
                    return ParseResult.Ok;
                }

                if (Current.Lexeme == ")")
                {
                    _pos++;
                    _context.Pop();
                    return ParseResult.Ok;
                }

                ErrorRange(start, _pos - 1, "Ожидалась ')'");
                _context.Pop();
                return ParseResult.Ok;
            }

            if (t.Code == 2 || t.Code == 10)
            {
                _pos++;
                _context.Pop();
                return ParseResult.Ok;
            }

            var res = IronError("идентификатор или число");
            _context.Pop();
            return res;
        }
        private ParseResult ParseMulExpr()
        {
            _context.Push("MulExpr");

            var r = ParseFactor();
            if (r == ParseResult.Error)
            {
                _context.Pop();
                return ParseResult.Error;
            }

            while (Current.Lexeme is "*" or "/")
            {
                _pos++;
                r = ParseFactor();
                if (r == ParseResult.Error)
                {
                    _context.Pop();
                    return ParseResult.Error;
                }
            }

            _context.Pop();
            return ParseResult.Ok;
        }
        private ParseResult ParseAddExpr()
        {
            _context.Push("AddExpr");

            var r = ParseMulExpr();
            if (r == ParseResult.Error)
            {
                _context.Pop();
                return ParseResult.Error;
            }

            while (Current.Lexeme is "+" or "-")
            {
                _pos++;
                r = ParseMulExpr();
                if (r == ParseResult.Error)
                {
                    _context.Pop();
                    return ParseResult.Error;
                }
            }

            _context.Pop();
            return ParseResult.Ok;
        }
        private bool IsAnyOperator(Token t)
        {
            return t.Lexeme is "+" or "-" or "*" or "/" or
                   "<" or ">" or "<=" or ">=" or "==" or "!=" or
                   "and" or "&&" or "or" or "||";
        }

        private ParseResult ParseRelExpr()
        {
            _context.Push("RelExpr");
            var r = ParseAddExpr();

            // Если мы встретили токен, который завершает условие или строку
            if (Current.Lexeme == ":" || Current.Lexeme == ";" || Current.Lexeme == ")")
            {
                if (!_errorInCurrentNode)
                {
                    // Мы ждали оператор, а выражение резко оборвалось
                    Error(Current, "Ожидался оператор");
                }
                _context.Pop();
                return ParseResult.Ok; // Выходим "мягко", давая ParseWhile увидеть ':'
            }

            // Если это не оператор и не символ остановки — вот это реальный мусор
            if (!IsAnyOperator(Current))
            {
                if (!_errorInCurrentNode)
                {
                    IronError("оператор"); // IronError сам удалит мусор и вставит заглушку
                }
                _errorInCurrentNode = true;
                _context.Pop();
                return ParseResult.Ok;
            }

            _pos++; // Пропускаем реальный оператор
            r = ParseAddExpr();
            _context.Pop();
            return r;
        }


        private ParseResult ParseAndExpr()
        {
            _context.Push("AndExpr");

            var r = ParseRelExpr();
            if (r == ParseResult.Error)
            {
                _context.Pop();
                return ParseResult.Error;
            }

            while (Current.Lexeme is "and" or "&&")
            {
                _pos++;
                r = ParseRelExpr();
                if (r == ParseResult.Error)
                {
                    _context.Pop();
                    return ParseResult.Error;
                }
            }

            _context.Pop();
            return ParseResult.Ok;
        }

        private ParseResult ParseOrExpr()
        {
            _context.Push("OrExpr");

            var r = ParseAndExpr();
            if (r == ParseResult.Error)
            {
                _context.Pop();
                return ParseResult.Error;
            }

            while (Current.Lexeme is "or" or "||")
            {
                _pos++;
                r = ParseAndExpr();
                if (r == ParseResult.Error)
                {
                    _context.Pop();
                    return ParseResult.Error;
                }
            }

            _context.Pop();
            return ParseResult.Ok;
        }

        private ParseResult ParseLogicExpr()
        {

            _context.Push("LogicExpr");
            var r = ParseOrExpr();
            _context.Pop();

            return r;
        }

        private ParseResult ParseStmt()
        {

            _context.Push("Stmt");

            int start = _pos;

            if (Current.Code == 2)
            {
                _pos++;

                if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
                {
                    _pos++;

                    var r = ParseLogicExpr();

                    _context.Push("Semicolon");
                    if (Current.Lexeme == ";")
                        _pos++;
                    else
                        IronError("';'");

                    _context.Pop();

                    _context.Pop();
                    return ParseResult.Ok;
                }

                _pos = start;
            }

            var r2 = ParseLogicExpr();

            if (r2 == ParseResult.Ok)
            {
                _context.Push("Semicolon");
                if (Current.Lexeme == ";")
                    _pos++;
                else
                    IronError("';'");

                _context.Pop();

                _context.Pop();
                return ParseResult.Ok;
            }

            ErrorRange(start, start, "Ожидалось присваивание или выражение");
            SyncTo(";");

            if (Current.Lexeme == ";")
                _pos++;

            _context.Pop();
            return ParseResult.Error;
        }

        private ParseResult ParseWhile()
        {

            _context.Push("While");
            if (Current.Lexeme != "while") IronError("while");
            else _pos++;

            _context.Pop();
            _errorInCurrentNode = false; // Сброс после 'while'

            // Парсим условие
            ParseLogicExpr();

            // После выражения парсер может быть в состоянии ошибки. 
            // Если мы видим ':', нужно позволить его распарсить.
            if (Current.Lexeme == ":")
            {
                _errorInCurrentNode = false; // Сброс, чтобы не игнорировать двоеточие
                _pos++;
            }
            else
            {
                _context.Push("Colon");
                IronError(":");
                _context.Pop();
            }

            // --- тело ---
            while (_pos < _tokens.Count &&
                    Current.Lexeme != "while" &&
                    Current.Lexeme != "")
            {
                int before = _pos;

                ParseStmt();

                if (_pos == before)
                    _pos++; // 🔥 защита
            }


            return ParseResult.Ok;
        }


        public void ParseProgram()
        {
            _pos = 0;
            Errors.Clear();

            while (_pos < _tokens.Count && Current.Lexeme != "EOF" && Current.Lexeme != "")
            {
                _errorInCurrentNode = false;
                int before = _pos;

                // Если это "while" ИЛИ (это идентификатор И за ним нет "=")
                if (Current.Lexeme == "while" || (Current.Code == 2 && !IsAssignmentNext()))
                {
                    ParseWhile();
                }
                else
                {
                    ParseStmt();
                }

                if (_pos == before) _pos++;
            }
        }

        // Вспомогательный метод (Lookahead на 1 токен)
        private bool IsAssignmentNext()
        {
            if (_pos + 1 >= _tokens.Count) return false;
            string next = _tokens[_pos + 1].Lexeme;
            return next is "=" or "+=" or "-=" or "*=" or "/=";
        }
        private void Error(Token t, string msg)
        {
            AddErrorOnce(new ParseError(t.Lexeme, t.Line, t.Start, msg));
        }
        private void ErrorRange(int startPos, int endPos, string msg)
        {
            if (startPos < 0 || startPos >= _tokens.Count)
                return;

            if (startPos > endPos)
                endPos = startPos;

            endPos = Math.Min(endPos, _tokens.Count - 1);

            string fragment = "";
            for (int i = startPos; i <= endPos; i++)
                fragment += _tokens[i].Lexeme + " ";

            fragment = fragment.Trim();

            var t = _tokens[startPos];

            AddErrorOnce(new ParseError(fragment, t.Line, t.Start, msg));
        }

        private Token ExpectLexeme(string lex)
        {
            var t = Current;

            if (t.Lexeme == lex)
            {
                _pos++;
                return t;
            }

            Error(t, $"Ожидалось '{lex}'");

           

            return new Token(
                2,
                "fake",
                lex,
                t.Line,
                t.Start,
                t.Start
            );
        }
        private void AddErrorOnce(ParseError err)
        {
            if (_errorInCurrentNode)
                return;

            Errors.Add(err);
            _errorInCurrentNode = true;
        }
    }
}