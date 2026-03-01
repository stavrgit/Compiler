using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Help
    {
        public void Show_Help()
        {
            string path = Path.Combine(Application.StartupPath, "help.html");
            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show("Файл справки не найден.", "Справка");
        }
        public void Show_About()
        {
            string path = Path.Combine(Application.StartupPath, "about.html");
            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show("Файл about.html не найден.", "О программе");
        }

    }


}
