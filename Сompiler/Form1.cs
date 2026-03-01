using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace Сompiler
{
    public partial class Form1 : Form
    {
        private readonly File_service file_service = new();
        private readonly Editor _editor = new();
        private readonly Help help = new();
        private System.Windows.Forms.Timer _typingTimer = new System.Windows.Forms.Timer();
        private readonly Style keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        private readonly Style commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        private readonly Style numberStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular);


        public Form1()
        {
            InitializeComponent();
            Clicks();
            tabControlEditor.SelectedIndexChanged += (s, e) => UpdateStatus();
            _typingTimer.Interval = 150;
            _typingTimer.Tick += (s, e) =>
            {
                _typingTimer.Stop();

                var editor = GetCurrentEditor();
                if (editor != null)
                    HighlightSyntax(editor);

                UpdateStatus();
            };
        }
        private void ReloadForm()
        {
            Controls.Clear();
            InitializeComponent();
            Clicks();
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            ReloadForm();
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            ReloadForm();
        }
        private void Clicks()
        {
            создатьToolStripMenuItem.Click += New_Click;
            toolStripButton1.Click += New_Click;

            открытьToolStripMenuItem.Click += Open_Click;
            toolStripButton2.Click += Open_Click;

            сохранитьToolStripMenuItem.Click += Save_Click;
            toolStripButton3.Click += Save_Click;

            отменитьToolStripMenuItem.Click += Undo_Click;
            toolStripButton4.Click += Undo_Click;

            повторToolStripMenuItem.Click += Redo_Click;
            toolStripButton5.Click += Redo_Click;

            вырезатьToolStripMenuItem.Click += Cut_Click;
            toolStripButton7.Click += Cut_Click;

            копироватьToolStripMenuItem.Click += Copy_Click;
            toolStripButton6.Click += Copy_Click;

            вставитьToolStripMenuItem.Click += Paste_Click;
            toolStripButton8.Click += Paste_Click;

            вызовСправкиToolStripMenuItem.Click += Help_Click;
            toolStripButton10.Click += Help_Click;

            оПрограммеToolStripMenuItem.Click += About_Click;
            toolStripButton11.Click += About_Click;
        }
        private FastColoredTextBox? GetCurrentEditor()
        {
            if (tabControlEditor.TabCount == 0)
                return null;

            var tab = tabControlEditor.SelectedTab;
            if (tab == null || tab.Controls.Count == 0)
                return null;

            return tab.Controls[0] as FastColoredTextBox;
        }
        private void New_Click(object sender, EventArgs e)
        {
            New_Tab("Новый файл", "");
            var editor = GetCurrentEditor();
            file_service.New_File(GetCurrentEditor());
            tabControlEditor.SelectedTab.Text = file_service.CurrentFileName;
            UpdateStatus();
        }
        private void Open_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                New_Tab(Path.GetFileName(dialog.FileName), "");
                var editor = GetCurrentEditor();
                file_service.Load_File(editor, dialog.FileName);
                tabControlEditor.SelectedTab.Tag = dialog.FileName;
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;

            if (tabControlEditor.SelectedTab.Tag is string path)
            {
                file_service.Save_File(editor, path);
            }
            else
            {
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
        }
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;

            using var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                file_service.Save_File(editor, dialog.FileName);
                tabControlEditor.SelectedTab.Text = Path.GetFileName(dialog.FileName);
                tabControlEditor.SelectedTab.Tag = dialog.FileName;
            }
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        private void Undo_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Undo(editor);
        }
        private void Redo_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Redo(editor);
        }
        private void Cut_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Cut(editor);
        }
        private void Copy_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Copy(editor);
        }
        private void Paste_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Paste(editor);
        }
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Delete(editor);
        }
        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.SelectAll(editor);
        }
        private void Help_Click(object sender, EventArgs e) => help.Show_Help();
        private void About_Click(object sender, EventArgs e) => help.Show_About();
        private void размерШрифтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;

            using (FontDialog fd = new FontDialog())
            {
                fd.Font = editor.Font;

                if (fd.ShowDialog() == DialogResult.OK)
                    editor.Font = fd.Font;
            }
        }
        private void New_Tab(string title, string text = "")
        {
            var tab = new TabPage(title);
            tab.Tag = null;

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
            editor.SelectionChanged += (s, e) =>
            {
                UpdateStatus();
            };
            editor.TextChanged += (s, e) =>
            {
                file_service.IsModified = true;
                _typingTimer.Stop();
                _typingTimer.Start();
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

                    file_service.Load_File(editor, path);

                    tab.Text = Path.GetFileName(path);
                    tab.Tag = path;

                    HighlightSyntax(editor);
                    UpdateStatus();
                }
            };

            tab.Controls.Add(editor);
            tabControlEditor.TabPages.Add(tab);
            tabControlEditor.SelectedTab = tab;
        }
        private void UpdateStatus()
        {
            var editor = GetCurrentEditor();
            if (editor == null)
            {
                statusFileName.Text = "";
                statusCursor.Text = "";
                statusLines.Text = "";
                statusSize.Text = "";
                return;
            }

            statusFileName.Text = tabControlEditor.SelectedTab.Text + (file_service.IsModified ? " *" : "");

            int line = editor.Selection.Start.iLine + 1;
            int col = editor.Selection.Start.iChar + 1;
            statusCursor.Text = $"Line: {line}, Column: {col}";

            statusLines.Text = $"Lines: {editor.LinesCount}";

            if (tabControlEditor.SelectedTab.Tag is string path)
            {
                long size = new FileInfo(path).Length;
                statusSize.Text = $"Size: {size} bytes";
            }
            else
            {
                statusSize.Text = $"Size: {Encoding.UTF8.GetByteCount(editor.Text)} bytes";
            }

            statusLang.Text = "Lang: " + Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
        }
        private void HighlightSyntax(FastColoredTextBox editor)
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
