using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class StatusBarService
    {
        private readonly Form1 form;

        public StatusBarService(Form1 form)
        {
            this.form = form;
        }

        public void UpdateStatus()
        {
            var editor = form.GetCurrentEditor();
            if (editor == null)
            {
                form.statusFileName.Text = "";
                form.statusCursor.Text = "";
                form.statusLines.Text = "";
                form.statusSize.Text = "";
                return;
            }

            form.statusFileName.Text = form.tabControlEditor.SelectedTab.Text +
                (form.file_service.IsModified ? " *" : "");

            int line = editor.Selection.Start.iLine + 1;
            int col = editor.Selection.Start.iChar + 1;
            form.statusCursor.Text = $"Line: {line}, Column: {col}";

            form.statusLines.Text = $"Lines: {editor.LinesCount}";

            if (form.tabControlEditor.SelectedTab.Tag is string path)
            {
                long size = new FileInfo(path).Length;
                form.statusSize.Text = $"Size: {size} bytes";
            }
            else
            {
                form.statusSize.Text = $"Size: {Encoding.UTF8.GetByteCount(editor.Text)} bytes";
            }

            form.statusLang.Text = "Lang: " + Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
        }
    }

}
