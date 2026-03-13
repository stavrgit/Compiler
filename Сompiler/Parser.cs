using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public static class Parser
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

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(error))
                        output += "\n[ERROR]\n" + error;

                    return output;
                }
            }
            catch (Exception ex)
            {
                return "Ошибка запуска parser.exe:\n" + ex.Message;
            }
        }
    }
}
