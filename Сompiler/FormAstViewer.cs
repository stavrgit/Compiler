using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Сompiler
{
    public class FormAstViewer : Form
    {
        private readonly AstNode _root;
        private readonly Font _font = new Font("Consolas", 10);

        public FormAstViewer(AstNode root)
        {
            _root = root;
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_root == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // рисуем дерево
            DrawNode(g, _root, this.ClientSize.Width / 2, 20);
        }

        private void DrawNode(Graphics g, AstNode node, int x, int y)
        {
            if (node is TerminalNode t)
            {
                int childY = y + 40; // терминал ближе к родителю
                var size = g.MeasureString(t.Symbol, _font);
                int w = (int)size.Width + 20;
                var rect = new Rectangle(x - w / 2, childY, w, 30);

                g.FillRectangle(Brushes.White, rect);
                g.DrawRectangle(Pens.Black, rect);
                g.DrawString($"\"{t.Symbol}\"", _font, Brushes.Black, rect,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                g.DrawLine(Pens.Black, x, y + 30, x, childY);
                return;
            }


            // ===== НЕ-ТЕРМИНАЛ =====
            if (node is NonTerminalNode nt)
            {
                DrawBox(g, nt.Name, x, y);

                if (nt.Child is AstNode childNode)
                {
                    g.DrawLine(Pens.Black, x, y + 30, x, y + 120);
                    DrawNode(g, childNode, x, y + 120);
                }
                else if (nt.Child is List<StmtNode> list)
                {
                    DrawStmtList(g, list, x, y);
                }
                return;
            }

            // ===== ОБЫЧНЫЙ УЗЕЛ =====
            string label = GetLabel(node);
            DrawBox(g, label, x, y);

            var children = GetChildren(node).ToList();
            if (children.Count == 0) return;

            int totalWidth = children.Sum(c => MeasureWidth(c));
            int startX = x - totalWidth / 2;

            foreach (var child in children)
            {
                int w = MeasureWidth(child);
                int childX = startX + w / 2;
                int childY = y + 120;

                // линия от центра родителя к центру верхней границы ребёнка
                g.DrawLine(Pens.Black, x, y + 30, childX, childY);

                // теперь рисуем ребёнка
                DrawNode(g, child, childX, childY);

                startX += w;
            }

        }


        // ===== ВСПОМОГАТЕЛЬНЫЙ МЕТОД ДЛЯ СПИСКА =====
        private void DrawStmtList(Graphics g, List<StmtNode> list, int x, int y)
        {
            int startX = x - (list.Count * 140) / 2;

            foreach (var stmt in list)
            {
                g.DrawLine(Pens.Black, x, y + 30, startX, y + 120);
                DrawNode(g, stmt, startX, y + 120);
                startX += 140;
            }
        }

        // ================= BOX =================
        private void DrawBox(Graphics g, string text, int x, int y)
        {
            var rect = new Rectangle(x - 60, y, 120, 30);

            g.FillRectangle(Brushes.White, rect);
            g.DrawRectangle(Pens.Black, rect);

            g.DrawString(text, _font, Brushes.Black, rect,
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
        }

        // ================= WIDTH =================
        private int MeasureWidth(AstNode node)
        {
            // Для терминала считаем ширину текста
            if (node is TerminalNode t)
            {
                using (var g = this.CreateGraphics())
                {
                    var size = g.MeasureString($"\"{t.Symbol}\"", _font);
                    return (int)size.Width + 40; // запас
                }
            }

            // Для идентификатора и литерала тоже считаем текст
            if (node is IdentifierNode id)
            {
                using (var g = this.CreateGraphics())
                {
                    var size = g.MeasureString(id.Name, _font);
                    return (int)size.Width + 40;
                }
            }
            if (node is IntLiteralNode lit)
            {
                using (var g = this.CreateGraphics())
                {
                    var size = g.MeasureString(lit.RawValue.ToString(), _font);
                    return (int)size.Width + 40;
                }
            }

            // Для нетерминала — минимум 120
            var children = GetChildren(node).ToList();
            if (children.Count == 0)
                return 120;

            // Сумма ширин детей + динамические промежутки
            int w = children.Sum(c => MeasureWidth(c)) + children.Count * 20;
            return Math.Max(w, 120);
        }




        // ================= LABEL =================
        private string GetLabel(AstNode node)
        {
            switch (node)
            {
                case WhileNode: return "While";
                case AssignNode: return "Assign";
                case CompareNode cmp: return $"Compare ({cmp.Op})";
                case IdentifierNode id: return $"Id: {id.Name}";
                case IntLiteralNode lit: return $"Int: {lit.RawValue}";
                default: return node.GetType().Name;
            }
        }

        // ================= CHILDREN =================
        private IEnumerable<AstNode> GetChildren(AstNode node)
        {
            switch (node)
            {
                case WhileNode w:
                    yield return new TerminalNode("while");
                    yield return new NonTerminalNode("Condition", w.Condition);
                    yield return new TerminalNode(":");
                    yield return new NonTerminalNode("Body", w.Body);
                    break;

                case AssignNode a:
                    yield return new IdentifierNode(a.Name);
                    yield return new TerminalNode(a.Op);
                    yield return a.Value;
                    yield return new TerminalNode(";");
                    break;

                case CompareNode c:
                    yield return c.Left;
                    yield return new TerminalNode(c.Op);
                    yield return c.Right;
                    break;
            }
        }

        // ================= SUPPORT NODES =================
        public class TerminalNode : AstNode
        {
            public string Symbol { get; }

            public TerminalNode(string symbol)
                : base(-1, -1)
            {
                Symbol = symbol;
            }
        }

        public class NonTerminalNode : AstNode
        {
            public string Name { get; }
            public object Child { get; }

            public NonTerminalNode(string name, object child)
                : base(-1, -1)
            {
                Name = name;
                Child = child;
            }
        }

        public sealed class IdentifierNode : ExprNode
        {
            public string Name { get; }

            public IdentifierNode(string name)
                : base(-1, -1)
            {
                Name = name;
            }

            public IdentifierNode(string name, int line, int col)
                : base(line, col)
            {
                Name = name;
            }
        }
    }
}
