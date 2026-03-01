using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Output
    {
        private readonly TextBox txtResults;
        private readonly DataGridView gridErrors;
        private readonly TabControl tabControl;

        public Output(TabControl tabControl, TextBox txtResults, DataGridView gridErrors)
        {
            this.tabControl = tabControl;
            this.txtResults = txtResults;
            this.gridErrors = gridErrors;
        }

        public void AddResult(string text)
        {
            txtResults.AppendText(text + Environment.NewLine);
            tabControl.SelectedIndex = 0;
        }

        public void AddError(string file, int line, int column, string message)
        {
            gridErrors.Rows.Add(file, line, column, message);
            tabControl.SelectedIndex = 1;
        }
        public void ClearResults() => txtResults.Clear();
        public void ClearErrors() => gridErrors.Rows.Clear();
    }

}
