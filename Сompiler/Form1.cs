using System.ComponentModel.Design;

namespace Сompiler
{
    public partial class Form1 : Form
    {
        private readonly File_service file_service = new();
        private readonly Editor _editor = new();
        private readonly Hepl help = new();

        public Form1()
        {
            InitializeComponent();

            richTextBox1.TextChanged += (s, e) => file_service.IsModified = true;

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

        // ===== ФАЙЛ =====

        private void New_Click(object sender, EventArgs e)
        {
            file_service.NewFile(richTextBox1);
        }

        private void Open_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Text files|*.txt|All files|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
                file_service.LoadFile(richTextBox1, ofd.FileName);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (file_service.CurrentFilePath == null)
            {
                сохранитьКакToolStripMenuItem_Click(sender, e);
                return;
            }

            file_service.SaveFile(richTextBox1, file_service.CurrentFilePath);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = "Text files|*.txt|All files|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
                file_service.SaveFile(richTextBox1, sfd.FileName);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
        private void Undo_Click(object sender, EventArgs e) => _editor.Undo(richTextBox1);
        private void Redo_Click(object sender, EventArgs e) => _editor.Redo(richTextBox1);
        private void Cut_Click(object sender, EventArgs e) => _editor.Cut(richTextBox1);
        private void Copy_Click(object sender, EventArgs e) => _editor.Copy(richTextBox1);
        private void Paste_Click(object sender, EventArgs e) => _editor.Paste(richTextBox1);
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e) => _editor.Delete(richTextBox1);
        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e) => _editor.SelectAll(richTextBox1);
        private void Help_Click(object sender, EventArgs e) => _editor.ShowHelp();
        private void About_Click(object sender, EventArgs e) => _editor.ShowAbout();

     
    }

}
