using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    internal class Semantica
    {
        public sealed class SymbolTable
        {
            private readonly Dictionary<string, int> vars = new();

            public bool Declare(string name)
            {
                if (vars.ContainsKey(name))
                    return false;

                vars[name] = 0;
                return true;
            }

            public bool Exists(string name) => vars.ContainsKey(name);
        }

        public sealed class SemanticAnalyzer
        {
            public List<string> Errors { get; } = new();
            private readonly SymbolTable symbols = new();

            public void Analyze(WhileNode node)
            {
                CheckExpr(node.Condition);

                foreach (var stmt in node.Body)
                {
                    if (stmt is StmtNode s)
                        CheckStmt(s);
                    else
                        throw new InvalidOperationException("В Body оказался не StmtNode");
                }
            }

            private void CheckStmt(StmtNode stmt)
            {
                if (stmt is AssignNode a)
                {
                    CheckExpr(a.Value);
                }

            }

            private void CheckExpr(ExprNode expr)
            {
                switch (expr)
                {
                    case IdentifierNode id:
                        break;

                    case IntLiteralNode lit:
                        if (!BigInteger.TryParse(lit.RawValue, out var value))
                        {
                            Errors.Add($"Ошибка: некорректное число '{lit.RawValue}' (строка {lit.Line})");
                        }
                        else if (value < -32768 || value > 32767)
                        {
                            Errors.Add($"Ошибка: значение {lit.RawValue} вне диапазона (строка {lit.Line})");
                        }
                        break;

                    case CompareNode cmp:
                        CheckExpr(cmp.Left);
                        CheckExpr(cmp.Right);
                        break;
                }
            }
        }
    }
}
