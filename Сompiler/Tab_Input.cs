using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Tab_Input
    {
        private readonly Form1 form;
        private readonly HotkeyManager hotkeys;
        private readonly StatusBarService status;

        public Tab_Input(Form1 form, HotkeyManager hotkeys, StatusBarService status)
        {
            this.form = form;
            this.hotkeys = hotkeys;
            this.status = status;
        }

        public TabPage Create(string title, string text)
        {
            var tab = new TabPage(title);
            var editor = new FastColoredTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 12),
                Text = text,
                BorderStyle = BorderStyle.None,
                Language = Language.Custom,
                ShowLineNumbers = true,
                WordWrap = false,
                AllowDrop = true
            };

            editor.SelectionChanged += (s, e) => status.UpdateStatus();
            editor.TextChanged += (s, e) =>
            {
                form.file_service.IsModified = true;
                form._typingTimer.Stop();
                form._typingTimer.Start();
            };

            editor.DragEnter += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
            };

            editor.DragDrop += (s, e) =>
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string path = files[0];
                    form.file_service.Load_File(editor, path);

                    tab.Text = Path.GetFileName(path);
                    tab.Tag = path;

                    form.syntax.Highlight(editor);
                    status.UpdateStatus();
                }
            };

            tab.Controls.Add(editor);
            hotkeys.Attach(editor);

            return tab;
        }
    }

}
