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
            _pos < _tokens.Count ? _tokens[_pos] : _tokens[_tokens.Count - 1];

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            Console.WriteLine("=== USING NEW PARSER ===");
        }

        // ---------------------------------------------------------
        //  ВХОДНАЯ ТОЧКА
        // ---------------------------------------------------------
        public void ParseProgram()
        {
            ParseWhile();
        }

        // ---------------------------------------------------------
        //  while <LogicExpr> : <StmtList>
        // ---------------------------------------------------------
        private bool ParseWhile()
        {
            int start = _pos;

            if (Current.Lexeme != "while")
            {
                while (_pos < _tokens.Count &&
                       Current.Lexeme != "while" &&
                       Current.Lexeme != ":")
                    _pos++;

                int end = Math.Max(start, _pos - 1);
                ErrorRange(start, end, "Ожидалось 'while'");
                return false;
            }

            ExpectLexeme("while");
            ExpectSpaceAfterKeyword();

            if (!ParseLogicExpr())
            {
                SyncTo(":");
                if (Current.Lexeme == ":")
                    _pos++;
                return false;
            }

            ExpectLexeme(":");

            ParseStmtList();
            return true;
        }

        // ---------------------------------------------------------
        //  ЛОГИКА (and/or/not и &&/||/!)
        // ---------------------------------------------------------
        private bool ParseLogicExpr()
        {
            return ParseOrExpr();
        }



        private bool ParseOrExpr()
        {
            int start = _pos;

            if (!ParseAndExpr())
            {
                ErrorRange(start, _pos - 1, "Ошибка в логическом выражении");
                return false;
            }

            while (Current.Lexeme == "or" || Current.Lexeme == "||" || Current.Code == 31)
            {
                _pos++;
                if (!ParseAndExpr())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в логическом выражении");
                    return false;
                }
            }

            return true;
        }

        private bool ParseAndExpr()
        {
            int start = _pos;

            if (!ParseRelExpr())
            {
                ErrorRange(start, _pos - 1, "Ошибка в логическом выражении");
                return false;
            }

            while (Current.Lexeme == "and" || Current.Lexeme == "&&" || Current.Code == 30)
            {
                _pos++;
                if (!ParseRelExpr())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в логическом выражении");
                    return false;
                }
            }

            return true;
        }

        private bool ParseRelExpr()
        {
            int start = _pos;

            if (!ParseAddExpr())
            {
                ErrorRange(start, _pos - 1, "Ошибка в условии");
                return false;
            }

            if (IsCompareOp(Current))
            {
                _pos++;
                if (!ParseAddExpr())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в условии");
                    return false;
                }
            }

            return true;
        }

        private bool IsCompareOp(Token t)
        {
            return t.Lexeme is "<" or ">" or "<=" or ">=" or "==" or "!=";
        }

        // ---------------------------------------------------------
        //  АРИФМЕТИКА
        // ---------------------------------------------------------
        private bool ParseAddExpr()
        {
            int start = _pos;

            if (!ParseMulExpr())
            {
                ErrorRange(start, _pos - 1, "Ошибка в арифметическом выражении");
                return false;
            }

            while (Current.Lexeme is "+" or "-")
            {
                _pos++;
                if (!ParseMulExpr())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в арифметическом выражении");
                    return false;
                }
            }

            return true;
        }

        private bool ParseMulExpr()
        {
            int start = _pos;

            if (!ParseFactor())
            {
                ErrorRange(start, _pos - 1, "Ошибка в арифметическом выражении");
                return false;
            }

            while (Current.Lexeme is "*" or "/")
            {
                _pos++;
                if (!ParseFactor())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в арифметическом выражении");
                    return false;
                }
            }

            return true;
        }

        private bool ParseFactor()
        {
            var t = Current;

            if (t.Lexeme == "not" || t.Lexeme == "!" || t.Code == 32)
            {
                _pos++;
                return ParseFactor();
            }

            if (t.Lexeme == "(")
            {
                int start = _pos;
                _pos++;

                if (!ParseLogicExpr())
                {
                    ErrorRange(start, _pos - 1, "Ошибка в логическом выражении");
                    return false;
                }

                if (Current.Lexeme == ")")
                {
                    _pos++;
                    return true;
                }

                ErrorRange(start, _pos - 1, "Ожидалась ')'");
                return false;
            }

            if (t.Code == 2 || t.Code == 10)
            {
                _pos++;
                return true;
            }

            Error(t, "Ожидался идентификатор или число");
            return false;
        }

        // ---------------------------------------------------------
        //  СПИСОК ОПЕРАТОРОВ
        // ---------------------------------------------------------
        private void ParseStmtList()
        {
            while (_pos < _tokens.Count &&
                   Current.Lexeme != "" &&
                   Current.Lexeme != "while")
            {
                ParseStmt();
            }
        }

        // ---------------------------------------------------------
        //  ОПЕРАТОР
        // ---------------------------------------------------------
        private bool ParseStmt()
        {
            int start = _pos;

            if (Current.Code != 2)
            {
                Error(Current, "Ожидался идентификатор");
                SyncTo(";");
                if (Current.Lexeme == ";") _pos++;
                return false;
            }

            _pos++;

            if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
            {
                _pos++;

                if (Current.Code is 2 or 10)
                {
                    _pos++;
                }
                else
                {
                    Error(Current, "Ожидался идентификатор или число");
                    SyncTo(";");
                    if (Current.Lexeme == ";") _pos++;
                    return false;
                }

                ExpectLexeme(";");
                return true;
            }

            Error(Current, "Ожидалось '=' или '+=' или '-=' или '*=' или '/='");
            SyncTo(";");
            if (Current.Lexeme == ";") _pos++;
            return false;
        }

        // ---------------------------------------------------------
        //  EXPECT
        // ---------------------------------------------------------
        private Token ExpectLexeme(string lex)
        {
            var t = Current;

            if (t.Lexeme == lex)
            {
                _pos++;
                return t;
            }

            Error(t, $"Ожидалось '{lex}'");
            return InsertFake(lex, t.Code, "fake");
        }

        private void ExpectSpaceAfterKeyword()
        {
            if (_pos == 0 || _pos >= _tokens.Count)
                return;

            var kw = _tokens[_pos - 1];
            var next = Current;

            if (next.Line == kw.Line && next.Start != kw.End + 2)
                Error(next, "После ключевого слова должен быть пробел");
        }

        // ---------------------------------------------------------
        //  СИНХРОНИЗАЦИЯ
        // ---------------------------------------------------------
        private void SyncTo(params string[] lexemes)
        {
            while (_pos < _tokens.Count &&
                   Array.IndexOf(lexemes, Current.Lexeme) == -1)
                _pos++;
        }

        // ---------------------------------------------------------
        //  ОШИБКИ
        // ---------------------------------------------------------
        private void Error(Token t, string msg)
        {
            Errors.Add(new ParseError(t.Lexeme, t.Line, t.Start, msg));
        }

        private void ErrorRange(int startPos, int endPos, string msg)
        {
            if (startPos < 0 || startPos >= _tokens.Count)
                return;


            endPos = Math.Min(endPos, _tokens.Count - 1);

            string fragment = "";
            for (int i = startPos; i <= endPos; i++)
                fragment += _tokens[i].Lexeme + " ";

            fragment = fragment.Trim();

            var t = _tokens[startPos];
            Errors.Add(new ParseError(fragment, t.Line, t.Start, msg));

            if (startPos > endPos)
            {
                // показываем хотя бы текущий токен
                startPos = endPos = Math.Min(_pos, _tokens.Count - 1);
            }
            if (string.IsNullOrWhiteSpace(fragment))
                fragment = _tokens[startPos].Lexeme;
        }

        private Token InsertFake(string lexeme, int code, string type)
        {
            return new Token(
                code,
                type,
                lexeme,
                Current.Line,
                Current.Start,
                Current.Start
            );
        }
    }
}
