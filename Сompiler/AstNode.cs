using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public abstract class AstNode
    {
        public int Line { get; }
        public int Column { get; }

        protected AstNode(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }

    public sealed class WhileNode : AstNode
    {
        public ExprNode Condition { get; }
        public List<StmtNode> Body { get; }

        public WhileNode(ExprNode condition, List<StmtNode> body, int line, int column)
            : base(line, column)
        {
            Condition = condition;
            Body = body;
        }
    }

    public abstract class StmtNode : AstNode
    {
        protected StmtNode(int line, int column) : base(line, column) { }
    }

    public sealed class AssignNode : StmtNode
    {
        public string Name { get; }
        public string Op { get; }
        public ExprNode Value { get; }

        public AssignNode(string name, string op, ExprNode value, int line, int column)
            : base(line, column)
        {
            Name = name;
            Op = op;
            Value = value;
        }
    }

    public abstract class ExprNode : AstNode
    {
        protected ExprNode(int line, int column) : base(line, column) { }
    }

    public sealed class CompareNode : ExprNode
    {
        public ExprNode Left { get; }
        public string Op { get; }
        public ExprNode Right { get; }

        public CompareNode(ExprNode left, string op, ExprNode right, int line, int column)
            : base(line, column)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }

    public sealed class IdentifierNode : ExprNode
    {
        public string Name { get; }

        public IdentifierNode(string name, int line, int column)
            : base(line, column)
        {
            Name = name;
        }
    }

    public sealed class IntLiteralNode : ExprNode
    {
        public string RawValue { get; set; }

        public IntLiteralNode(string raw, int line, int column)
            : base(line, column)
        {
            RawValue = raw;
        }
    }


}
