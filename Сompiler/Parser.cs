using System;
using System.Collections.Generic;
using System.Linq;

namespace Сompiler
{
    public class Parser
    {
        private List<Token> tokens;
        private int pos = 0;

        private List<string> poliz = new();
        private List<Quad> quads = new();
        public List<ParseError> Errors { get; } = new();
        private bool _exprError = false;

        private const int MAX_ERRORS = 6;
        private int tempCounter = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Token Current => pos < tokens.Count ? tokens[pos] : tokens[^1];
        private void Next() => pos++;

        private void Error(string msg)
        {
            if (Errors.Count >= MAX_ERRORS)
                return;

            var t = Current;
            Errors.Add(new ParseError(t.Lexeme, t.Line, t.Start, msg));
            _exprError = true;
        }
        private string NewTemp() => $"t{tempCounter++}";

        public string ParseE()
        {
            string t = ParseT();
            return ParseA(t);
        }

        private string ParseA(string inherited)
        {
            if (pos >= tokens.Count) return inherited;

            if (Current.Lexeme == "+" || Current.Lexeme == "-")
            {
                string op = Current.Lexeme;
                Next();

                if (pos >= tokens.Count)
                {
                    Error("После оператора ожидается выражение");
                    return inherited;
                }

                string t = ParseT();
                poliz.Add(op);

                string temp = NewTemp();
                quads.Add(new Quad(op, inherited, t, temp));

                return ParseA(temp);
            }

            return inherited;
        }

        private string ParseT()
        {
            string f = ParseF();
            return ParseB(f);
        }
        private string ParseB(string inherited)
        {
            while (Current.Lexeme == "*" || Current.Lexeme == "/" ||
                   Current.Lexeme == "//" || Current.Lexeme == "%" ||
                   Current.Lexeme == "**")
            {
                string op = Current.Lexeme;
                Next();

                if (pos >= tokens.Count)
                {
                    Error("После оператора ожидается выражение");
                    return inherited;
                }
                string f = ParseF();

                poliz.Add(op);

                string temp = NewTemp();
                quads.Add(new Quad(op, inherited, f, temp));

                inherited = temp;
            }

            return inherited;
        }
        private string ParseF()
        {
            if (Current.Type == "num")
            {
                poliz.Add(Current.Lexeme);
                string val = Current.Lexeme;
                Next();
                return val;
            }

            if (Current.Type == "id")
            {
                poliz.Add(Current.Lexeme);
                string val = Current.Lexeme;
                Next();
                return val;
            }

            if (Current.Lexeme == "(")
            {
                Next();
                string e = ParseE();

                if (Current.Lexeme != ")")
                {
                    Error("Ожидалась )");
                    return ""; 
                }
                Next();
                return e;
            }

            Error("Ожидалось число, id или (");
            if (pos < tokens.Count) Next();
            return "";
        } 

        public void Parse()
        {
            foreach (var t in tokens.Where(x => x.Type == "ошибка"))
            {
                Errors.Add(new ParseError(t.Lexeme, t.Line, t.Start, "Недопустимый символ"));
            }

            if (Errors.Count > 0)
                return; 

            ParseE();

            if (pos < tokens.Count)
            {
                Error("Лишние символы");
                return;
            }
        }

        public List<string> GetPoliz() => poliz;
        public List<Quad> GetQuads() => quads;

        public int EvaluatePoliz()
        {
            Stack<int> stack = new();

            foreach (var t in poliz)
            {
                if (int.TryParse(t, out int num))
                {
                    stack.Push(num);
                }
                else if (t is "+" or "-" or "*" or "/" or "%" or "//" or "**")
                {
                    if (stack.Count < 2)
                    {
                        Errors.Add(new ParseError(t, 0, 0, "Недостаточно операндов для операции"));
                        return 0;
                    }

                    int b = stack.Pop();
                    int a = stack.Pop();

                    int res = t switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "%" => a % b,
                        "//" => a / b,
                        "**" => (int)Math.Pow(a, b),
                        _ => 0
                    };

                    stack.Push(res);
                }
                else
                {
                    Errors.Add(new ParseError(t, 0, 0, "Недопустимый символ в ПОЛИЗ"));
                    return 0;
                }
            }

            return stack.Count > 0 ? stack.Pop() : 0;
        }

    }
}