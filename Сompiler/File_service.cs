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
            CurrentFileName = $"Новый файл_{newFileCounter++}";
            CurrentFilePath = null;
            IsModified = false;
            editor.Text = ""; 
        }

        public void Load_File(FastColoredTextBox editor, string path)
        {
            editor.Text = File.ReadAllText(path);
            CurrentFilePath = path;
            CurrentFileName = Path.GetFileName(path);
            IsModified = false;
        }
        public string? Save_File(FastColoredTextBox editor, string? path)
        {
            try
            {
                if (path == null)
                {
                    using SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return null;

                    path = sfd.FileName;
                }

                File.WriteAllText(path, editor.Text);
                CurrentFilePath = path;
                CurrentFileName = Path.GetFileName(path);
                IsModified = false;
                return path;
            }
            catch
            {
                return null;
            }
        }
    }
}