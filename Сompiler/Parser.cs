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
            _pos < _tokens.Count ? _tokens[_pos] : _tokens[_tokens.Count - 1];

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
            Token j = Current;

            var bad = Current;
            while (_pos < _tokens.Count &&
                   _tokens[_pos].Lexeme == bad.Lexeme &&
                   _tokens[_pos].Code == bad.Code)
            {
                _pos++;
            }

            int endPos = _pos - 1;

            string broken = _context.Count > 0 ? _context.Peek() : "Factor";
            string q = BuildQ(broken);

            InsertQ(q);

            ErrorRange(startPos, endPos, $"Ожидался {expected}");

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
        private bool IsCompareOp(Token t)
        {
            return t.Lexeme is "<" or ">" or "<=" or ">=" or "==" or "!=";
        }

        private ParseResult ParseRelExpr()
        {
            _context.Push("RelExpr");

            var r = ParseAddExpr();
            if (r == ParseResult.Error)
            {
                _context.Pop();
                return ParseResult.Error;
            }

            if (IsCompareOp(Current))
            {
                _pos++;
                r = ParseAddExpr();
                if (r == ParseResult.Error)
                {
                    _context.Pop();
                    return ParseResult.Error;
                }
            }

            _context.Pop();
            return ParseResult.Ok;
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
            _errorInCurrentNode = false;

            int start = _pos; 

            if (Current.Code == 2)
            {
                _pos++;

                if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
                {
                    _pos++;

                    _context.Push("LogicExpr");
                    var r = ParseLogicExpr();
                    _context.Pop();

                    if (r == ParseResult.Error)
                    {
                        SyncTo(";");
                        if (Current.Lexeme == ";") _pos++;
                        return ParseResult.Error;
                    }

                    ExpectLexeme(";");
                    return ParseResult.Ok;
                }

                _pos = start;

                _context.Push("LogicExpr");
                if (ParseLogicExpr() == ParseResult.Ok)
                {
                    _context.Pop();
                    ExpectLexeme(";");
                    return ParseResult.Ok;
                }
                _context.Pop();

                _pos = start;
                ErrorRange(start, start, "Ожидалось присваивание или выражение");
                SyncTo(";");
                if (Current.Lexeme == ";") _pos++;
                return ParseResult.Error;
            }

            ErrorRange(start, start, "Ожидался идентификатор");
            SyncTo(";");
            if (Current.Lexeme == ";") _pos++;
            return ParseResult.Error;
        }

        private ParseResult ParseWhile()
        {
            _errorInCurrentNode = false;

            if (Current.Lexeme != "while")
            {
                int start = _pos;
                SyncTo("while", "");
                int end = _pos - 1;
                if (end < start) end = start;

                ErrorRange(start, end, "Ожидалось 'while'");
                return ParseResult.Ok;
            }

            _pos++;

            _context.Push("LogicExpr");
            var r = ParseLogicExpr();
            _context.Pop();

            if (r == ParseResult.Error)
            {
                IronError("логическое выражение");
                SyncTo(":");
            }

            if (Current.Lexeme == ":")
                _pos++;
            else
            {
                ErrorRange(_pos, _pos, "Ожидалось ':'");
                SyncTo("+=", "-=", "*=", "/=", ";", "while");
            }

            while (_pos < _tokens.Count &&
                   Current.Lexeme != "while" &&
                   Current.Lexeme != "")
            {
                ParseStmt();
            }

            return ParseResult.Ok;
        }
        public void ParseProgram()
        {
            _pos = 0;      
            Errors.Clear(); 

            while (_pos < _tokens.Count && Current.Lexeme != "")
            {
                if (Current.Lexeme == "while")
                {
                    var r = ParseWhile();
                    if (r == ParseResult.Error)
                        return;
                }
                else
                {
                    int startPos = _pos;
                    SyncTo("while", "");

                    int endPos = _pos - 1;
                    if (endPos < startPos) endPos = startPos;

                    ErrorRange(startPos, endPos, "Ожидалось 'while'");
                }

            }
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