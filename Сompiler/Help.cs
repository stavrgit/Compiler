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
        private string Lang => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        public void Show_Help()
        {
            string file = Lang == "ru" ? "Справка.html" : "Help.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.", Lang == "ru" ? "Справка" : "Help");
        }
        public void Show_About()
        {
            string file = Lang == "ru" ? "О программе.html" : "About.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.", Lang == "ru" ? "О программе" : "About");
        }
        public void Show_Task()
        {
            string file = "Постановка задачи.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.", Lang == "ru" ? "Постановка задачи" : "Task");
        }
        public void Show_Grammar()
        {
            string file = "Грамматика.html"; 
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Грамматика" : "Grammar");
        }
        public void Show_GrammarClassification()
        {
            string file = "Классификация грамматики.html"; 
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Классификация грамматики" : "Grammar Classification");
        }
        public void Show_AnalysisMethod()
        {
            string file = "Метод анализа.html"; 
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Метод анализа" : "Analysis Method");
        }
        public void Show_TestExample()
        {
            string file = "Тестовый пример.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Тестовый пример" : "Test Example");
        }
        public void Show_Literature()
        {
            string file = "Список литературы.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Список литературы" : "References");
        }
        public void Show_SourceCode()
        {
            string file = "Исходный код.html";
            string path = Path.Combine(Application.StartupPath, file);

            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            else
                MessageBox.Show($"Файл {file} не найден.",
                                Lang == "ru" ? "Исходный код" : "Source Code");
        }
    }
}
