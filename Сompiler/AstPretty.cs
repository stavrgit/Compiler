using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public static class AstPretty
    {
        public static string GetLabel(AstNode node)
        {
            switch (node)
            {
                case WhileNode w:
                    return "While";

                case AssignNode a:
                    return $"Assign {a.Name} {a.Op}";

                case CompareNode c:
                    return $"Compare {c.Op}";

                case IdentifierNode id:
                    return $"Id {id.Name}";

                case IntLiteralNode lit:
                    return $"Int {lit.RawValue}";

                default:
                    return node.GetType().Name;
            }
        }

        public static IEnumerable<AstNode> GetChildren(AstNode node)
        {
            switch (node)
            {
                case WhileNode w:
                    foreach (var c in new AstNode[] { w.Condition }.Where(x => x != null))
                        yield return c;
                    foreach (var s in w.Body)
                        yield return s;
                    break;

                case AssignNode a:
                    if (a.Value != null) yield return a.Value;
                    break;

                case CompareNode c:
                    if (c.Left != null) yield return c.Left;
                    if (c.Right != null) yield return c.Right;
                    break;
            }
        }
    }

}
