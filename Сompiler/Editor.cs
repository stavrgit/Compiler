using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastColoredTextBoxNS;

namespace Сompiler
{
    public class Editor
    {
        public void Undo(FastColoredTextBox edit)
        {
            edit.Undo();
        }

        public void Redo(FastColoredTextBox edit)
        {
            edit.Redo();
        }

        public void Cut(FastColoredTextBox edit)
        {
            edit.Cut();
        }

        public void Copy(FastColoredTextBox edit)
        {
            edit.Copy();
        }

        public void Paste(FastColoredTextBox edit)
        {
            edit.Paste();
        }

        public void Delete(FastColoredTextBox edit)
        {
            edit.ClearSelected();
        }

        public void SelectAll(FastColoredTextBox edit)
        {
            edit.SelectAll();
        }
    }
}