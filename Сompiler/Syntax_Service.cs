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
        private readonly Style commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        private readonly Style numberStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular);

        public void Highlight(FastColoredTextBox editor)
        {
            var range = editor.Range;

            range.ClearStyle(keywordStyle, commentStyle, numberStyle);
            range.SetStyle(commentStyle, @"//.*$", RegexOptions.Multiline);
            range.SetStyle(numberStyle, @"\b\d+\b");

            string keywords = string.Join("|", Syntax_Words.Keywords.Select(Regex.Escape));
            range.SetStyle(keywordStyle, $@"\b({keywords})\b");
        }
    }

}
