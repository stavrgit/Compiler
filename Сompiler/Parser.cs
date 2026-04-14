using System;
using System.Collections.Generic;

namespace Сompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;
        private bool _exprError = false;
        private const int MAX_ERRORS = 4;

        public List<ParseError> Errors { get; } = new();

        private Token Current =>
            _pos < _tokens.Count
            ? _tokens[_pos]
            : new Token(0, "EOF", "EOF", 0, 0, 0);

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private void Error(string msg)
        {
            if (Errors.Count >= MAX_ERRORS)
                return;

            var t = Current;
            Errors.Add(new ParseError(t.Lexeme, t.Line, t.Start, msg));
            _exprError = true;
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
            Error($"Ожидался {expected}");

            InsertFake(fake);
            _pos++; 
        }

        private void SyncTo(params string[] lexemes)
        {
            while (_pos < _tokens.Count &&
                   Array.IndexOf(lexemes, Current.Lexeme) == -1)
            {
                _pos++;
            }
        }
        private void ParseValue()
        {
            if (Current.Code == 2 || Current.Code == 10)
            {
                _pos++;
                return;
            }

            IronError("идентификатор или число", "0");
        }
        private void ParseCompare()
        {
            ParseValue();

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                _pos++;
            }
            else
            {
                IronError("оператор сравнения", "==");
            }

            ParseValue();
        }
        private void ParsePrimary()
        {
            if (Current.Lexeme == "(")
            {
                _pos++;

                ParseCond();

                if (Current.Lexeme == ")")
                {
                    _pos++;
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
                _pos++; 
            }

            ParseCompare();
        }
        private void ParseFactor()
        {
            if (Current.Lexeme == "not")
            {
                _pos++;
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
                _pos++;
                ParseFactor();
            }
        }
        private bool ParseCond()
        {
            if (Current.Lexeme == ":" || Current.Lexeme == "\n" || Current.Lexeme == "EOF")
                return false;

            bool old = _exprError;
            _exprError = false;

            ParseTerm();

            while (Current.Lexeme == "or")
            {
                _pos++;
                ParseTerm();
                if (Current.Lexeme == ":" || Current.Lexeme == "\n" || Current.Lexeme == "EOF")
                    break;
            }

            bool result = !_exprError;
            _exprError = old || _exprError;

            return result;
        }
        private void ParseAssign()
        {
            if (Current.Code != 2)
            {
                IronError("идентификатор", "x");
                return;
            }

            _pos++;

            if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
            {
                _pos++;
            }
            else
            {
                IronError("оператор присваивания", "=");
            }

            ParseValue();
        }
        private void ParseStmt()
        {
            int beforeErrors = Errors.Count;

            if (Current.Lexeme == ")")
            {
                Error("Лишняя ')'");
                _pos++;
                return;
            }

            ParseAssign();

            if (Errors.Count > beforeErrors)
            {
                SyncTo(";", "\n", "EOF");
            }

            if (Current.Lexeme == ";")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался ';'");
            }
        }
        // ================= WHILE =================

        private void ParseWhile()
        {
            _pos++; 
            bool hasOpenParen = false;
            bool hasCloseParen = false;

            if (Current.Lexeme == "(")
            {
                hasOpenParen = true;
                _pos++; 
            }

            int beforeCondPos = _pos;
            bool oldError = _exprError;
            _exprError = false;

            if (Current.Code == 2)
                _pos++;
            else
                IronError("идентификатор", "x");

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
                _pos++;
            else
                IronError("оператор сравнения", "==");

            if (Current.Code == 2 || Current.Code == 10)
                _pos++;
            else
                IronError("идентификатор или число", "0");

            bool condOk = !_exprError;
            _exprError = oldError || _exprError;

            if (hasOpenParen)
            {
                if (Current.Lexeme == ")")
                {
                    hasCloseParen = true;
                    _pos++; 
                }
                else
                {
                    Error("Ожидалась ')'");
                }
            }
            else
            {
                if (Current.Lexeme == ")")
                {
                    Error("Лишняя ')'");
                    _pos++; 
                }
            }

            if (Current.Lexeme == ":")
                _pos++;
            else
                Error("Ожидался ':'");

            while (Current.Lexeme == ")")
            {
                Error("Лишняя ')'");
                _pos++;
            }

            while (_pos < _tokens.Count &&
                   Current.Lexeme != "while" &&
                   Current.Lexeme != "EOF")
            {
                int before = _pos;
                ParseStmt();
                if (_pos == before)
                    _pos++;
            }
        }
        public void ParseProgram()
        {
            _pos = 0;
            Errors.Clear();

            while (_pos < _tokens.Count && Current.Lexeme != "EOF")
            {
                if (Errors.Count >= MAX_ERRORS)
                    break;

                int beforeTop = _pos;

                if (Current.Lexeme == "while")
                {
                    ParseWhile();
                    break;  
                }
                else
                {
                    Error("Ожидалось 'while'");

                    if (Current.Code == 2 && Current.Lexeme.StartsWith("w"))
                    {
                        ParseBrokenWhileHeader();
                        ParseBrokenWhileBody();
                        break;
                    }

                    break;
                }

                if (_pos == beforeTop)
                    _pos++;
            }
        }

        private void ParseBrokenWhileHeader()
        {
            _pos++;
            bool hasOpenParen = false;

            if (Current.Lexeme == "(")
            {
                hasOpenParen = true;
                _pos++;
            }

            if (Current.Code == 2)
            {
                _pos++;
            }
            else
            {
                IronError("идентификатор", "x");
            }

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                _pos++;
            }
            else
            {
                IronError("оператор сравнения", "==");
            }

            if (Current.Code == 2 || Current.Code == 10)
            {
                _pos++;
            }
            else
            {
                IronError("идентификатор или число", "0");
            }

            if (hasOpenParen)
            {
                if (Current.Lexeme == ")")
                {
                    _pos++; 
                }
                else
                {
                    Error("Ожидалась ')'");
                }
            }
            else if (Current.Lexeme == ")")
            {
                Error("Лишняя ')'");
                _pos++;
            }

            if (Current.Lexeme == ":")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался ':'");
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
                    _pos++;
            }
        }
    }
}