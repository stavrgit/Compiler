using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public static class Parser_Bison
    {
        public static string RunParser(string input)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "parser.exe",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            try
            {
                using (var process = Process.Start(psi))
                {
                    process.StandardInput.Write(input);
                    process.StandardInput.Close();

                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    string combined = stdout + "\n" + stderr;

                    if (combined.Contains("Lexical error"))
                    {
                        string msg = ExtractError(combined);
                        return "Лексическая ошибка:\n" + msg;
                    }

                    if (combined.Contains("Syntax error"))
                    {
                        string msg = ExtractError(combined);
                        return "Синтаксическая ошибка:\n" + msg;
                    }
                    return "Синтаксический анализ: все хорошо";
                }
            }
            catch (Exception ex)
            {
                return "Ошибка запуска parser.exe:\n" + ex.Message;
            }
        }

        private static string ExtractError(string text)
        {
            foreach (var line in text.Split('\n'))
            {
                if (line.Contains("error", StringComparison.OrdinalIgnoreCase))
                    return line.Trim();
            }
            return text.Trim();
        }
    }

}
