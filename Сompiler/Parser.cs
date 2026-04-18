using System;
using System.Collections.Generic;

namespace Сompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos = 0;

        private bool _exprError = false;

        private const int MAX_ERRORS = 3;

        public List<ParseError> Errors { get; } = new();

        private Token Current =>
            _pos < _tokens.Count
            ? _tokens[_pos]
            : new Token(0, "EOF", "EOF", 0, 0, 0);

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        // ================= ERROR =================

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
            if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                _pos++;
                return;
            }

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

        // ================= VALUE =================

        private void ParseValue()
        {
            if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                _pos++;
                return;
            }

            if (Current.Code == 2 || Current.Code == 10)
            {
                _pos++;
                return;
            }

            IronError("идентификатор или число", "0");
        }

        // ================= COMPARE =================

        private void ParseCompare()
        {
            // если сразу мусор → выходим
            if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                _pos++;
                return;
            }

            ParseValue();

            // 🔥 ВАЖНО: если уже была ошибка — НЕ продолжаем
            if (_exprError) return;

            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                _pos++;
            }
            else
            {
                IronError("оператор сравнения", "==");
                return; // 🔥 СТОП
            }

            ParseValue();
        }

        // ================= PRIMARY =================

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

            // 🔥 ВОТ ЭТО ГЛАВНОЕ
            if (Current.Lexeme == ")")
            {
                Error("Лишняя ')'");
                _pos++; // 🔥 съели и забыли
                return;
            }

            ParseCompare();
        }

        // ================= FACTOR =================

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

        // ================= TERM =================

        private void ParseTerm()
        {
            ParseFactor();

            while (Current.Lexeme == "and")
            {
                _pos++;
                ParseFactor();
            }
        }

        // ================= COND =================

        private bool ParseCond()
        {
            bool old = _exprError;
            _exprError = false;

            ParseTerm();

            if (_exprError)
            {
                _exprError = old || _exprError;
                return false; // ✅ просто return
            }

            while (Current.Lexeme == "or")
            {
                _pos++;
                ParseTerm();

                if (_exprError) break;
            }

            bool result = !_exprError;
            _exprError = old || _exprError;

            return result;
        }


        // ================= ASSIGN =================

        private void ParseAssign()
        {
            if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                _pos++;
                return;
            }

            if (Current.Code != 2)
            {
                IronError("идентификатор", "x");
                return;
            }

            _pos++; // идентификатор

            // 🔥 БЕЗ if (_exprError) return;

            if (Current.Lexeme is "=" or "+=" or "-=" or "*=" or "/=")
            {
                _pos++;
            }
            else
            {
                IronError("оператор присваивания", "=");
                return;
            }

            // 🔥 БЕЗ if (_exprError) return;

            ParseValue();
        }


        // ================= STMT =================

        private void ParseStmt()
        {
            int beforeErrors = Errors.Count;
            int beforePos = _pos;

            // 🔥 1. если сразу мусор (например @)
            if (Current.Code == 11)
            {
                Error("Недопустимый символ");
                _pos++;

                // синхронизация до конца оператора
                SyncTo(";", "\n", "EOF");
                if (Current.Lexeme == ";")
                    _pos++;

                return;
            }

            // 🔥 2. разбор присваивания
            ParseAssign();

            // 🔥 3. если в процессе была ошибка — НЕ продолжаем
            if (Errors.Count > beforeErrors)
            {
                SyncTo(";", "\n", "EOF");

                if (Current.Lexeme == ";")
                    _pos++;

                return; // 🔥 КЛЮЧЕВОЕ
            }

            // 🔥 4. нормальная проверка ;
            if (Current.Lexeme == ";")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался ';'");

                SyncTo(";", "\n", "EOF");

                if (Current.Lexeme == ";")
                    _pos++;
            }

            // 🔥 5. защита от зависания
            if (_pos == beforePos)
            {
                Error("Неожиданный токен");
                _pos++;
            }
        }

        // ================= WHILE =================

        // ================= WHILE =================
        private void EatGarbage()
        {
            while (Current.Code == 11) // недопустимый символ
            {
                Error("Недопустимый символ");
                _pos++; // съели мусор
            }
        }

        private void ParseWhile()
        {
            _pos++; // съели while

            // ===== УНИВЕРСАЛЬНАЯ ОБРАБОТКА МУСОРА =====
            EatGarbage();

            // ===== ( =====
            bool hasOpenParen = false;
            if (Current.Lexeme == "(")
            {
                hasOpenParen = true;
                _pos++;
                EatGarbage();
            }

            // ===== ИДЕНТИФИКАТОР =====
            if (Current.Code == 2)
            {
                _pos++;
            }
            else
            {
                Error("Ожидался идентификатор");
                _pos++;          // съели мусор
            }
            EatGarbage();

            // ===== ОПЕРАТОР СРАВНЕНИЯ =====
            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался оператор сравнения");
                _pos++;          // съели мусор
            }
            EatGarbage();

            // ===== ЗНАЧЕНИЕ =====
            if (Current.Code == 2 || Current.Code == 10)
            {
                _pos++;
            }
            else
            {
                Error("Ожидался идентификатор или число");
                _pos++;          // съели мусор
            }
            EatGarbage();

            // ===== ) =====
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
            else
            {
                if (Current.Lexeme == ")")
                {
                    Error("Лишняя ')'");
                    _pos++;
                }
            }
            EatGarbage();

            // ===== : =====
            if (Current.Lexeme == ":")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался ':'");
            }
            EatGarbage();

            _exprError = false;
            // ===== ТЕЛО =====
            while (_pos < _tokens.Count &&
                   Current.Lexeme != "while" &&
                   Current.Lexeme != "EOF")
            {
                int before = _pos;
                ParseStmt();

                if (_pos == before)
                {
                    Error("Неожиданный токен в теле");
                    _pos++;
                }
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
                    break;   // ← ВОТ ЭТО ИСПРАВЛЯЕТ ЛИШНЮЮ ОШИБКУ
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
            _pos++; // пропускаем сломанный ключ (например "whil")

            EatGarbage();

            bool hasOpenParen = false;

            // ===== ( =====
            if (Current.Lexeme == "(")
            {
                hasOpenParen = true;
                _pos++;
                EatGarbage();
            }

            // ===== ИДЕНТИФИКАТОР =====
            if (Current.Code == 2)
            {
                _pos++;
            }
            else
            {
                Error("Ожидался идентификатор");
                _pos++;
            }
            EatGarbage();

            // ===== ОПЕРАТОР СРАВНЕНИЯ =====
            if (Current.Lexeme is ">" or "<" or ">=" or "<=" or "==" or "!=")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался оператор сравнения");
                _pos++;
            }
            EatGarbage();

            // ===== ЗНАЧЕНИЕ =====
            if (Current.Code == 2 || Current.Code == 10)
            {
                _pos++;
            }
            else
            {
                Error("Ожидался идентификатор или число");
                _pos++;
            }
            EatGarbage();

            // ===== ) =====
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
            else
            {
                if (Current.Lexeme == ")")
                {
                    Error("Лишняя ')'");
                    _pos++;
                }
            }
            EatGarbage();

            if (Current.Lexeme == ":")
            {
                _pos++;
            }
            else
            {
                Error("Ожидался ':'");
            }
            EatGarbage();
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
                    _pos++;
                }
            }
        }
    }
}