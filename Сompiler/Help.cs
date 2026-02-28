using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Hepl
    {
        public void ShowHelp()
        {
            MessageBox.Show("Это справка по программе.", "Справка");
        }

        public void ShowAbout()
        {
            MessageBox.Show("Текстовый редактор\nВерсия 1.0\nАвтор: Sasuke", "О программе");
        }
    }

}
