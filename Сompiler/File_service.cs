using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastColoredTextBoxNS;

namespace Сompiler
{
    public class File_service
    {
        public string? CurrentFilePath { get; private set; }
        public bool IsModified { get; set; }
        public string CurrentFileName { get; private set; } = "Новый файл";
        private int newFileCounter = 1;

        public void New_File(FastColoredTextBox editor)
        {
            CurrentFileName = $"Новый файл_{newFileCounter}";
            newFileCounter++;
            CurrentFilePath = null;
            IsModified = false;
            editor?.Clear();
        }
        public void Load_File(FastColoredTextBox editor, string path)
        {
            editor.Text = File.ReadAllText(path);
            CurrentFilePath = path;
            CurrentFileName = Path.GetFileName(path);
            IsModified = false;
        }
        public bool Save_File(FastColoredTextBox editor, string? path)
        {
            try
            {
                if (path == null)
                {
                    using SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return false;

                    path = sfd.FileName;
                }

                File.WriteAllText(path, editor.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}