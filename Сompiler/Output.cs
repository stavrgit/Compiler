using System.Windows.Forms;

namespace Сompiler
{
    public class Output
    {
        private readonly TabControl tabControlOutput;
        private readonly DataGridView dataGridParser;  
        private readonly DataGridView gridScanner;      

        public Output(TabControl tabControlOutput, DataGridView dataGridParser, DataGridView gridScanner)
        {
            this.tabControlOutput = tabControlOutput;
            this.dataGridParser = dataGridParser;
            this.gridScanner = gridScanner;
        }

        public void AddParserError(string fragment, int line, int column, string message)
        {
            dataGridParser.Rows.Add(fragment, $"{line}:{column}", message);
            tabControlOutput.SelectedIndex = 0; 
        }

        public void ClearParserErrors()
        {
            dataGridParser.Rows.Clear();
        }

        public void AddScannerToken(int code, string type, string lexeme, string position)
        {
            gridScanner.Rows.Add(code, type, lexeme, position);
            tabControlOutput.SelectedIndex = 1; 
        }

        public void ClearScannerTokens()
        {
            gridScanner.Rows.Clear();
        }
    }
}