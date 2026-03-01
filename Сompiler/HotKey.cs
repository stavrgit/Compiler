using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastColoredTextBoxNS;

namespace Сompiler
{
    public class HotkeyManager
    {
        private readonly Form1 form;

        public HotkeyManager(Form1 form)
        {
            this.form = form;
        }

        public void Attach(FastColoredTextBox editor)
        {
            editor.KeyDown += Editor_KeyDown;
        }

        private void Editor_KeyDown(object? sender, KeyEventArgs e)
        {
            var editor = sender as FastColoredTextBox;
            if (editor == null) return;

            // --- ФАЙЛЫ ---
            if (e.Control && e.KeyCode == Keys.N)
            {
                form.New_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.O)
            {
                form.Open_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                form.Save_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            // --- РЕДАКТИРОВАНИЕ ---
            if (e.Control && e.KeyCode == Keys.Z)
            {
                form.Undo_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.Y)
            {
                form.Redo_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.X)
            {
                form.Cut_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                form.Copy_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                form.Paste_Click(null, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                editor.SelectAll();
                e.SuppressKeyPress = true;
            }
        }
    }
}

