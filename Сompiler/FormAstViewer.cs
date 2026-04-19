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
        private const int NODE_WIDTH = 120;
        private const int NODE_HEIGHT = 30;
        private const int H_SPACING = 40;   // расстояние между узлами
        private const int V_SPACING = 90;   // расстояние по вертикали

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
                int childY = y + 80; // 🔥 одинаковый отступ для всех терминалов

                var size = g.MeasureString(t.Symbol, _font);
                int w = (int)size.Width + 20;

                var rect = new Rectangle(x - w / 2, childY, w, 30);

                g.FillRectangle(Brushes.White, rect);
                g.DrawRectangle(Pens.Black, rect);

                g.DrawString(t.Symbol, _font, Brushes.Black, rect,
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });

                // 🔥 линия строго в центр терминала
                g.DrawLine(
                    Pens.Black,
                    x,
                    y + 30,
                    x,
                    childY
                );

                return;
            }

            if (node is NonTerminalNode nt)
            {
                DrawBox(g, nt.Name, x, y);

                if (nt.Child is AstNode childNode)
                {
                    g.DrawLine(Pens.Black, x, y + NODE_HEIGHT, x, y + V_SPACING);

                    DrawNode(g, childNode, x, y + V_SPACING);
                }

                return;
            }
            if (node is StmtListNode list)
            {
                DrawBox(g, "Body", x, y);

                int stmtTotalWidth = list.Statements.Sum(s => MeasureWidth(g, s)) + (list.Statements.Count - 1) * H_SPACING;
                int stmtStartX = x - stmtTotalWidth / 2 - 50;

                foreach (var stmt in list.Statements)
                {
                    int w = MeasureWidth(g, stmt);
                    int childX = stmtStartX + w / 2;
                    int childY = y + V_SPACING;

                    g.DrawLine(Pens.Black, x, y + NODE_HEIGHT, childX, childY);
                    DrawNode(g, stmt, childX, childY);

                    stmtStartX += w + H_SPACING;
                }
                return;
            }
            if (node is ConditionNode cond)
            {
                DrawBox(g, "Condition", x, y);

                int condtotalWidth = cond.Parts.Sum(p => MeasureWidth(g, p)) + (cond.Parts.Count - 1) * H_SPACING;
                int condstartX = x - condtotalWidth / 2;

                foreach (var part in cond.Parts)
                {
                    int w = MeasureWidth(g, part);
                    int childX = condstartX + w / 2;
                    int childY = y + V_SPACING;

                    g.DrawLine(Pens.Black, x, y + NODE_HEIGHT, childX, childY);
                    DrawNode(g, part, childX, childY);

                    condstartX += w + H_SPACING;
                }
                return;
            }



            // обычный узел
            string label = GetLabel(node);
            DrawBox(g, label, x, y);

            var children = GetChildren(node).ToList();
            if (children.Count == 0) return;

            int totalWidth = children.Sum(c => MeasureWidth(g, c)) + (children.Count - 1) * H_SPACING;
            int startX = x - totalWidth / 2;

            foreach (var child in children)
            {
                int w = MeasureWidth(g, child);
                int childX = startX + w / 2;
                int childY = y + V_SPACING;

                // линия аккуратно вниз
                g.DrawLine(Pens.Black, x, y + NODE_HEIGHT, childX, childY);

                DrawNode(g, child, childX, childY);

                startX += w + H_SPACING;
            }
        }


       

        // ================= BOX =================
        private void DrawBox(Graphics g, string text, int x, int y)
        {
            var rect = new Rectangle(x - NODE_WIDTH / 2, y, NODE_WIDTH, NODE_HEIGHT);

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
        private int MeasureWidth(Graphics g, AstNode node)
        {
            string label = GetLabel(node);
            var size = g.MeasureString(label, _font);

            int baseWidth = Math.Max((int)size.Width + 40, NODE_WIDTH);

            if (node is ConditionNode cond)
            {
                int childrenWidth = cond.Parts.Sum(p => MeasureWidth(g, p)) + (cond.Parts.Count - 1) * H_SPACING;
                return Math.Max(NODE_WIDTH, childrenWidth);
            }
            if (node is StmtListNode list)
            {
                int childrenWidth = list.Statements.Sum(s => MeasureWidth(g, s)) + (list.Statements.Count - 1) * H_SPACING;
                return Math.Max(NODE_WIDTH, childrenWidth);
            }

            var children = GetChildren(node).ToList();
            if (children.Count == 0) return baseWidth;

            int totalWidth = children.Sum(c => MeasureWidth(g, c)) + (children.Count - 1) * H_SPACING;
            return Math.Max(baseWidth, totalWidth);
        }


        // ================= LABEL =================
        private string GetLabel(AstNode node)
        {
            switch (node)
            {
                case WhileNode: return "While";
                case AssignNode: return "Assign";
                case CompareNode cmp: return $"Compare ({cmp.Op})";
                case IdentifierNode id: return $"{id.Name}";
                case IntLiteralNode lit: return $"{lit.RawValue}";
                default: return node.GetType().Name;
            }
        }

        // ================= CHILDREN =================
        private IEnumerable<AstNode> GetChildren(AstNode node)
        {
            switch (node)
            {
                case WhileNode w:
                    yield return new NonTerminalNode("Modifiers", new TerminalNode("while"));
                    yield return new ConditionNode(new List<AstNode> {
                                    w.Condition,
                                    new TerminalNode(":")
                                                        });
                    yield return new StmtListNode(w.Body);
                    break;

                case CompareNode c:
                    yield return new NonTerminalNode("Identifier name", c.Left);
                    yield return new NonTerminalNode("Compare", new TerminalNode(c.Op));
                    yield return new NonTerminalNode("IntLiteral value", c.Right);
                    break;

                case AssignNode a:
                    yield return new NonTerminalNode("Identifier name", new TerminalNode(a.Name));
                    yield return new NonTerminalNode("Assign", new TerminalNode(a.Op));
                    yield return new NonTerminalNode("IntLiteral value", a.Value);
                    yield return new NonTerminalNode("Semicolon", new TerminalNode(";"));
                    break;



            }
        }
    }
}
