using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Editor
    {
        public void Undo(RichTextBox edit)
        {
            if (edit.CanUndo)
                edit.Undo();
        }

        public void Redo(RichTextBox edit)
        {
            if (edit.CanRedo)
                edit.Redo();
        }

        public void Cut(RichTextBox edit)
        {
            edit.Cut();
        }

        public void Copy(RichTextBox edit)
        {
            edit.Copy();
        }

        public void Paste(RichTextBox edit)
        {
            edit.Paste();
        }

        public void Delete(RichTextBox edit)
        {
            edit.SelectedText = "";
        }

        public void SelectAll(RichTextBox edit)
        {
            edit.SelectAll();
        }
        public void ShowHelp() { }
        public void ShowAbout() { }
    }

}
