using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FastColoredTextBoxNS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private Dictionary<TabPage, Rectangle> closeButtons = new();
        public Form1()
        {
            InitializeComponent();
            tabControlEditor.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlEditor.Padding = new Point(20, 4);
            tabControlEditor.DrawItem += TabControlEditor_DrawItem;
            tabControlEditor.MouseDown += TabControlEditor_MouseDown;
            _output = new Output(tabControlOutput, dataGridParser, gridScanner);
            Clicks();
            tabControlEditor.SelectedIndexChanged += (s, e) => status.UpdateStatus();
            hotkeys = new HotkeyManager(this);
            syntax = new Syntax_Service();
            status = new StatusBarService(this);
            tab_Input = new Tab_Input(this, hotkeys, status);
            this.FormClosing += Form1_FormClosing;
            _typingTimer.Interval = 150;
            _typingTimer.Tick += (s, e) =>
            {
                _typingTimer.Stop();
                status.UpdateStatus();
            };
        }
        private void ReloadForm()
        {
            var savedTabs = new List<(string title, string text, string? path, bool modified)>();

            foreach (TabPage tab in tabControlEditor.TabPages)
            {
                if (tab.Controls.Count == 0)
                    continue;

                var editor = tab.Controls[0] as FastColoredTextBox;
                if (editor == null)
                    continue;

                var info = tab.Tag as File_Info;

                savedTabs.Add((
                    tab.Text,
                    editor.Text,
                    info?.Path,
                    info?.IsModified ?? false
                ));
            }

            Controls.Clear();
            InitializeComponent();
            Clicks();

            hotkeys = new HotkeyManager(this);
            syntax = new Syntax_Service();
            status = new StatusBarService(this);
            tab_Input = new Tab_Input(this, hotkeys, status);

            foreach (var t in savedTabs)
            {
                var tab = tab_Input.Create(t.title, t.text);
                var info = tab.Tag as File_Info;
                info.Path = t.path;
                info.IsModified = t.modified;

                tabControlEditor.TabPages.Add(tab);
            }

            status.UpdateStatus();
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

            toolStripButton9.Click += buttonRun_Click;
            //пускToolStripMenuItem.Click += buttonRun_Click;
            парсерToolStripMenuItem.Click += buttonRun_Click;
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
            var tab = New_Tab("Новый файл", "");
            var editor = GetCurrentEditor();

            file_service.New_File(editor);

            if (tab.Tag is File_Info info)
                info.IsModified = false;

            tab.Text = file_service.CurrentFileName;
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
                if (tabControlEditor.SelectedTab.Tag is File_Info info)
                {
                    info.Path = dialog.FileName;
                    info.IsModified = false;
                }
            }
        }
        internal void Save_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;

            if (tabControlEditor.SelectedTab.Tag is File_Info info)
            {
                string? newPath = file_service.Save_File(editor, info.Path);

                if (newPath == null)
                    return;

                info.Path = newPath;
                info.IsModified = false;
                tabControlEditor.SelectedTab.Text = Path.GetFileName(newPath);
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
        internal void выходToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();
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
        private TabPage New_Tab(string title, string text = "")
        {
            var tab = tab_Input.Create(title, text);
            tabControlEditor.TabPages.Add(tab);
            tabControlEditor.SelectedTab = tab;
            return tab;
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

            closeButtons[tab] = closeRect;
        }
        private void TabControlEditor_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControlEditor.TabPages.Count; i++)
            {
                var tab = tabControlEditor.TabPages[i];

                if (closeButtons.TryGetValue(tab, out Rectangle rect) && rect.Contains(e.Location))
                {
                    tabControlEditor.TabPages.Remove(tab);
                    break;
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage tab in tabControlEditor.TabPages)
            {
                if (tab.Tag is File_Info info && info.IsModified)
                {
                    var editor = tab.Controls[0] as FastColoredTextBox;

                    var result = MessageBox.Show(
                        string.Format(
                            Lang(
                                "Файл \"{0}\" содержит несохранённые изменения. Сохранить?",
                                "File \"{0}\" has unsaved changes. Save?"
                            ),
                            tab.Text
                        ),
                        Lang("Подтверждение", "Confirmation"),
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (result == DialogResult.Yes)
                    {
                        string? newPath = file_service.Save_File(editor, info.Path);

                        if (newPath == null)
                        {
                            e.Cancel = true;
                            return;
                        }

                        info.Path = newPath;
                        info.IsModified = false;
                        tab.Text = Path.GetFileName(newPath);
                    }
                }
            }

            var exitResult = MessageBox.Show(
                Lang(
                    "Вы действительно хотите выйти?",
                    "Are you sure you want to exit?"
                ),
                Lang(
                    "Подтверждение выхода",
                    "Exit confirmation"
                ),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (exitResult == DialogResult.No)
                e.Cancel = true;
        }

        private string Lang(string ru, string en)
        {
            return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ru" ? ru : en;
        }
        internal void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (FastColoredTextBox)sender;
            syntax.Highlight(editor, e);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null)
                return;

            string code = editor.Text;

            _output.ClearParserErrors();
            _output.ClearScannerTokens();

            var scanner = new Scanner(code);
            var tokens = scanner.Analyze();

            foreach (var t in tokens)
            {
                string pos = $"{t.Line}:{t.Start}-{t.End}";
                _output.AddScannerToken(t.Code, t.Type, t.Lexeme, pos);
            }

            var parser = new Parser(tokens);
            parser.ParseProgram();

            bool hasErrors = parser.Errors.Count > 0;

            if (hasErrors)
            {
                foreach (var err in parser.Errors)
                {
                    int row = dataGridParser.Rows.Add(err.Fragment, $"{err.Line}:{err.Col}", err.Message);

                    var r = dataGridParser.Rows[row];
                    r.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                    r.DefaultCellStyle.ForeColor = Color.DarkRed;
                }

                MessageBox.Show("Обнаружены ошибки в коде.", "Парсер", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = dataGridParser.Rows.Add("—", "—", "Ошибок не обнаружено");

                var r = dataGridParser.Rows[row];
                r.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200);
                r.DefaultCellStyle.ForeColor = Color.DarkGreen;

                MessageBox.Show("Ошибок не обнаружено.", "Парсер", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            tabControlOutput.SelectedIndex = 0;
        }

        private void gridScanner_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var editor = GetCurrentEditor();
            if (editor == null) return;

            string pos = gridScanner.Rows[e.RowIndex].Cells[3].Value?.ToString();
            if (string.IsNullOrWhiteSpace(pos)) return;

            var p = pos.Split(':', '-');
            int line = int.Parse(p[0].Trim()) - 1;
            int start = int.Parse(p[1].Trim()) - 1;
            int end = int.Parse(p[2].Trim());

            editor.Selection.Start = new Place(start, line);
            editor.Selection.End = new Place(end, line);
            editor.Navigate(line);
            editor.Focus();
        }

        private void dataGridParser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridParser.CurrentRow;
            if (row == null)
                return;

            string location = row.Cells[1].Value?.ToString();
            if (string.IsNullOrWhiteSpace(location))
                return;

            MessageBox.Show(location);

            var parts = location.Split(':');
            if (parts.Length != 2)
                return;

            if (!int.TryParse(parts[0].Trim(), out int line))
                return;

            string colPart = parts[1].Split('-')[0].Trim();

            if (!int.TryParse(colPart, out int pos))
                return;

            NavigateToPosition(line, pos);
        }

        private void NavigateToPosition(int line, int pos)
        {
            var editor = GetCurrentEditor();
            if (editor == null || editor.LinesCount == 0)
                return;

            int lineIndex = Math.Max(0, line - 1);
            if (lineIndex >= editor.LinesCount)
                return;

            int columnIndex = Math.Max(0, pos - 1);

            int lineLength = editor.Lines[lineIndex].Length;
            columnIndex = Math.Min(columnIndex, lineLength);

            var place = new FastColoredTextBoxNS.Place(columnIndex, lineIndex);

            editor.Selection.Start = place;
            editor.Selection.End = place;

            editor.DoSelectionVisible();
            editor.Focus();
        }

        private void antlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetCurrentEditor();
            if (editor == null)
                return;

            dataGridParser.Rows.Clear();
            string code = editor.Text;

            var inputStream = new Antlr4.Runtime.AntlrInputStream(code);
            var antlrLexer = new antlerLexer(inputStream);
            var antlrTokens = new Antlr4.Runtime.CommonTokenStream(antlrLexer);
            var antlrParser = new antlerParser(antlrTokens);

            var antlrErrors = new List<string>();

            antlrParser.RemoveErrorListeners();
            antlrParser.AddErrorListener(new AntlrErrorCollector(antlrErrors));

            antlrParser.program();

            if (antlrErrors.Count > 0)
            {
                foreach (var err in antlrErrors)
                {
                    int row = dataGridParser.Rows.Add("ANTLR", "—", err);
                    var r = dataGridParser.Rows[row];
                    r.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 200);
                    r.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }
            else
            {
                int row = dataGridParser.Rows.Add("ANTLR", "—", "Ошибок нет");
                var r = dataGridParser.Rows[row];
                r.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
                r.DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void постановкаЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_Task();
        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_Grammar();
        }

        private void класификацияГрамматикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_GrammarClassification();
        }

        private void методАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_AnalysisMethod();
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_TestExample();
        }

        private void списокЛитературыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_Literature();
        }

        private void исходныйКодПргограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show_SourceCode();
        }
    }
}