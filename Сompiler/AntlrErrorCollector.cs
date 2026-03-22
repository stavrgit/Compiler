using Antlr4.Runtime;
using System.Collections.Generic;
using System.IO;

public class AntlrErrorCollector : BaseErrorListener
{
    private readonly List<string> _errors;

    public AntlrErrorCollector(List<string> errors)
    {
        _errors = errors;
    }

    public override void SyntaxError(
        TextWriter output,
        IRecognizer recognizer,
        IToken offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e)
    {
        _errors.Add($"Строка {line}, позиция {charPositionInLine}: {msg}");
    }
}
