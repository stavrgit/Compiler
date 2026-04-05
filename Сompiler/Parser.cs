using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;

namespace Сompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;
        private bool _inStmtExpr = false;
        public List<ParseError> Errors { get; } = new();
        private Token Current => _pos < _tokens.Count ? _tokens[_pos] : new Token(0, "EOF", "", 0, 0, 0);
        private enum ParseResult { Ok, Error }
        private readonly Stack<string> _context = new();
        private bool _errorInCurrentNode = false;
        private bool _inWhileCondition = false;


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
            if (_pos < _tokens.Count) _pos++;

            string broken = _context.Count > 0 ? _context.Peek() : "Factor";
            string q = BuildQ(broken);
            InsertQ(q);

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

            string lex = Current.Lexeme;

            if ((lex == ":" && !_inWhileCondition) ||
                (lex == ";" && !_inStmtExpr) ||
                lex == ")")
            {
                if (!_errorInCurrentNode)
                    Error(Current, "Ожидался оператор");

                _context.Pop();
                return ParseResult.Ok;
            }

            if ((lex == ":" && _inWhileCondition) ||
                (lex == ";" && _inStmtExpr))
            {
                _context.Pop();
                return ParseResult.Ok;
            }

            if (lex == "")
            {
                _context.Pop();
                return ParseResult.Ok;
            }

            if (!IsAnyOperator(Current))
            {
                if (!_errorInCurrentNode)
                    IronError("оператор");

                _errorInCurrentNode = true;
                _context.Pop();
                return ParseResult.Ok;
            }

            _pos++;
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

                    _inStmtExpr = true;
                    var r = ParseLogicExpr();
                    _inStmtExpr = false;

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

            _inStmtExpr = true;
            var r2 = ParseLogicExpr();
            _inStmtExpr = false;

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
            _errorInCurrentNode = false; 

            ParseLogicExpr();

            if (Current.Lexeme == ":")
            {
                _errorInCurrentNode = false; 
                _pos++;
            }
            else
            {
                _context.Push("Colon");
                IronError(":");
                _context.Pop();
            }

            while (_pos < _tokens.Count &&
                    Current.Lexeme != "while" &&
                    Current.Lexeme != "")
            {
                int before = _pos;

                ParseStmt();

                if (_pos == before)
                    _pos++; 
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
        private void AddErrorOnce(ParseError err)
        {
            if (_errorInCurrentNode)
                return;

            Errors.Add(err);
            _errorInCurrentNode = true;
        }
    }
}