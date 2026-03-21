public class ParseError
{
    public string Fragment { get; }
    public int Line { get; }
    public int Col { get; }
    public string Message { get; }

    public ParseError(string fragment, int line, int col, string message)
    {
        Fragment = fragment;
        Line = line;
        Col = col;
        Message = message;
    }
}