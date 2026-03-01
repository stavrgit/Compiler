using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace Сompiler
{
    public partial class Form1 : Form
    {
        internal readonly File_service file_service = new();
        private readonly Editor _editor = new();
        private readonly Help help = new();
        internal System.Windows.Forms.Timer _typingTimer = new System.Windows.Forms.Timer();
        private Output _output;
        private HotkeyManager hotkeys;
        private StatusBarService status;
        public Syntax_Service syntax;
        private Tab_Input tab_Input;

        public Form1()
        {
            InitializeComponent();
            tabControlEditor.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlEditor.Padding = new Point(20, 4);
            tabControlEditor.DrawItem += TabControlEditor_DrawItem;
            tabControlEditor.MouseDown += TabControlEditor_MouseDown;
            _output = new Output(tabControlOutput, txtResults, gridErrors);
            Clicks();
            tabControlEditor.SelectedIndexChanged += (s, e) => status.UpdateStatus();
            hotkeys = new HotkeyManager(this);
            syntax = new Syntax_Service();
            status = new StatusBarService(this);
            tab_Input = new Tab_Input(this, hotkeys, status);
            _typingTimer.Interval = 150;
            _typingTimer.Tick += (s, e) =>
            {
                _typingTimer.Stop();

                var editor = GetCurrentEditor();
                if (editor != null)
                    syntax.Highlight(editor);

                status.UpdateStatus();
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
        internal FastColoredTextBox? GetCurrentEditor()
        {
            if (tabControlEditor.TabCount == 0)
                return null;

            var tab = tabControlEditor.SelectedTab;
            if (tab == null || tab.Controls.Count == 0)
                return null;

            return tab.Controls[0] as FastColoredTextBox;
        }
        internal void New_Click(object sender, EventArgs e)
        {
            New_Tab("Новый файл", "");
            var editor = GetCurrentEditor();
            file_service.New_File(GetCurrentEditor());
            tabControlEditor.SelectedTab.Text = file_service.CurrentFileName;
            status.UpdateStatus();
        }
        internal void Open_Click(object sender, EventArgs e)
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
        internal void Save_Click(object sender, EventArgs e)
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
        internal void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
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
        internal void выходToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        internal void Undo_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Undo(editor);
        }
        internal void Redo_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Redo(editor);
        }
        internal void Cut_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Cut(editor);
        }
        internal void Copy_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Copy(editor);
        }
        internal void Paste_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Paste(editor);
        }
        internal void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            _editor.Delete(editor);
        }
        internal void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
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
            var tab = tab_Input.Create(title, text);
            tabControlEditor.TabPages.Add(tab);
            tabControlEditor.SelectedTab = tab;
        }
        private void TabControlEditor_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tabControlEditor.TabPages[e.Index];
            var rect = e.Bounds;
            int paddingRight = 12; 
            int size = 7;         
            int offsetTop = 3;     

            TextRenderer.DrawText(
                e.Graphics,
                tab.Text,
                tab.Font,
                new Point(rect.X + 8, rect.Y + 6),
                tab.ForeColor
            );
            Rectangle closeRect = new Rectangle(
                rect.Right - paddingRight,
                rect.Top + offsetTop,
                size,
                size
            );

            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawLine(pen, closeRect.Left, closeRect.Top, closeRect.Right, closeRect.Bottom);
                e.Graphics.DrawLine(pen, closeRect.Right, closeRect.Top, closeRect.Left, closeRect.Bottom);
            }

            tab.Tag = closeRect;
        }
        private void TabControlEditor_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControlEditor.TabPages.Count; i++)
            {
                var tab = tabControlEditor.TabPages[i];

                if (tab.Tag is Rectangle closeRect && closeRect.Contains(e.Location))
                {
                    tabControlEditor.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
    }
}