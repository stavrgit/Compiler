using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Syntax_Service
    {
        private readonly Style keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        private readonly Style numberStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular);
        private readonly Style operatorStyle = new TextStyle(Brushes.MediumSeaGreen, null, FontStyle.Bold);
        private readonly Style punctuationStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        private readonly Style identifierStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        internal readonly TextStyle errorStyle = new TextStyle(Brushes.Red, null, FontStyle.Underline);
        internal readonly TextStyle searchHighlightStyle = new TextStyle(Brushes.Black, Brushes.Yellow, FontStyle.Regular);


        public void Highlight(FastColoredTextBox editor, TextChangedEventArgs e)
        {
            var range = e.ChangedRange;

            range.ClearStyle(keywordStyle, numberStyle, operatorStyle, punctuationStyle, identifierStyle, errorStyle);

            range.SetStyle(keywordStyle, @"\bwhile\b");
            range.SetStyle(numberStyle, @"\b\d+\b");
            range.SetStyle(operatorStyle, @"==|!=|<=|>=|=|<|>");
            range.SetStyle(punctuationStyle, @"[:;]");
            range.SetStyle(identifierStyle, @"\b(?!while\b)[a-zA-Z][a-zA-Z0-9]*\b");
        }
        public void Highlight(FastColoredTextBox editor)
        {
            var range = editor.Range;

            range.ClearStyle(keywordStyle, numberStyle, operatorStyle, punctuationStyle, identifierStyle);

            range.SetStyle(keywordStyle, @"\bwhile\b");
            range.SetStyle(numberStyle, @"\b\d+\b");
            range.SetStyle(operatorStyle, @"==|!=|<=|>=|=|<|>");
            range.SetStyle(punctuationStyle, @"[:;]");
            range.SetStyle(identifierStyle, @"\b(?!while\b)[a-zA-Z][a-zA-Z0-9]*\b");
        }

        public void HighlightError(FastColoredTextBox editor, Token tok)
        {
            int line = tok.Line - 1;

            int start = tok.Start - 1;
            int end = tok.End;

            var range = editor.GetRange(
                new Place(start, line),
                new Place(end, line)
            );

            range.SetStyle(errorStyle);
        }
        public void ClearErrors(FastColoredTextBox editor)
        {
            editor.Range.ClearStyle(errorStyle);
        }
        public void HighlightSearch(FastColoredTextBox editor, int start, int length)
        {
            var range = editor.GetRange(start, start + length);
            range.SetStyle(searchHighlightStyle);
        }
        public void ClearSearch(FastColoredTextBox editor)
        {
            editor.Range.ClearStyle(searchHighlightStyle);
        }
    }
} 
