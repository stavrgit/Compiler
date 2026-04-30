using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Сompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;

        private bool _exprError = false;

        private const int MAX_ERRORS = 6;

        public List<ParseError> Errors { get; } = new();

        private int _consecutiveSkips = 0;

        private Token Current =>
            _pos < _tokens.Count
            ? _tokens[_pos]
            : new Token(0, "EOF", "EOF", 0, 0, 0);

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }
        private Token Peek(int offset = 1)
        {
            int idx = _pos + offset;
            return idx < _tokens.Count
                ? _tokens[idx]
                : new Token(0, "EOF", "EOF", 0, 0, 0);
        }

        private void Error(string msg)
        {
            if (Errors.Count >= MAX_ERRORS)
                return;

            var t = Current;
            Errors.Add(new ParseError(t.Lexeme, t.Line, t.Start, msg));
            _exprError = true;
        }
        private void HandleExtraToken(string msg, int consume = 1, bool sync = false, params string[] syncTo)
        {
            Error(msg);
            for (int i = 0; i < consume && _pos < _tokens.Count; i++)
                Accept();

            if (sync && syncTo != null && syncTo.Length > 0)
                SyncTo(syncTo);
        }
        private void InsertFake(string lexeme)
        {
            var fake = new Token(
                999,
                "FAKE",
                lexeme,
                Current.Line,
                Current.Start,
                Current.Start
            );

            _tokens.Insert(_pos, fake);
        }

        private void IronError(string expected, string fake)
        {
            if (Current.Code == 11)
            {
                EatGarbage();
                return;
            }

            Error($"Ожидался {expected}");
            InsertFake(fake);
            Accept();
        }

        private void Accept()
        {
            if (_pos < _tokens.Count) _pos++;
            _consecutiveSkips = 0;
        }

        private void AdvanceSkip()
        {
            if (_pos < _tokens.Count)
            {
                _pos++;
                _consecutiveSkips++;
            }
        }
        private void InsertAndAcceptFake(string lexeme)
        {
            InsertFake(lexeme);
            Accept(); 
        }
        private void SyncTo(params string[] lexemes)
        {
            int skipped = 0;
            while (_pos < _tokens.Count &&
                   Array.IndexOf(lexemes, Current.Lexeme) == -1 &&
                   Current.Lexeme != "EOF")
            {
                if (skipped >= 3)
                {
                    InsertAndAcceptFake(lexemes.Length > 0 ? lexemes[0] : "EOF");
                    return;
                }

                AdvanceSkip();
                skipped++;
            }
            _consecutiveSkips = 0;
        }
        private void ParseValue()
        {
            if (Current.Code == 11)
            {
                EatGarbage();
                return;
            }

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                Error("Ожидался идентификатор или число, найден оператор сравнения");
                Accept();
                return;
            }

            if (Current.Code == 2 || Current.Code == 10)
            {
                Accept();
                return;
            }

            IronError("идентификатор или число", "0");
        }
        private void ParseCompare()
        {
            ParseValue();

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                Accept();
            }
            else
            {
                IronError("оператор сравнения", "==");
                return;
            }

            ParseValue();
        }
        private void ParsePrimary()
        {
            if (Current.Lexeme == "(")
            {
                Accept();

                ParseCond();

                if (Current.Lexeme == ")")
                {
                    Accept();
                }
                else
                {
                    IronError("')'", ")");
                }

                return;
            }

            if (Current.Lexeme == ")")
            {
                Error("Лишняя ')'");
                Accept();
                return;
            }

            ParseCompare();
        }
        private void ParseFactor()
        {
            if (Current.Lexeme == "not")
            {
                Accept();
                ParseFactor();
                return;
            }

            ParsePrimary();
        }
        private void ParseTerm()
        {
            ParseFactor();

            while (Current.Lexeme == "and")
            {
                Accept();
                ParseFactor();
            }
        }
        private bool ParseCond()
        {
            bool old = _exprError;
            _exprError = false;

            ParseTerm();

            if (_exprError)
            {
                _exprError = old || _exprError;
                return false;
            }

            while (Current.Lexeme == "or")
            {
                Accept();
                ParseTerm();

                if (_exprError) break;
            }

            bool result = !_exprError;
            _exprError = old || _exprError;

            return result;
        }
        private bool ParseAssign()
        {
            bool ok = true;

            if (Current.Code != 2)
            {
                IronError("идентификатор", "x");
                ok = false; 
            }
            else
            {
                Accept(); 
            }

            if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
            {
                Accept();
            }
            else
            {
                IronError("оператор присваивания", "=");
                ok = false;
            }

            ParseValue(); 

            return ok;
        }
        private void ParseStmt()
        {
            int before = _pos;
            int beforeErrors = Errors.Count;

            bool assignOk = ParseAssign();

            if (!assignOk || Errors.Count > beforeErrors)
            {
                SyncTo(";", "\n", "EOF");
                if (Current.Lexeme == ";") Accept();
                return;
            }

            if (_pos == before)
            {
                IronError("токен", "x");
                return;
            }

            if (Current.Lexeme == ";")
            {
                ConsumeDuplicates(";", "Лишний токен ';'");
            }
            else
            {
                Error("Ожидался ';'");
            }
        }
        private void ConsumeDuplicates(string lexeme, string errorMsg)
        {
            if (Current.Lexeme != lexeme)
                return;

            Accept(); 

            if (Current.Lexeme == lexeme)
            {
                Error(errorMsg);

                while (Current.Lexeme == lexeme)
                    Accept();
            }
        }
        private void EatGarbage()
        {
            while (Current.Code == 11)
            {
                Error("Недопустимый символ");
                Accept();
            }
        }
        private void ParseWhile()
        {
            Accept();
            EatGarbage(); 

            if (Current.Lexeme == "EOF")
            {
                IronError("идентификатор", "x");
                return;
            }

            bool looksLikeStmtStart = Current.Code == 2 &&
                                      _pos + 1 < _tokens.Count &&
                                      (_tokens[_pos + 1].Lexeme is "=" or "+=" or "-=" or "*=" or "/=");

            if (looksLikeStmtStart)
            {
                Error("Ожидался заголовок while (идентификатор <op> значение)");
                goto PARSE_BODY;
            }

            if (Current.Code == 2)
            {
                Accept();
                EatGarbage(); 
            }
            else if (Current.Code == 11)
            {
                EatGarbage();
                SyncTo(":", "EOF");
                if (Current.Lexeme == ":") Accept();
                return;
            }
            else
            {
                IronError("идентификатор", "x");
                EatGarbage();
            }

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                if (Current.Lexeme == ">")
                    ConsumeDuplicates(">", "Лишний оператор '>'");
                else if (Current.Lexeme == "<")
                    ConsumeDuplicates("<", "Лишний оператор '<'");
                else
                    Accept(); 
            }
            else if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                Accept();
            }
            else
            {
                HandleExtraToken("Ожидался оператор сравнения");
            }

            if (Current.Code == 2 || Current.Code == 10)
            {
                Accept();
                EatGarbage(); 
            }
            else if (Current.Code == 11)
            {
                EatGarbage();
                SyncTo(":", "EOF");
                if (Current.Lexeme == ":") Accept();
                return;
            }
            else
            {
                IronError("идентификатор или число", "0");
                EatGarbage();
            }

            if (Current.Lexeme == ":")
            {
                ConsumeDuplicates(":", "Лишний токен ':'");
                EatGarbage();
            }
            else
            {
                IronError("':'", ":");
            }
        PARSE_BODY:
            while (_pos < _tokens.Count && Current.Lexeme != "EOF")
            {
                int before = _pos;
                ParseStmt();

                if (_pos == before)
                {
                    IronError("токен", "x");
                    Accept();
                }
            }
        }

        public void ParseProgram()
        {
            _pos = 0;
            Errors.Clear();
            _exprError = false;
            _consecutiveSkips = 0;

            if (Current.Lexeme == "EOF")
                return;

            if (Current.Lexeme == "while")
            {
                ParseWhile();
                return;
            }

            if (Current.Code == 2 && Current.Lexeme.StartsWith("w"))
            {
                Error("Ожидалось 'while'"); 
                ParseBrokenWhileHeader();
                if (Current.Lexeme != "EOF")
                    ParseBrokenWhileBody();
                return;
            }

            Error("Ожидалось 'while'"); 
            return;
        }

        private void ParseBrokenWhileHeader()
        {
            Accept(); 
            EatGarbage();

            bool looksLikeStmtStart =
                Current.Code == 2 &&
                _pos + 1 < _tokens.Count &&
                (_tokens[_pos + 1].Lexeme is "=" or "+=" or "-=" or "*=" or "/=");

            if (looksLikeStmtStart)
            {
                Error("Ожидался заголовок while (идентификатор <op> значение)");
                goto EXPECT_COLON;
            }

            if (Current.Code == 2)
            {
                Accept();
                EatGarbage();
            }
            else if (Current.Code == 11)
            {
                EatGarbage();
                goto EXPECT_COLON; 
            }
            else
            {
                IronError("идентификатор", "x");
                EatGarbage();
            }

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                if (Current.Lexeme == ">")
                    ConsumeDuplicates(">", "Лишний оператор '>'");
                else if (Current.Lexeme == "<")
                    ConsumeDuplicates("<", "Лишний оператор '<'");
                else
                    Accept();

                EatGarbage();
            }
            else if (Current.Code == 11)
            {
                EatGarbage();
                goto EXPECT_COLON;
            }
            else
            {
                HandleExtraToken("Ожидался оператор сравнения");
                EatGarbage();
            }

            if (Current.Code == 2 || Current.Code == 10)
            {
                Accept();
                EatGarbage();
            }
            else if (Current.Code == 11)
            {
                EatGarbage();
                goto EXPECT_COLON;
            }
            else
            {
                IronError("идентификатор или число", "0");
                EatGarbage();
            }

        EXPECT_COLON:

            if (Current.Lexeme == ":")
            {
                ConsumeDuplicates(":", "Лишний токен ':'");
                EatGarbage();
            }
            else
            {
                IronError("':'", ":");
            }
        }
        private void ParseBrokenWhileBody()
        {
            while (_pos < _tokens.Count &&
                   Current.Lexeme != "while" &&
                   Current.Lexeme != "EOF")
            {
                int before = _pos;

                ParseStmt();

                if (_pos == before)
                {
                    Error("Неожиданный токен в теле");
                    Accept();
                }
            }
        }
    }
}
