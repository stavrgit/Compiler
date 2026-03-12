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
    }
}
